using System.Collections;
using UnityEngine;
using KKoding92.Util;


namespace KKoding92.Stage
{
    public class ActionManager
    {
        Transform m_Container;          //컨테이저 (Board GameObject)
        Stage m_Stage;
        MonoBehaviour m_MonoBehaviour;  //코루틴 호출시 필요한 MonoBehaviour
        bool m_bRunning;                //액션 실행 상태 : 실행중인 경우 true

        public ActionManager(Transform container, Stage stage)
        {
            m_Container = container;
            m_Stage = stage;

            m_MonoBehaviour = container.gameObject.GetComponent<MonoBehaviour>();
        }

        /*
         * 코루틴 Wapper 메소드   
         */
        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return m_MonoBehaviour.StartCoroutine(routine);
        }

        /*
         * 스와이프를 액션을 시작한다.
         * @param nRow, nCol 블럭 위치
         * @swipeDir 스와이프 방향
         */
        public void DoSwipeAction(int nRow, int nCol, Swipe swipeDir)
        {
            Debug.Assert(nRow >= 0 && nRow < m_Stage.maxRow && nCol >= 0 && nCol < m_Stage.maxCol);

            if (m_Stage.IsValideSwipe(nRow, nCol, swipeDir))
            {
                StartCoroutine(CoDoSwipeAction(nRow, nCol, swipeDir));
            }
        }

        /*
        * 스와이프 액션을 수행하는 코루틴
        */
        IEnumerator CoDoSwipeAction(int nRow, int nCol, Swipe swipeDir)
        {
            if (!m_bRunning)  //다른 액션이 수행 중이면 PASS
            {
                m_bRunning = true;    //액션 실행 상태 ON

                //1. swipe action 수행
                /*
                 * 코루틴 실행 결과를 전달받을 Returnable 객체를 생성한다.
                 * 코루틴은 IEnumerator를 리턴할 뿐 코루틴 수행 결과값을 리턴해주지 않는다. 그래서 Returanable 객체를 인자로 전달한다.
                 */
                Returnable<bool> bSwipedBlock = new Returnable<bool>(false);
                yield return m_Stage.CoDoSwipeAction(nRow, nCol, swipeDir, bSwipedBlock);

                m_bRunning = false;  //액션 실행 상태 OFF
            }
            yield break;
        }
    }
}
