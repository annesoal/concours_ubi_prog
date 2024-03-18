using Grid.Interface;
using Unity.Netcode;
using UnityEngine;

namespace Grid
{
    public class Ressource : MonoBehaviour, ITopOfCell
    {
        [SerializeField] private float TopOfCell = 0.72f;

        // Start is called before the first frame update
        void Start()
        {
            Vector3 position = transform.position;
            PlayerSelectorGridHelper helper = 
                new PlayerSelectorGridHelper(TilingGrid.LocalToGridPosition(position));
            helper.AddOnTopCell(gameObject, this);
            transform.position = TilingGrid.GridPositionToLocal(helper.Cell.position, TopOfCell );
        }

        public TypeTopOfCell GetType()
        {
            return TypeTopOfCell.Resource;
        }

        public GameObject ToGameObject()
        {
            return this.gameObject;
        }
    }
}
