using UnityEngine;
using KKoding92.Scriptable;
using System.Collections;

namespace KKoding92.Board
{
    public class BlockBehaviour : MonoBehaviour
    {
        [SerializeField] BlockConfig m_BlockConfig;

        Block m_Block;
        SpriteRenderer m_SpriteRenderer;

        void Start()
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();

            UpdateView(false);
        }

        internal void SetBlock(Block block)
        {
            m_Block = block;
        }

        public void UpdateView(bool bValueChanged)
        {
            if(m_Block.type == BlockType.EMPTY)
            {
                m_SpriteRenderer.sprite = null;
            }
            else if(m_Block.type == BlockType.BASIC)
            {
                m_SpriteRenderer.sprite = m_BlockConfig.basicBlockSprites[(int)m_Block.breed];
            }
        }

        public void DoActionClear()
        {
            StartCoroutine(CoStartSimpleExplosion(true));
        }

        /*
         * 블럭이 폭발한 후, GameObject를 삭제한다.
         */
        IEnumerator CoStartSimpleExplosion(bool bDestroy = true)
        {
            //1. 폭파시키는 효과 연출 : 블럭 자체의 Clear 효과를 연출한다 (모든 블럭 동일)
            GameObject explosionObj = m_BlockConfig.GetExplosionObject(BlockQuestType.CLEAR_SIMPLE);
            ParticleSystem.MainModule newModule = explosionObj.GetComponent<ParticleSystem>().main;
            newModule.startColor = m_BlockConfig.GetBlockColor(m_Block.breed);

            explosionObj.SetActive(true);
            explosionObj.transform.position = this.transform.position;

            yield return new WaitForSeconds(0.1f);

            //2. 블럭 GameObject 객체 삭제 or make size zero
            if (bDestroy)
                Destroy(gameObject);
            else
            {
                Debug.Assert(false, "Unknown Action : GameObject No Destory After Particle");
            }
        }
    }
}
