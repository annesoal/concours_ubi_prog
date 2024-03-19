using Grid.Interface;
using Unity.Netcode;
using UnityEngine;

namespace Grid
{
    public class Ressource : NetworkBehaviour, ITopOfCell
    {
        [SerializeField] private float TopOfCell = 0.72f;

        public new TypeTopOfCell GetType()
        {
            return TypeTopOfCell.Resource;
        }

        public GameObject ToGameObject()
        {
            return this.gameObject;
        }
    }
}
