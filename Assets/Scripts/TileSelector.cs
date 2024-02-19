using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem; 

namespace DefaultNamespace
{
    public class TileSelector : MonoBehaviour
    {
        [SerializeField] private GameObject quad;
        private bool _canMove;
        private const float Cooldown = 0.1f; 
        private float _cooldown = 0.0f;  
        public InputAction move;
        public bool CanMove
        {
            set
            {
                _canMove = value;
            }
        } 
        void Start()
        {
            move.Enable();
        }
        //Permet de deplacer le Selector... TODO : A changer car trop dur atm !  
        public void Move()
        {
            Vector2 input = move.ReadValue<Vector2>();
            // x de input, y de input == x,z en 3d
            Vector3 directionToAdd = new Vector3(input.x, 0  ,input.y);

            if (_cooldown <= 0.0f)
            {
                transform.position += directionToAdd;
                _cooldown = Cooldown;
            }
            else
            {
                // reduit le cd par le temps entre chaque frame 
                _cooldown -= Time.deltaTime; 
            }
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
            quad.SetActive(false);
        }

        public void Show()
        {
            quad.SetActive(true);
        }
    }
}