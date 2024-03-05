using System.Collections;
using Grid;
using Grid.Blocks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TileSelector : MonoBehaviour
{
    [SerializeField] private GameObject quad;
    [SerializeField] private Player player;
    public InputAction move;
    private GridHelper _helper;
    private CellRecorder _recorder; 

    //Permet de deplacer le Selector...   
    public void MoveSelector( Vector2Int direction)
    {
        if (direction != Vector2Int.zero && _helper.IsValidCell(direction))
            _helper.SetHelperPosition(direction);

        var nextPosition = TilingGrid.GridPositionToLocal(_helper.GetHelperPosition());
        transform.position = nextPosition;
    }
     //;

    // prablement a diviser en sous methode ? 
    // deplace le joueur la ou le selector se trouve, empeche le selector de bouger, cache le selector et indique 
    // au joueur qu'il peut recommencer le processus de selection
    public IEnumerator MoveCharacter()
    {
        while (!_recorder.IsEmpty())
        {
            Cell cell = _recorder.RemoveLast();
            Vector3 cellLocalPosition = TilingGrid.GridPositionToLocal(cell.position);
            Transform playerTransform = player.transform;
            playerTransform.LookAt(cellLocalPosition);
            playerTransform.position = cellLocalPosition;
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Initialise le Selecteur, en le deplacant sous le joueur, active le renderer 
    // et initialise le Helper dans la grille. 
    public void Initialize(Vector3 position)
    {
        transform.position = new Vector3(position.x, 0.51f, position.z);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        quad.SetActive(true);
        InitializeHelper();
    }

    // Initialize le Helper avec la position du selecteur et un recorder
    private void InitializeHelper()
    {
        _recorder = new();
        Vector2Int gridPosition = TilingGrid.LocalToGridPosition(transform.position);
        _helper = new SelectorGridHelper(gridPosition, _recorder);
    }

    public void Destroy()
    {
        if (IsValidTileToMove())
        {
            quad.SetActive(false);
            StartCoroutine(MoveCharacter());
        }
    }

    // Check si la cellule peut permettre au joueur de se deplacer
    private bool IsValidTileToMove()
    {
        var cell = _helper.Cell;
        return cell.Has(BlockType.Walkable);
    }
}