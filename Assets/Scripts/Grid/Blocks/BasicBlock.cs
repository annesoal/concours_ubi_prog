using Unity.Collections;
using UnityEngine;

namespace Grid.Blocks
{
    public enum BlockType
    {
        None = 0,
        Walkable,
        Buildable,
        Movable,
        Destructible,
        SpawnBlock1,
        SpawnBlock2
    }
    public class BasicBlock : MonoBehaviour, IBlock
    {
        [SerializeField] protected BlockType blockType = BlockType.None;
       
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Awake est bien dans editor ou mettre en edit mode
        // Peut creer un lag 
        void Awake()
        {
        
        }


        public Vector3 GetPosition()
        {
            return this.transform.localPosition;
        }

        public BlockType GetBlockType()
        {
            return blockType;
        }
    }
}
