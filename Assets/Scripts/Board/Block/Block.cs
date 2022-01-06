using UnityEngine;

namespace KKoding92.Board
{
    public class Block
    {
        BlockType m_BlockType;

        protected BlockBehaviour m_BlockBehaviour;

        public Block(BlockType blockType)
        {
            m_BlockType = blockType;
        }

        public BlockBehaviour blockBehaviour
        {
            get { return m_BlockBehaviour; }
            set
            {
                m_BlockBehaviour = value;
                m_BlockBehaviour.SetBlock(this);
            }
        }

        public BlockType type
        {
            get { return m_BlockType; }
            set { m_BlockType = value; }
        }

        internal Block InstantiateBlockObj(GameObject blockPrefab, Transform containerObj)
        {
            //1. Block 오브젝트를 생성한다.
            GameObject newObj = Object.Instantiate(blockPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            //2. 컨테이너(Board)의 차일드로 Block을 포함시킨다.
            newObj.transform.parent = containerObj;

            //3. Block 오브젝트에 적용된 BlockBehaviour 컴포너트를 보관한다.
            this.blockBehaviour = newObj.transform.GetComponent<BlockBehaviour>();

            return this;
        }

        internal void Move(float x, float y)
        {
            blockBehaviour.transform.position = new Vector3(x, y);
        }
    }
}
