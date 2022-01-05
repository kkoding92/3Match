namespace KKoding92.Board
{
    public class Cell
    {
        protected CellType m_CellType;
        public Cell(CellType cellType)
        {
            m_CellType = cellType;
        }

        public CellType type
        {
            get { return m_CellType; }
            set { m_CellType = value; }
        }
    }
}
