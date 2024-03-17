using System;
using UnityEngine;

namespace Grid
{
    public class Ressource : MonoBehaviour
    {
        [SerializeField] private float TopOfCell = 0.72f;

        public const string Name = "Resource";
        // Start is called before the first frame update
        void Start()
        {
            Vector3 position = transform.position;
            PlayerSelectorGridHelper helper = 
                new PlayerSelectorGridHelper(TilingGrid.LocalToGridPosition(position));
            helper.AddOnTopCell(gameObject);
            foreach(var variable in helper.Cell.objectsOnTop)
            {
               Debug.Log(variable + " has been added to : " + helper.Cell.position);
            }
            transform.position = TilingGrid.GridPositionToLocal(helper.Cell.position, 0.72f );
        }
        
        public override string ToString()
        {
            return Name;
        }

        public void OnDestroy()
        {
            Debug.Log(this + " has been destroyed");
        }
    }
}
