using UnityEngine;

namespace KKoding92.Board
{
    public class Cell
    {
        protected CellType m_CellType;
        protected CellBehaviour m_CellBehaviour;

        public Cell(CellType cellType)
        {
            m_CellType = cellType;
        }

        public CellBehaviour cellBehaviour
        {
            get { return m_CellBehaviour; }
            set
            {
                m_CellBehaviour = value;
                m_CellBehaviour.SetCell(this);
            }
        }

        public CellType type
        {
            get { return m_CellType; }
            set { m_CellType = value; }
        }
        public Cell InstantiateCellObj(GameObject cellPrefab, Transform containerObj)
        {
            //1. Cell 오브젝트를 생성한다.
            GameObject newObj = Object.Instantiate(cellPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            //2. 컨테이너(Board)의 차일드로 Cell을 포함시킨다.
            newObj.transform.parent = containerObj;

            //3. Cell 오브젝트에 적용된 CellBehaviour 컴포너트를 보관한다.
            this.cellBehaviour = newObj.transform.GetComponent<CellBehaviour>();

            return this;
        }

        public void Move(float x, float y)
        {
            cellBehaviour.transform.position = new Vector3(x, y);
        }
    }
}
