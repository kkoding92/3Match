using UnityEngine;

namespace KKoding92.Board
{
    public class Block
    {
        BlockType m_BlockType;

        protected BlockBehaviour m_BlockBehaviour;
        protected BlockBreed m_Breed;

        Vector2Int m_vtDuplicate;       //블럭 중복 개수, Shuffle시에 중복검사에 사용.

        
        public int horzDuplicate      //가로방향 중복 검사시 사용  
        {
            get { return m_vtDuplicate.x; }
            set { m_vtDuplicate.x = value; }
        }

        public int vertDuplicate       //세로방향 중복 검사시 사용
        {
            get { return m_vtDuplicate.y; }
            set { m_vtDuplicate.y = value; }
        }

        public void ResetDuplicationInfo()
        {
            m_vtDuplicate.x = 0;
            m_vtDuplicate.y = 0;
        }

        public bool IsEqual(Block target)
        {
            if (IsMatchableBlock() && this.breed == target.breed)
                return true;

            return false;
        }

        public bool IsMatchableBlock()
        {
            return !(type == BlockType.EMPTY);
        }

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

        public BlockBreed breed
        {
            get { return m_Breed; }
            set
            {
                m_Breed = value;
                m_BlockBehaviour?.UpdateView(true);
            }
        }

        public BlockType type
        {
            get { return m_BlockType; }
            set { m_BlockType = value; }
        }

        public Transform blockObj { get { return m_BlockBehaviour?.transform; } }

        internal Block InstantiateBlockObj(GameObject blockPrefab, Transform containerObj)
        {
            //유효하지 않은 블럭인 경우 Block Obj를 생성하지 않음.
            if (IsValidate() == false)
                return null;

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

        public bool IsValidate()
        {
            return type != BlockType.EMPTY;
        }
    }
}
