using UnityEngine;

namespace KKoding92.Stage
{
    public class StageController : MonoBehaviour
    {
        bool m_bInit;
        Stage m_Stage;

        void Start()
        {
            InitStage();
        }

        private void InitStage()
        {
            if (m_bInit)
                return;

            m_bInit = true;

            BuildStage();
            m_Stage.PrintAll();
        }

        void BuildStage()
        {
            //1. Stage 구성
            m_Stage = StageBuilder.BuildStage(nStage: 0, row: 9, col: 9);
        }
    }
}
