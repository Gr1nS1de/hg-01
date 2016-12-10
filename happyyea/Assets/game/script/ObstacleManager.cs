using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ObstacleManager : MonoBehaviour
{
    [Serializable]
    public struct ObstacleTemplate
    {
        public ObstacleEntity.State state;
        public Sprite[] sprite;
        public String spriteResourcesPath;
    }

    public ObstacleTemplate[]   m_ObstacleTemplate = new ObstacleTemplate[(int)ObstacleEntity.State._COUNTFLAG];
    public int                  m_MaxObstaclesCount;
    public GameObject           m_ObstaclePrefab;
    public float                m_ObstacleExpolsionForce = 0.001f;
    public int                  m_ObstacleFractureCount;

    private Player              _player;
    private GM                  _gameManager;

    private Dictionary<ObstacleEntity.State, GameObject[]> 
        _obstacleTemplatesInstaceDictionary = new Dictionary<ObstacleEntity.State, GameObject[]>();

    private void Awake()
    {
        GM.OnNewGameLoad += OnNewGameLoad;
        GM.OnGameStart += OnGameStart;

        _player = FindObjectOfType<Player>();
        _gameManager = FindObjectOfType<GM>();
    }

    private void OnNewGameLoad()
    {
        Debug.Log("New game load ObstacleManager");
        AddSpritesFromResource();
        PreloadObstacleTemplates();
    }

    private void OnGameStart()
    {
        StartCoroutine( ObstacleInstantiator() );
    }

    IEnumerator ObstacleInstantiator()
    {
        while ( true )
        {
            var allObstacles = FindObjectsOfType<ObstacleEntity>();
            bool doInstantiateObstacle = false;

            if ( allObstacles != null && allObstacles.Length > 1 )
            {
                var allVisibleObtacles = Array.FindAll( allObstacles, o => o.m_IsVisible == true );

                if ( allVisibleObtacles != null && allVisibleObtacles.Length < _gameManager.m_CurrentPointScore + 3 )
                    doInstantiateObstacle = true;
            }
            else
                doInstantiateObstacle = true;

            if ( doInstantiateObstacle )
                DOInstantiateObstacle();

            yield return new WaitForSeconds( Util.GetRandomNumber( 0.20f, 0.5f ) );
        }
    }

    public void DODeleteObstacleInstance( GameObject obstacleInstance )
    {

        DestroyObject( obstacleInstance );

        obstacleInstance = null;
    }

    private void DOInstantiateObstacle()
    {
        GameObject objTmp = null;

        var randomObstacleState = (ObstacleEntity.State)UnityEngine.Random.Range( 0, (int)ObstacleEntity.State._COUNTFLAG );
        var stateInstancesCount = _obstacleTemplatesInstaceDictionary[m_ObstacleTemplate[(int)randomObstacleState].state].Length;
        var randomInstanceIndex = UnityEngine.Random.Range( 0, stateInstancesCount );

        Debug.Log(randomInstanceIndex + " " + m_ObstacleTemplate[0].sprite.Length);

        var obstacleRandomTemplate = _obstacleTemplatesInstaceDictionary[m_ObstacleTemplate[(int)randomObstacleState].state][randomInstanceIndex];

        objTmp = Instantiate( obstacleRandomTemplate ) as GameObject;
        objTmp.SetActive( true );

        ObstacleEntity obstacleEntity = objTmp.GetComponent<ObstacleEntity>();

        obstacleEntity.m_State = obstacleRandomTemplate.GetComponent<ObstacleEntity>().m_State;

        obstacleEntity.Init( _player.GetRotation() - 30, Util.GetRandomNumber( 0f, 100f ) < 50 );
    }

    private void AddSpritesFromResource()
    {
        for ( int i = 0; i < (int)ObstacleEntity.State._COUNTFLAG; i++ )
        {
            var obstacleSprites = Resources.LoadAll<Sprite>( m_ObstacleTemplate[i].spriteResourcesPath );

            m_ObstacleTemplate[i].sprite = new Sprite[obstacleSprites.Length];

            for ( int j = 0; j < obstacleSprites.Length; j++ )
            {
                m_ObstacleTemplate[i].sprite[j] = obstacleSprites[j];
            }
        }
    }

    public void PreloadObstacleTemplates()
    {
        for ( int i = 0; i < m_ObstacleTemplate.Length; i++ )
        {

            switch ( m_ObstacleTemplate[i].state )
            {
                case ObstacleEntity.State.NORMAL:
                    {
                        List<GameObject> obstacleInstances = new List<GameObject>();

                        foreach ( Sprite sprite in m_ObstacleTemplate[i].sprite )
                        {
                            var objTmp = Instantiate( m_ObstaclePrefab ) as GameObject;
                            var obstacleEntity = objTmp.GetComponent<ObstacleEntity>();

                            obstacleEntity.m_ObstacleSprite = sprite;
                            obstacleEntity.SetNormalTemplate();

                            objTmp.SetActive( false );

                            obstacleInstances.Add( objTmp );
                        }

                        _obstacleTemplatesInstaceDictionary.Add( m_ObstacleTemplate[i].state, obstacleInstances.ToArray() );
                    }
                    break;

                case ObstacleEntity.State.DESTRUCTIBLE:
                    {
                        List<GameObject> obstacleInstances = new List<GameObject>();

                        foreach ( Sprite sprite in m_ObstacleTemplate[i].sprite )
                        {
                            var objTmp = Instantiate( m_ObstaclePrefab ) as GameObject;
                            var obstacleEntity = objTmp.GetComponent<ObstacleEntity>();

                            obstacleEntity.m_ObstacleSprite = sprite;
                            obstacleEntity.SetDestructibleTemplate();

                            objTmp.SetActive( false );

                            obstacleInstances.Add( objTmp );
                        }

                        _obstacleTemplatesInstaceDictionary.Add( m_ObstacleTemplate[i].state, obstacleInstances.ToArray() );
                    }
                    break;
            }
        }
    }
}
