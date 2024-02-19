using UnityEngine;
using UnityEngine.InputSystem;

public class TileSelector : MonoBehaviour
{
    private const float Cooldown = 0.1f;
    [SerializeField] private GameObject quad;
    [SerializeField] private Player player;
    public InputAction move;
    private bool _canMove;
    private float _cooldown;
    

    public bool CanMove
    {
        set => _canMove = value;
    }

    private void Start()
    {
        move.Enable();
    }

    private void Update()
    {
        if (_canMove) Move();
        // TODO : utiliser le nouveau system ! 
        if (Input.GetKey(KeyCode.Space)) MovePlayer(); 
    }

    //Permet de deplacer le Selector... TODO : A changer car trop saccade!  
    public void Move()
    {
        var input = move.ReadValue<Vector2>();
        // x de input, y de input == x,z en 3d
        var directionToAdd = new Vector3(input.x, 0, input.y);

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

    // prablement a diviser en sous methode ? 
    // deplace le joueur la ou le selector se trouve, empeche le selector de bouger, cache le selector et indique 
    // au joueur qu'il peut recommencer le processus de selection
    public void MovePlayer()
    {
        _canMove = false;
        player.transform.position = 
            new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        player.CanSelectectNextTile = true;
        Hide();
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