using UnityEngine;
using System;
using System.Collections;

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
}
