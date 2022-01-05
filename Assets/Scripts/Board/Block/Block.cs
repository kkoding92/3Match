namespace KKoding92.Board
{
    public class Block
    {
        BlockType m_BlockType;

        public Block(BlockType blockType)
        {
            m_BlockType = blockType;
        }

        public BlockType type
        {
            get { return m_BlockType; }
            set { m_BlockType = value; }
        }
    }
}
