using UnityEngine;

namespace KKoding92.Scriptable
{
    [CreateAssetMenu(menuName = "3Match/Block Config", fileName = "BlockConfig.asset")]
    public class BlockConfig : ScriptableObject
    {
        public Sprite[] basicBlockSprites;
    }
}
