using System;
using UnityEngine;

namespace Grid
{
    public class Ressource : MonoBehaviour
    {
        [SerializeField] private float TopOfCell = 0.72f;

        // Start is called before the first frame update
        void Start()
        {
            Vector3 position = transform.position;
            PlayerSelectorGridHelper helper = 
                new PlayerSelectorGridHelper(TilingGrid.LocalToGridPosition(position));
            helper.AddOnTopCell(gameObject);
            transform.position = TilingGrid.GridPositionToLocal(helper.Cell.position, 0.72f );
        }
    }
}
