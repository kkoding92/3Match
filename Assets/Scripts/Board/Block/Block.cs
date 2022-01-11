using UnityEngine;
using KKoding92.Quest;
using KKoding92.Stage;

namespace KKoding92.Board
{
    public class Block
    {
        BlockType m_BlockType;

        protected BlockBehaviour m_BlockBehaviour;
        protected BlockBreed m_Breed;

        Vector2Int m_vtDuplicate;       //블럭 중복 개수, Shuffle시에 중복검사에 사용.

        public BlockStatus status;
        public BlockQuestType questType;
        public MatchType match = MatchType.NONE;
        public short matchCount;

        int m_nDurability;                          //내구도, 0이되면 제거
        public virtual int durability
        {
            get { return m_nDurability; }
            set { m_nDurability = value; }
        }

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

            status = BlockStatus.NORMAL;
            questType = BlockQuestType.CLEAR_SIMPLE;
            match = MatchType.NONE;
            m_Breed = BlockBreed.NA;

            m_nDurability = 1;
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

        public void MoveTo(Vector3 to, float duration)
        {
            m_BlockBehaviour.StartCoroutine(Util.Action2D.MoveTo(blockObj, to, duration));
        }

        public bool IsSwipeable(Block baseBlock)
        {
            return true;
        }

        /*
         * 블럭 평가하기
         * Board의 Evaluation으로 업데이트된 블럭 자신의 매칭 상태를 평가해서 현재 상태에 부여된 동작을 수행한다.
         * - BlockQuestType.CLEAR_SIMPLE 인 경우, 내구도(duration)을 1 감소시키킨다.
         * - 내구도가 '0'이되면 상태를 BlockStatus.CLEAR로 변경한다. (MATCH 상태에서 CLEAR 상태로 전이)
         * - 매치되지 않은 블럭은 상태 및 매칭 정보를 초기화한다.
         */
        public bool DoEvaluation(BoardEnumerator boardEnumerator, int nRow, int nCol)
        {
            Debug.Assert(boardEnumerator != null, $"({nRow},{nCol})");

            if (!IsEvaluatable())
                return false;

            //1. 매치 상태(클리어 조건 충족)인 경우
            if (status == BlockStatus.MATCH)
            {
                if (questType == BlockQuestType.CLEAR_SIMPLE || boardEnumerator.IsCageTypeCell(nRow, nCol)) //TODO cagetype cell 조건이 필요한가? 
                {
                    Debug.Assert(m_nDurability > 0, $"durability is zero : {m_nDurability}");

                    //보드에 블럭 클리어 이벤트를 전달한다.
                    //블럭 클리어 후에 보드에 미치는 영향을 반영한다.
                    //if (boardEnumerator.SendMessageToBoard(BlockStatus.CLEAR, nRow, nCol))
                    durability--;
                }
                else //특수블럭인 경우 true 리턴
                {
                    return true;
                }

                if (m_nDurability == 0)
                {
                    status = BlockStatus.CLEAR;
                    return false;
                }
            }

            //2. 클리어 조건에 아직 도달하지 않는 경우 NORMAL 상태로 복귀
            status = BlockStatus.NORMAL;
            match = MatchType.NONE;
            matchCount = 0;

            return false;
        }

        /*
         * 블럭의 매칭 상태를 업데이트한다. 이미 매치된 블럭인 경우에 기존 상태에 추가되는 상태가 더해진다.
         * 예를 들어, MatchType.THREE 상태에서 파라미터로 MatchType.FOUR 상태를 수신하면 MatchType.THREE_FOUR 상태가 된다.
         */
        public void UpdateBlockStatusMatched(MatchType matchType, bool bAccumulate = true)
        {
            this.status = BlockStatus.MATCH;

            if (match == MatchType.NONE)
            {
                this.match = matchType;
            }
            else
            {
                this.match = bAccumulate ? match.Add(matchType) : matchType; //match + matchType
            }

            matchCount = (short)matchType;
        }

        public bool IsEvaluatable()
        {
            //이미 처리완료(CLEAR) 되었거나, 현재 처리중인 블럭인 경우
            if (status == BlockStatus.CLEAR || !IsMatchableBlock())
                return false;

            return true;
        }

        public virtual void Destroy()
        {
            Debug.Assert(blockObj != null, $"{match}");
            blockBehaviour.DoActionClear();
        }
    }
}
