using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Grid.Blocks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;
using UnityEngine.Serialization;

public class PlayerTileSelector : MonoBehaviour
{
    [SerializeField] private GameObject _quad;
    private MeshRenderer _renderer;
    [SerializeField] private Material _baseMaterial; 
    [SerializeField] private Material _confirmedMaterial; 
    private GridHelper _helper;
    private Recorder<Cell> _recorder;
    public bool isSelecting = false;

    private void Awake()
    {
        _renderer = _quad.GetComponent<MeshRenderer>();
    }
    /// <summary>
    /// Permet de deplacer le Selector... 
    /// </summary>
    /// <param name="direction"></param>
    private int debug = 0; 
    public MoveType MoveSelector( Vector2Int direction)
    {
        debug++;
        MoveType hasMoved = MoveType.Invalid;
        if (direction != Vector2Int.zero && _helper.IsValidCell(direction))
        {
            _helper.SetHelperPosition(direction);
            if (_recorder.Size() > 1)
            {
                Vector2Int cellAtDirection = _helper.GetHelperPosition();
                hasMoved = cellAtDirection == _recorder.HeadSecond().position 
                    ? MoveType.ConsumeLast : MoveType.New ;
            }
            else
            {
                hasMoved = MoveType.New;
            }
        }

        var nextPosition = TilingGrid.GridPositionToLocal(_helper.GetHelperPosition());
        transform.position = nextPosition;
        return hasMoved;
    }

    public void AddToRecorder()
    {
        Cell cell = TilingGrid.grid.GetCell(_helper.GetHelperPosition());
        _recorder.Add(cell);
    }
    public void RemoveFromRecorder()
    {
        _recorder.RemoveFirst();
    }


    /// <summary>
    ///  Initialise le Selecteur, en le deplacant sous le joueur, active le renderer
    ///  et initialise le Helper dans la grille.
    /// </summary>
    /// <param name="position"> La position dans le monde actuel </param>
    public void Initialize(Vector3 position)
    {
        transform.position = new Vector3(position.x,TilingGrid.TopOfCell, position.z);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        _quad.SetActive(true);
        InitializeHelper();
    }

    /// <summary>
    /// Initialize le Helper avec la position du selecteur et un recorder 
    /// </summary>
    private void InitializeHelper()
    {
        _recorder = new();
        Vector2Int gridPosition = TilingGrid.LocalToGridPosition(transform.position);
        _helper = new PlayerSelectorGridHelper(gridPosition, _recorder);
    }
    
    /// <summary>
    /// Cache le visuel du Selector et indique que le selector n'est plus en train de selectionner
    /// </summary>
    public void Disable()
    {
        isSelecting = false;
        _quad.SetActive(false);
    }

    /// <summary>
    /// Cache le Selector, reset le recorder et place sous la du joueur 
    /// </summary>
    public void ResetSelf()
    {
        Disable();
        if (_recorder != null && !_recorder.IsEmpty())
        {
            Cell lastCell = _recorder.RemoveLast();
            _recorder.Reset();
            _recorder.Add(lastCell);
        }
    }
    
    /// <summary>
    /// Enlever une Cell du recorder et donne sa position
    /// </summary>
    /// <returns>null si le _recoder est null ou vide, la position de la cell sinon</returns>
    public Vector2Int? GetNextPositionToGo()
    {
        if (_recorder == null || _recorder.IsEmpty())
            return null;
        
        return _recorder.RemoveLast().position;
    }

    public Vector2Int GetCurrentPosition()
    {
        return _helper.Cell.position;
    }

    public void Confirm()
    {
       ChangeSelectorVisualToConfirm(); 
    }

    public void Select()
    {
        _quad.SetActive(true);
       ChangeSelectorVisualToSelect(); 
    }

    private void ChangeSelectorVisualToConfirm()
    {
        _renderer.material = _confirmedMaterial; 
    }
    
    private void ChangeSelectorVisualToSelect()
    {
        _renderer.material = _baseMaterial; 
    }


}
