using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public abstract class Cell : MonoBehaviour, IGraphNode
{
    [HideInInspector]
    [SerializeField]
    private Vector2 _offsetCoord;
    public Vector2 OffsetCoord { get { return _offsetCoord; } set { _offsetCoord = value; } }

    public enum HighlightState {None, Highlighted, Reachable, Path, Attackable, AttackSelected, Friendly, FriendlySelected };

    public int cellNumber { get; set; }

    /// <summary>
    /// Indicates if something is occupying the cell.
    /// </summary>
    public bool IsTaken;
    /// <summary>
    /// Cost of moving through the cell.
    /// </summary>
    public int MovementCost;

    public Unit unit;

    /// <summary>
    /// CellClicked event is invoked when user clicks the unit. It requires a collider on the cell game object to work.
    /// </summary>
    public event EventHandler CellClicked;
    /// <summary>
    /// CellHighlighed event is invoked when user moves cursor over the cell. It requires a collider on the cell game object to work.
    /// </summary>
    public event EventHandler CellHighlighted;
    public event EventHandler CellDehighlighted;

    protected virtual void OnMouseEnter()
    {
        if (CellHighlighted != null)
            CellHighlighted.Invoke(this, new EventArgs());
    }
    protected virtual void OnMouseExit()
    {    
        if (CellDehighlighted != null)
            CellDehighlighted.Invoke(this, new EventArgs());
    }
    void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (CellClicked != null)
                CellClicked.Invoke(this, new EventArgs());
        }
        
    }

    /// <summary>
    /// Method returns distance to other cell, that is given as parameter. 
    /// </summary>
    public abstract int GetDistance(Cell other);

    /// <summary>
    /// Method returns cells adjacent to current cell, from list of cells given as parameter.
    /// </summary>
    public abstract List<Cell> GetNeighbours(List<Cell> cells);
      
    public abstract Vector3 GetCellDimensions(); //Cell dimensions are necessary for grid generators.

    /// <summary>
    /// Method for highlighting cells depending on the situation. Used to provide good user feedback
    /// </summary>
    public abstract void Mark(HighlightState s);

    /// <summary>
    /// Method returns the cell to its base appearance.
    /// </summary>
    public abstract void UnMark();

    



    public int GetDistance(IGraphNode other)
    {
        return GetDistance(other as Cell);
    }
}