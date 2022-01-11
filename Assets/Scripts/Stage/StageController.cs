using UnityEngine;
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
            if (m_InputManager.isTouchDown)
            {
                Vector2 point = m_InputManager.touchPosition;
 
                Debug.Log($"Input Down = {point}, local = {m_InputManager.touch2BoardPosition}");
            }
            //Touch UP : 유효한 블럭 위에서 Down 후에 발생하는 경우
            else if (m_InputManager.isTouchUp)
            {
                Vector2 point = m_InputManager.touchPosition;
                Debug.Log($"Input Up = {point}, local = {m_InputManager.touch2BoardPosition}");
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
