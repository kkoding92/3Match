using KKoding92.Board;
using UnityEngine;

namespace KKoding92.Scriptable
{
    [CreateAssetMenu(menuName = "3Match/Block Config", fileName = "BlockConfig.asset")]
    public class BlockConfig : ScriptableObject
    {
        public float[] dropSpeed;
        public Sprite[] basicBlockSprites;
        public GameObject explosion;
        public Color[] blockColors;

        public GameObject GetExplosionObject(BlockQuestType questType)
        {
            switch (questType)
            {
                case BlockQuestType.CLEAR_SIMPLE:
                    return Instantiate(explosion) as GameObject;
                default:
                    return Instantiate(explosion) as GameObject;
            }
        }

        public Color GetBlockColor(BlockBreed breed)
        {
            return blockColors[(int)breed];
        }
    }
}
