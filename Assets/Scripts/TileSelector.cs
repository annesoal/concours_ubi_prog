using System;
using Grid;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private GameObject quad;
    [SerializeField] private Player player;
    private Collider _collider; 
    public InputAction move;
    private GridHelper _helper; 
    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _helper = new SelectorGridHelper();
    }

    //Permet de deplacer le Selector... TODO : A changer car trop saccade!  
    public void Control()
    {
        Vector2Int input = new Vector2Int(0,0);
        // x de input, y de input == x,z en 3d

        if (Input.GetKeyDown(KeyCode.W))
            input.y += 1; 
        else if (Input.GetKeyDown(KeyCode.S))
            input.y -= 1;
        else if (Input.GetKeyDown(KeyCode.A))
            input.x -= 1;
        else if (Input.GetKeyDown(KeyCode.D))
            input.x += 1;

        if (_helper.IsValidTile(input))
            _helper.SetHelperPosition(input);

        Vector3 nextPosition = TilingGrid.GridPositionToLocal(_helper.GetHelperPosition());
        
        transform.position += nextPosition;
        
        if (Input.GetKeyDown(KeyCode.Space) && IsValidTile()) MovePlayer(); 
    }

    // prablement a diviser en sous methode ? 
    // deplace le joueur la ou le selector se trouve, empeche le selector de bouger, cache le selector et indique 
    // au joueur qu'il peut recommencer le processus de selection
    public void MovePlayer()
    {
        player.transform.position = 
            new Vector3(transform.localPosition.x, player.transform.position.y, transform.localPosition.z);
        player.CanSelectectNextTile = true;
        Destroy();
    }

    public void Initialize(Vector3 position)
    {
        transform.position = new Vector3(position.x, 0.51f ,position.z); 
        transform.rotation = Quaternion.Euler(0, 0, 0); 
        quad.SetActive(true);
    }

    private void Destroy()
    {
       quad.SetActive(false);
       player.CanSelectectNextTile = true;
    }

    private bool IsValidTile()
    {
        
            
    }

}