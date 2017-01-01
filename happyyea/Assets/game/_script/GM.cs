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
public class GM : MonoBehaviour
{
    public static event Action OnNewGameLoad = delegate { };
    public static event Action OnGameStart = delegate { };

    public enum GameStatus
    {
        STARTED,
        GAMEOVER
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

    public int m_CurrentPointScore
    {
        get
        {
            return _pointScore;
        }
    }

    public ObstacleManager      m_ObstacleManager;
    public LevelManager         m_LevelManager;
    public Gradient[]           m_ThemeGradients;
    public Color                m_BackgroundColor = Color.white;
    public Color                m_CircleColor = Color.black;
    public Color                m_PlayerColor = Color.white;
    public Color                m_ThemeDynamicColor
    {
        get
        {
            return _dynamicColor;
        }
    }
    public float                m_DynamicColorCycleDuration;
    public bool                 m_ActivateCameraShake = false;
    public Text                 m_PointText;
    public GameObject           m_CirclePrefab;
    public GameObject           m_Particle;
    public float                m_PlayerExpolsionForce;
    public int                  m_PlayerFractureCount;

    private float               _radiusBorder;
    private int                 _pointScore = 0;
    private Player              _player;
    private SoundManager        _soundManager;
    private GameStatus          _currentGameStatus;
    private CanvasScaler        _canvasScaler;
    private Vector3             _expolsionPoint;
    private Color               _dynamicColor;

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
        //Application.targetFrameRate = 60;
        
        Init();
        
        if (FindObjectOfType<SpawnManager>() == null)
        {
            GameObject go = new GameObject();

            go.name = "_SpawnManager";

            go.AddComponent(typeof(SpawnManager));

            go.GetComponent<SpawnManager>().particle = this.m_Particle;
            go.GetComponent<SpawnManager>().Init();
        }
    }

    void Update()
    {
        float t = Mathf.PingPong( Time.time / m_DynamicColorCycleDuration, 1f );
        _dynamicColor = m_ThemeGradients[0].Evaluate( t );
    }

    public void Start()
    {
        InstantiateCircle();
    }

    private void Init()
    {
        Camera cam = Camera.main;
        float height = 2.3f * cam.orthographicSize;
        float width = height * cam.aspect;

        this._radiusBorder = width * 0.80f / 1.5f;

    }

    public void Add1Point()
    {
        if (DOTween.IsTweening(Camera.main))
            return;

        _pointScore++;

        m_PointText.text = _pointScore.ToString();

        _soundManager.PlayTouch();

        //FindObjectOfType<Circle>().DOParticle();
    }

    public void InstantiateCircle()
    {
        var radius = this._radiusBorder;

        _player.DOStartPosition(radius, 0);

        var go = Instantiate(m_CirclePrefab) as GameObject;

        go.transform.position = new Vector3(0, 0, 5f);
        
        var circle = go.GetComponent<RoadCircle>();

        circle.SetRadius(radius);
        circle.DOStart();
    }

    public void DOStartGame()
    {
        OnGameStart();
    }

    void SetNewGame()
    {
        _pointScore = 0;

        m_PointText.text = _pointScore.ToString();

        _player.transform.eulerAngles = Vector3.zero;

        OnNewGameLoad();

        Debug.Log("GM set new Game");
    }

    public void OnImpactObstacleByPlayer(GameObject obstacleEntityObj, Vector2 collisionPoint)
    {
        var obstacleEntity = obstacleEntityObj.GetComponent<ObstacleEntity>();
        var obstacleRenderObject = obstacleEntity.m_ObstacleObject;
        var obstacleDestructible = obstacleRenderObject.GetComponent<D2dDestructible>();

        switch (obstacleEntity.m_State)
        {
            case ObstacleEntity.State.NORMAL:
                    obstacleRenderObject.GetComponent<Rigidbody2D>().isKinematic = true;
                    GameOver( collisionPoint );
                break;

            case ObstacleEntity.State.DESTRUCTIBLE:
                    Add1Point();
                    DOBreakObject( obstacleDestructible, collisionPoint, m_ObstacleManager.m_ObstacleFractureCount);
                break;

            default:
                break;
        }
    }

    public void DOBreakObject( D2dDestructible destructible, Vector2 collisionPoint, int fractureCount)
    {
        
        // Store explosion point (used in OnEndSplit)
        if (collisionPoint == Vector2.zero)
            _expolsionPoint = destructible.transform.position;
        else
            _expolsionPoint = collisionPoint;

        destructible.transform.tag = "Untagged";

        if( destructible.GetComponentInChildren<D2dCollider>() )
            destructible.GetComponentInChildren<D2dCollider>().m_SpriteChildCollider.tag = "Untagged";

        // Register split event
        destructible.OnEndSplit.AddListener(OnEndSplit);

        // Split via fracture
        D2dQuadFracturer.Fracture(destructible, fractureCount, 0.5f);

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

                var force = ( m_GameStatus == GameStatus.GAMEOVER ? m_PlayerExpolsionForce : m_ObstacleManager.m_ObstacleExpolsionForce );

                // Apply relative force
                rigidbody.AddRelativeForce(vector * force, ForceMode2D.Impulse);
            }
        }
    }

    public void GameOver( Vector2 collisionPoint )
    {
        if (m_GameStatus == GameStatus.GAMEOVER)
            return;

        //ReportScoreToLeaderboard(point);

        m_GameStatus = GameStatus.GAMEOVER;

        _player.DesactivateTouchControl();

        DOTween.KillAll();

        Utils.SetLastScore(_pointScore);

        //ShowAds();

        StopAllCoroutines();

        DOTween.KillAll();

        _soundManager.PlayFail();

        DOBreakObject(_player.m_PlayerTransform.GetComponent<D2dDestructible>(), collisionPoint, m_PlayerFractureCount);

        FindObjectOfType<CanvasController>().OnGameOver(() =>
        {
            DOReloadScene();
        });
    }

    private void DOReloadScene()
    {

#if UNITY_5_3_OR_NEWER
        SceneManager.LoadSceneAsync( 0, LoadSceneMode.Single );
#else
		    Application.LoadLevel(Application.loadedLevel);
#endif
    }
}