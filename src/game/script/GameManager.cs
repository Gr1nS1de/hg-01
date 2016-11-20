using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Destructible2D;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

/// <summary>
/// Class in charge of the logic of the game. This class will restart the level at game over, handle and save the point, and call the Ads if you import the VERY SIMPLE ADS asset available here: http://u3d.as/oWD
/// </summary>
public class GameManager : MonoBehaviour
{

    public enum GameStatus
    {
        STARTED,
        GAMEOVER
    }

    public Color backgroundColor = Color.white;
    public Color circleColor = Color.black;
    public Color playerColor = Color.white;
    public Color hazardColor
    {
        get
        {
            return backgroundColor;
        }
    }

    public GameStatus m_GameStatus
    {
        get
        {
            return _currentGameStatus;
        }

        set
        {
            _currentGameStatus = value;
        }
    }

    public int m_MaxObstaclesCount = 10;
    public bool m_ActivateCameraShake = false;
    //	public int                  numberOfPlayToShowInterstitial = 5;
    public Text m_PointText;
    public GameObject m_CirclePrefab;
    public GameObject m_Particle;
    public GameObject m_ObstaclePrefab;
    public float m_ExpolsionForce = 200;


    private int _pointScore = 0;
    private Player _player;
    private SoundManager _soundManager;
    private GameStatus _currentGameStatus;
    private CanvasScaler _canvasScaler;
    private List<GameObject> _obstacleInstancesList;
    private Vector3 _expolsionPoint;
    [NonSerialized]
    public float m_RadiusBorder;
    private int _fractureCount = 10;

    void Awake()
    {
        _player = FindObjectOfType<Player>();
        _soundManager = FindObjectOfType<SoundManager>();
       
        if (Time.realtimeSinceStartup < 1)
            DOTween.Init();


        DOTween.KillAll();

        SetNewGame();

        GC.Collect();
        Resources.UnloadUnusedAssets();
        Application.targetFrameRate = 60;


        Init();


        if (FindObjectOfType<SpawnManager>() == null)
        {
            GameObject go = new GameObject();

            go.name = "_SpawnManager";

            go.AddComponent(typeof(SpawnManager));

            go.GetComponent<SpawnManager>().particle = this.m_Particle;
            go.GetComponent<SpawnManager>().Init();
        }

        _obstacleInstancesList = new List<GameObject>();

    }

    public void Start()
    {
        InstantiateCircle();
    }

    void Init()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        this.m_RadiusBorder = width * 0.80f / 2f;

    }

    public void Add1Point()
    {
        if (DOTween.IsTweening(Camera.main))
            return;

        _pointScore++;

        m_PointText.text = _pointScore.ToString();

        _soundManager.PlayTouch();

        FindObjectOfType<Circle>().DOParticle();
    }

    public void InstantiateCircle()
    {
        var radius = this.m_RadiusBorder;

        _player.DOStartPosition(radius, 0);

        var go = Instantiate(m_CirclePrefab) as GameObject;

        go.transform.position = new Vector3(0, 0, 98f);


        var circle = go.GetComponent<Circle>();

        circle.SetRadius(radius);
        circle.DOStart();
    }

    public void DOStart()
    {
        StartCoroutine(ObstacleInstantiator());
    }

    IEnumerator ObstacleInstantiator()
    {
        while (true)
        {
            var allObstacles = FindObjectsOfType<ObstacleEntity>();

            bool doInstantiateObstacle = false;

            if (allObstacles != null && allObstacles.Length > 1)
            {
                var allVisibleObtacles = Array.FindAll(allObstacles, o => o.m_IsVisible == true);

                if (allVisibleObtacles != null && allVisibleObtacles.Length < _pointScore + 3)
                {
                    doInstantiateObstacle = true;
                }
            }
            else
            {
                doInstantiateObstacle = true;
            }

            if (doInstantiateObstacle)
            {
                DOInstantiateObstacle();
            }

            yield return new WaitForSeconds(Util.GetRandomNumber(0.20f, 0.5f));
        }
    }

    void DOInstantiateObstacle()
    {
        GameObject objTmp = null;

        if (_obstacleInstancesList.Count < m_MaxObstaclesCount)
        {
            objTmp = Instantiate(m_ObstaclePrefab) as GameObject;
            _obstacleInstancesList.Add(objTmp);
        }
        else
        {
            objTmp = _obstacleInstancesList[0];
            _obstacleInstancesList.Remove(objTmp);
            _obstacleInstancesList.Add(objTmp);
        }

        ObstacleEntity obstacle = objTmp.GetComponent<ObstacleEntity>();

        obstacle.Init(_player.GetRotation() - 30, Util.GetRandomNumber(0f, 100f) < 50);
    }

    public void DODeleteObstacleInstance(GameObject obstacleInstance)
    {
        if (_obstacleInstancesList.Contains(obstacleInstance))
            _obstacleInstancesList.Remove(obstacleInstance);

        DestroyObject(obstacleInstance);

        obstacleInstance = null;

        //Debug.Log(obstacleInstance + " destroyed!");
        //Debug.Break();
    }

    void SetNewGame()
    {
        _pointScore = 0;

        m_PointText.text = _pointScore.ToString();

        _player.transform.eulerAngles = Vector3.zero;
    }

    public void OnPlayerImpactObstacle(GameObject obstacleEntityObj, Vector2 collisionPoint)
    {
        var obstacleEntity = obstacleEntityObj.GetComponent<ObstacleEntity>();

        switch (obstacleEntity.m_State)
        {
            case ObstacleEntity.State.NORMAL:
                GameOver();
                break;

            case ObstacleEntity.State.DESTRUCTIBLE:
                DOBreakObstacle(obstacleEntity.m_ObstacleObject, collisionPoint);
                break;

            default:
                break;
        }
    }

    public void DOBreakObstacle(GameObject obstacleSpriteObj, Vector2 collisionPoint)
    {
        var destructible = obstacleSpriteObj.GetComponent<D2dDestructible>();

        Debug.Log(obstacleSpriteObj.GetComponent<D2dDestructible>());

        //Debug.Break();

        // Store explosion point (used in OnEndSplit)
        if (collisionPoint == Vector2.zero)
            _expolsionPoint = obstacleSpriteObj.transform.position;
        else
            _expolsionPoint = collisionPoint;

        destructible.tag = "Untagged";
        destructible.GetComponentInChildren<D2dCollider>().m_SpriteChildCollider.tag = "Untagged";

        // Register split event
        destructible.OnEndSplit.AddListener(OnEndSplit);

        // Split via fracture
        D2dQuadFracturer.Fracture(destructible, _fractureCount, 0.5f);

        // Unregister split event
        destructible.OnEndSplit.RemoveListener(OnEndSplit);
    }

    private void OnEndSplit(List<D2dDestructible> clones)
    {
        // Go through all clones in the clones list
        for (var i = clones.Count - 1; i >= 0; i--)
        {
            var clone = clones[i];
            var rigidbody = clone.GetComponent<Rigidbody2D>();

            // Does this clone have a Rigidbody2D?
            if (rigidbody != null)
            {
                // Get the local point of the explosion that called this split event
                var localPoint = (Vector2)clone.transform.InverseTransformPoint(_expolsionPoint);

                // Get the vector between this point and the center of the destructible's current rect
                var vector = clone.AlphaRect.center - localPoint;

                // Apply relative force
                rigidbody.AddRelativeForce(vector * m_ExpolsionForce, ForceMode2D.Impulse);
            }
        }
    }

    public void GameOver()
    {
        if (m_GameStatus == GameStatus.GAMEOVER)
            return;

        //ReportScoreToLeaderboard(point);

        m_GameStatus = GameStatus.GAMEOVER;

        _player.DesactivateTouchControl();

        DOTween.KillAll();

        Util.SetLastScore(_pointScore);

        //ShowAds();

        StopAllCoroutines();

        DOTween.KillAll();

        _soundManager.PlayFail();

        FindObjectOfType<CanvasManager>().OnGameOver(() =>
        {

            DOTween.KillAll();

#if UNITY_5_3_OR_NEWER
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
#else
		    Application.LoadLevel(Application.loadedLevel);
#endif

        });
    }
}