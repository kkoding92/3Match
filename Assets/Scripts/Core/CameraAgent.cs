﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KKoding92.Core
{
    public class CameraAgent : MonoBehaviour
    {
        [SerializeField] Camera m_TargetCamera;
        [SerializeField] float m_BoardUnit;

        void Start()
        {
            m_TargetCamera.orthographicSize = m_BoardUnit / m_TargetCamera.aspect;
        }
    }
}
