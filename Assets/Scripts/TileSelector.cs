using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class TileSelector : MonoBehaviour
    {
        [SerializeField] private GameObject quad;
        private MeshRenderer _meshRenderer; 
        private bool _canMove;
        public bool CanMove
        {
            set
            {
                _canMove = value;
            }
        } 
        void Start()
        {
            _meshRenderer = quad.GetComponent<MeshRenderer>(); 
            Debug.Log(_meshRenderer);
        }
        public void Move()
        {
            
            Vector2 movementNormalized = GameInput.Instance.GetMovementNormalized();
            
        }

        void Update()
        {
            if (_canMove)
            {
                Move();
            }
        }

        public void Hide()
        {
            _meshRenderer.enabled = false;
        }

        public void Show()
        {
            _meshRenderer.enabled = true;
        }
    }
}