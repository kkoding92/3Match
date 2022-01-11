using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KKoding92.Stage
{
    public class BoardEnumerator
    {
        KKoding92.Board.Board m_Board;

        public BoardEnumerator(KKoding92.Board.Board board)
        {
            this.m_Board = board;
        }

        public bool IsCageTypeCell(int nRow, int nCol)
        {
            return false;
        }
    }
}
