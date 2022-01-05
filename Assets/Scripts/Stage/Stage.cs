public class Stage
{
    int m_nRow;
    int m_nCol;

    public int maxRow { get { return m_nRow; } }
    public int maxCol { get { return m_nCol; } }

    Board m_Board;
    public Board board { get { return m_Board; } }
}
