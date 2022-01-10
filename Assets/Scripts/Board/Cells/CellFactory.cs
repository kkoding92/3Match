using UnityEngine;
using KKoding92.Stage;

namespace KKoding92.Board
{
    public static class CellFactory
    {
        public static Cell SpawnCell(StageInfo stageInfo, int nRow, int nCol)
        {
            Debug.Assert(stageInfo != null);
            Debug.Assert(nRow < stageInfo.row && nCol < stageInfo.col);

            return SpawnCell(stageInfo.GetCellType(nRow, nCol));
        }

        public static Cell SpawnCell(CellType cellType)
        {
            return new Cell(cellType);
        }
    }
}