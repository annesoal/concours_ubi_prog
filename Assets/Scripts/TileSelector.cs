using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private GameObject quad;
    [SerializeField] private Player player;
    private Collider _collider; 
    public InputAction move;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    //Permet de deplacer le Selector... TODO : A changer car trop saccade!  
    public void Control()
    {
        Vector2 input = new Vector2(0,0);
        // x de input, y de input == x,z en 3d

        if (Input.GetKeyDown(KeyCode.W))
            input.y += 1; 
        else if (Input.GetKeyDown(KeyCode.S))
            input.y -= 1;
        else if (Input.GetKeyDown(KeyCode.A))
            input.x -= 1;
        else if (Input.GetKeyDown(KeyCode.D))
            input.x += 1; 
        
        var directionToAdd = new Vector3(input.x, 0, input.y);
        
        transform.position += directionToAdd;
        
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
        Debug.Log(position);
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
        Vector3 origin = transform.position;
        Vector3 direction = new Vector3(transform.position.x, -1, transform.position.z);
        
        Debug.Log(origin);
        Debug.Log(direction);
        Debug.Log(Physics.Raycast(origin, direction, maxDistance:1.0f));
        return Physics.Raycast(origin, direction, maxDistance: 1.0f);
        
            
    }

}