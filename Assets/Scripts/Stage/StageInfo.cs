using UnityEngine;
using KKoding92.Board;

namespace KKoding92.Stage
{
    [System.Serializable]
    public class StageInfo
    {
        public int row;
        public int col;

        public int[] cells;

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }

        public CellType GetCellType(int nRow, int nCol)
        {
            Debug.Assert(cells != null && cells.Length > nRow * col + nCol, $"Invalid Row/Col = {nRow}, {nCol}");

            int revisedRow = (row - 1) - nRow;
            if (cells.Length > revisedRow * col + nCol)
                return (CellType)cells[revisedRow * col + nCol];

            Debug.Assert(false);

            return CellType.EMPTY;
        }

        public bool DoValidation()
        {
            Debug.Assert(cells.Length == row * col);
            Debug.Log($"cell length : {cells.Length}, row, col = ({row}, {col})");

            if (cells.Length != row * col)
                return false;

            return true;
        }
    }
}
