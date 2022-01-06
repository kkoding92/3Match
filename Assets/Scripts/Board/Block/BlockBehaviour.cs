using UnityEngine;

namespace KKoding92.Board
{
    public class BlockBehaviour : MonoBehaviour
    {
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
        }
    }
}
