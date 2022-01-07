using KKoding92.Board;

namespace KKoding92.Stage
{
    public class StageBuilder
    {
        int m_nStage;

        public StageBuilder(int nStage)
        {
            m_nStage = nStage;
        }

        public Stage ComposeStage(int row, int col)
        {
            // 1. Stage 객체를 생성
            Stage stage = new Stage(this, row, col);

            for (int nRow = 0; nRow < row; nRow++)
            {
                for (int nCol = 0; nCol < col; nCol++)
                {
                    stage.blocks[nRow, nCol] = SpawnBlockForStage(nRow, nCol);
                    stage.cells[nRow, nCol] = SpawnCellForStage(nRow, nCol);
                }
            }

            return stage;
        }

        Block SpawnBlockForStage(int nRow, int nCol)
        {
            //return new Block(BlockType.BASIC);
            return nRow == nCol ? SpawnEmptyBlock() : SpawnBlock();
        }

        Cell SpawnCellForStage(int nRow, int nCol)
        {
            return new Cell(nRow == nCol ? CellType.EMPTY : CellType.BASIC);
            //return new Cell(CellType.BASIC);
        }

        public static Stage BuildStage(int nStage, int row, int col)
        {
            StageBuilder stageBuilder = new StageBuilder(0);
            Stage stage = stageBuilder.ComposeStage(row, col);

            return stage;
        }

        public Block SpawnBlock()
        {
            return BlockFactory.SpawnBlock(BlockType.BASIC);
        }

        public Block SpawnEmptyBlock()
        {
            Block newBlock = BlockFactory.SpawnBlock(BlockType.EMPTY);

            return newBlock;
        }
    }
}
