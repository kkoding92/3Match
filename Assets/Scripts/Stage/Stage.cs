using UnityEngine;
using KKoding92.Board;
using KKoding92.Util;
using KKoding92.Core;
using System.Collections;

namespace KKoding92.Stage
{
    public class Stage
    {
        public int maxRow { get { return m_Board.maxRow; } }
        public int maxCol { get { return m_Board.maxCol; } }

        KKoding92.Board.Board m_Board;
        public KKoding92.Board.Board board { get { return m_Board; } }

        StageBuilder m_StageBuilder;
        public Block[,] blocks { get { return m_Board.blocks; } }
        public Cell[,] cells { get { return m_Board.cells; } }

        public Stage(StageBuilder stageBuilder, int nRow, int nCol)
        {
            m_StageBuilder = stageBuilder;

            m_Board = new KKoding92.Board.Board(nRow, nCol);
        }

        internal void ComposeStage(GameObject cellPrefab, GameObject blockPrefab, Transform container)
        {
            m_Board.ComposeStage(cellPrefab, blockPrefab, container);
        }

        public bool IsOnValideBlock(Vector2 point, out BlockPos blockPos)
        {
            //1. Local 좌표 -> 보드의 블럭 인덱스로 변환한다.
            Vector2 pos = new Vector2(point.x + (maxCol / 2.0f), point.y + (maxRow / 2.0f));
            int nRow = (int)pos.y;
            int nCol = (int)pos.x;

            //리턴할 블럭 인덱스 생성
            blockPos = new BlockPos(nRow, nCol);

            //2. 스와이프 가능한지 체크한다.
            return board.IsSwipeable(nRow, nCol);
        }

        public bool IsInsideBoard(Vector2 ptOrg)
        {
            // 계산의 편의를 위해서 (0, 0)을 기준으로 좌표를 이동한다. 
            // 8 x 8 보드인 경우: x(-4 ~ +4), y(-4 ~ +4) -> x(0 ~ +8), y(0 ~ +8) 
            Vector2 point = new Vector2(ptOrg.x + (maxCol / 2.0f), ptOrg.y + (maxRow / 2.0f));

            if (point.y < 0 || point.x < 0 || point.y > maxRow || point.x > maxCol)
                return false;

            return true;
        }

        public IEnumerator CoDoSwipeAction(int nRow, int nCol, Swipe swipeDir, Returnable<bool> actionResult)
        {
            actionResult.value = false; //코루틴 리턴값 RESET

            //1. 스와이프되는 상대 블럭 위치를 구한다. (using SwipeDir Extension Method)
            int nSwipeRow = nRow, nSwipeCol = nCol;
            nSwipeRow += swipeDir.GetTargetRow(); //Right : +1, LEFT : -1
            nSwipeCol += swipeDir.GetTargetCol(); //UP : +1, DOWN : -1

            Debug.Assert(nRow != nSwipeRow || nCol != nSwipeCol, "Invalid Swipe : ({nSwipeRow}, {nSwipeCol})");
            Debug.Assert(nSwipeRow >= 0 && nSwipeRow < maxRow && nSwipeCol >= 0 && nSwipeCol < maxCol, $"Swipe 타겟 블럭 인덱스 오류 = ({nSwipeRow}, {nSwipeCol}) ");

            //2. 스와이프 가능한 블럭인지 체크한다. (인덱스 Validation은 호출 전에 검증됨)
            if (m_Board.IsSwipeable(nSwipeRow, nSwipeCol))
            {
                //2.1 스와이프 대상 블럭(소스, 타겟)과 각 블럭의 이동전 위치를 저장한다.
                Block targetBlock = blocks[nSwipeRow, nSwipeCol];
                Block baseBlock = blocks[nRow, nCol];
                Debug.Assert(baseBlock != null && targetBlock != null);

                Vector3 basePos = baseBlock.blockObj.transform.position;
                Vector3 targetPos = targetBlock.blockObj.transform.position;

                //2.2 스와이프 액션을 실행한다.
                if (targetBlock.IsSwipeable(baseBlock))
                {
                    //2.2.1 상대방의 블럭 위치로 이동하는 애니메이션을 수행한다
                    baseBlock.MoveTo(targetPos, Constants.SWIPE_DURATION);
                    targetBlock.MoveTo(basePos, Constants.SWIPE_DURATION);

                    yield return new WaitForSeconds(Constants.SWIPE_DURATION);

                    //2.2.2 Board에 저장된 블럭의 위치를 교환한다
                    blocks[nRow, nCol] = targetBlock;
                    blocks[nSwipeRow, nSwipeCol] = baseBlock;

                    actionResult.value = true;
                }
            }

            yield break;
        }

        public bool IsValideSwipe(int nRow, int nCol, Swipe swipeDir)
        {
            switch (swipeDir)
            {
                case Swipe.DOWN: return nRow > 0; ;
                case Swipe.UP: return nRow < maxRow - 1;
                case Swipe.LEFT: return nCol > 0;
                case Swipe.RIGHT: return nCol < maxCol - 1;
                default:
                    return false;
            }
        }

        public void PrintAll()
        {
            System.Text.StringBuilder strCells = new System.Text.StringBuilder();
            System.Text.StringBuilder strBlocks = new System.Text.StringBuilder();

            for (int nRow = maxRow - 1; nRow >= 0; nRow--)
            {
                for (int nCol = 0; nCol < maxCol; nCol++)
                {
                    strCells.Append($"{cells[nRow, nCol].type}, ");
                    strBlocks.Append($"{blocks[nRow, nCol].type}, ");
                }

                strCells.Append("\n");
                strBlocks.Append("\n");
            }

            Debug.Log(strCells.ToString());
            Debug.Log(strBlocks.ToString());
        }
    }
}
