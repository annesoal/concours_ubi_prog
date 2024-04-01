using Grid.Interface;
using Unity.Netcode;
using UnityEngine;

namespace Synchrone
{
    public class Bonus : NetworkBehaviour, ITopOfCell
    {
        public TypeTopOfCell GetType()
        {
            return TypeTopOfCell.Bonus;
        }

        public GameObject ToGameObject()
        {
            return this.gameObject;
        }
    }
}