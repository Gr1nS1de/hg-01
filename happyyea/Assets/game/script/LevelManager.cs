using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Serializable]
    public struct LevelTemplates
    {
        public int                  m_Id;
        public Gradient[]           m_BackgroundGradients;
        public Color                m_RoadColor;
        public Sprite               m_Image;
        public float                m_BackgroundGradientDuration;
    }
    
    public LevelTemplates[] m_LevelTemplates = new LevelTemplates[2];

    private void Awake()
    {
        
    }

    private void Start()
    {

    }
}
