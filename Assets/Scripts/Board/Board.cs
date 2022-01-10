using UnityEngine;
using KKoding92.Stage;

namespace KKoding92.Board
{
    public class Board
    {
        int m_nRow;
        int m_nCol;

        Transform m_Container;
        GameObject m_CellPrefab;
        GameObject m_BlockPrefab;

        public int maxRow { get { return m_nRow; } }
        public int maxCol { get { return m_nCol; } }

        Cell[,] m_Cells;
        public Cell[,] cells { get { return m_Cells; } }

        Block[,] m_Blocks;
        public Block[,] blocks { get { return m_Blocks; } }

        public Board(int nRow, int nCol)
        {
            m_nRow = nRow;
            m_nCol = nCol;

            m_Cells = new Cell[nRow, nCol];
            m_Blocks = new Block[nRow, nCol];
        }
        internal void ComposeStage(GameObject cellPrefab, GameObject blockPrefab, Transform container)
        {
            //1. 스테이지 구성에 필요한 Cell,Block, Container(Board) 정보를 저장한다.
            m_CellPrefab = cellPrefab;
            m_BlockPrefab = blockPrefab;
            m_Container = container;

            //2. 3매치된 블럭이 없도록 섞는다.  
            BoardShuffler shuffler = new BoardShuffler(this, true);
            shuffler.Shuffle();

            //3. Cell, Block Prefab을 이용해서 Board에 Cell/Block GameObject를 추가한다.
            float initX = CalcInitX(0.5f);
            float initY = CalcInitY(0.5f);
            for (int nRow = 0; nRow < m_nRow; nRow++)
            { 
                for (int nCol = 0; nCol < m_nCol; nCol++)
                {
                    Cell cell = m_Cells[nRow, nCol]?.InstantiateCellObj(cellPrefab, container);
                    cell?.Move(initX + nCol, initY + nRow);

                    Block block = m_Blocks[nRow, nCol]?.InstantiateBlockObj(blockPrefab, container);
                    block?.Move(initX + nCol, initY + nRow);
                }
            }
        }

        public float CalcInitX(float offset = 0)
        {
            return -m_nCol / 2.0f + offset;
        }

        public float CalcInitY(float offset = 0)
        {
            return -m_nRow / 2.0f + offset;
        }

        public bool CanShuffle(int nRow, int nCol, bool bLoading)
        {
            if (!m_Cells[nRow, nCol].type.IsBlockMovableType())
                return false;

            return true;
        }

        public void ChangeBlock(Block block, BlockBreed notAllowedBreed)
        {
            BlockBreed genBreed;

            while (true)
            {
                genBreed = (BlockBreed)UnityEngine.Random.Range(0, 6);

                if (notAllowedBreed == genBreed)
                    continue;

                break;
            }

            block.breed = genBreed;
        }
    }
}
