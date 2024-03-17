using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Grid
{
    public class Ressource : MonoBehaviour
    {
        [SerializeField] private float TopOfCell = 0.72f;
        private NetworkObject _networkObject;

        // Start is called before the first frame update
        void Start()
        {
            Vector3 position = transform.position;
            PlayerSelectorGridHelper helper = 
                new PlayerSelectorGridHelper(TilingGrid.LocalToGridPosition(position));
            helper.AddOnTopCell(gameObject);
            transform.position = TilingGrid.GridPositionToLocal(helper.Cell.position, 0.72f );
            (_networkObject = this.GetComponent<NetworkObject>()).Spawn();
        }

        private void OnDestroy()
        {
           _networkObject.Despawn();
           Destroy(this);
        }
    }
}
