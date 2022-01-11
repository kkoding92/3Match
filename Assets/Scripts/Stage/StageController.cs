﻿using UnityEngine;
using KKoding92.Util;

namespace KKoding92.Stage
{
    public class StageController : MonoBehaviour
    {
        [SerializeField] Transform m_Container;
        [SerializeField] GameObject m_CellPrefab;
        [SerializeField] GameObject m_BlockPrefab;

        bool m_bInit;
        Stage m_Stage;
        InputManager m_InputManager;

        //Members for Event
        bool m_bTouchDown;          //입력상태 처리 플래그, 유효한 블럭을 클릭한 경우 true
        BlockPos m_BlockDownPos;    //블럭 위치 (보드에 저장된 위치)
        Vector3 m_ClickPos;         //DOWN 위치(보드 기준 Local 좌표)

        private void Start()
        {
            InitStage();
        }

        private void Update()
        {
            if (!m_bInit)
                return;

            OnInputHandler();
        }

        void OnInputHandler()
        {
            //Touch Down 
            if (!m_bTouchDown && m_InputManager.isTouchDown)
            {
                //1.1 보드 기준 Local 좌표를 구한다.
                Vector2 point = m_InputManager.touch2BoardPosition;

                //1.2 Play 영역(보드)에서 클릭하지 않는 경우는 무시
                if (!m_Stage.IsInsideBoard(point))
                    return;

                //1.3 클릭한 위치이 블럭을 구한다.
                BlockPos blockPos;
                if (m_Stage.IsOnValideBlock(point, out blockPos))
                {
                    //1.3.1 유효한(스와이프 가능한) 블럭에서 클릭한 경우
                    m_bTouchDown = true;        //클릭 상태 플래그 ON
                    m_BlockDownPos = blockPos;  //클릭한 블럭의 위치(row, col) 저장
                    m_ClickPos = point;         //클릭한 Local 좌표 저장
                                                //Debug.Log($"Mouse Down In Board : (blockPos})");
                }
            }
            //2. Touch UP : 유효한 블럭 위에서 Down 후에만 UP 이벤트 처리
            else if (m_InputManager.isTouchUp)
            {
                //2.1 보드 기준 Local 좌표를 구한다.
                Vector2 point = m_InputManager.touch2BoardPosition;

                //2.2 스와이프 방향을 구한다.
                Swipe swipeDir = m_InputManager.EvalSwipeDir(m_ClickPos, point);

                Debug.Log($"Swipe : {swipeDir} , Block = {m_BlockDownPos}");

                m_bTouchDown = false;   //클릭 상태 플래그 OFF
            }
        }

        private void InitStage()
        {
            if (m_bInit)
                return;

            m_bInit = true;
            m_InputManager = new InputManager(m_Container);

            BuildStage();
            //m_Stage.PrintAll();
        }

        void BuildStage()
        {
            //1. Stage 구성
            m_Stage = StageBuilder.BuildStage(nStage: 1);

            m_Stage.ComposeStage(m_CellPrefab, m_BlockPrefab, m_Container);
        }
    }
}
