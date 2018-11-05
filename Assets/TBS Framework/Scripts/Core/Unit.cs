using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

/// <summary>
/// Base class for all units in the game.
/// </summary>
public abstract class Unit : MonoBehaviour
{
    public bool isSelected = false;
    public bool canAttack = false;

    public Vector3 Offset;
    /// <summary>
    /// UnitClicked event is invoked when user clicks the unit. It requires a collider on the unit game object to work.
    /// </summary>
    public event EventHandler UnitClicked;
    /// <summary>
    /// UnitSelected event is invoked when user clicks on unit that belongs to him. It requires a collider on the unit game object to work.
    /// </summary>
    public event EventHandler UnitSelected;
    public event EventHandler UnitDeselected;
    /// <summary>
    /// UnitHighlighted event is invoked when user moves cursor over the unit. It requires a collider on the unit game object to work.
    /// </summary>
    public event EventHandler UnitHighlighted;
    public event EventHandler UnitDehighlighted;
    public event EventHandler<AttackEventArgs> UnitAttacked;
    public event EventHandler<AttackEventArgs> UnitDestroyed;
    public event EventHandler<MovementEventArgs> UnitMoved;

    public UnitState UnitState { get; set; }
    public void SetState(UnitState state)
    {
        UnitState.MakeTransition(state);
    }

    public List<Buff> Buffs { get; private set; }
    //unit stats
    public string UnitName;
    public int Armor = 10;
    public int TotalArmor { get; set; }

    public int HitPoints;
    public int TotalHitPoints { get; private set; }

    /// <summary>
    /// Determines how far on the grid the unit can move.
    /// </summary>
    public int MovementPoints;
    protected int TotalMovementPoints;

    /// <summary>
    /// Determines how many attacks unit can perform in one turn.
    /// </summary>
    public int ActionPoints;
    protected int TotalActionPoints;

    /// <summary>
    /// Determines the range of a unit's attack.
    /// </summary>
    public int AttackRange;
    public int BaseAttackRange;

    /// <summary>
    /// Determines the power of a unit's attack.
    /// </summary>
    public int AttackFactor;
    public int BaseAttack;
    public int AttackAOE;
    public int GunAttack = 5;
    public int DefenceFactor;
    public int Speed;

    /// <summary>
    /// Handles sfx effects
    /// </summary>
    public SFXLoader sfx;

    /// <summary>
    /// Cell that the unit is currently occupying.
    /// </summary>
    public Cell Cell { get; set; }

    
    /// <summary>
    /// Determines speed of movement animation.
    /// </summary>
    public float MovementSpeed;
    

    /// <summary>
    /// Indicates the player that the unit belongs to. Should correspoond with PlayerNumber variable on Player script.
    /// </summary>
    public int PlayerNumber;

    /// <summary>
    /// Indicates if movement animation is playing.
    /// </summary>
    public bool isMoving { get; set; }

    /// <summary>
    /// Audio clips that may be played, by audio name
    /// </summary>
    public AudioClip atkHpAud;
    public AudioClip atkArmorAud;
    public AudioClip gunAud;
    public AudioClip aoeAud;
    public AudioClip moveAud;
    public AudioClip deathAud;

    private static IPathfinding _pathfinder = new AStarPathfinding();

    /// <summary>
    /// Method called after object instantiation to initialize fields etc. 
    /// </summary>
    public virtual void Initialize()
    {
        Buffs = new List<Buff>();

        AttackAOE = 1;
        UnitState = new UnitStateNormal(this);
        TotalArmor = Armor;
        BaseAttackRange = AttackRange;
        TotalHitPoints = HitPoints;
        TotalMovementPoints = MovementPoints;
        TotalActionPoints = ActionPoints;
        BaseAttack = AttackFactor;
        sfx = GameObject.Find("SFX Source").GetComponent<SFXLoader>();
}

    protected virtual void OnMouseDown()
    {
        if (UnitClicked != null)
            UnitClicked.Invoke(this, new EventArgs());
    }
    protected virtual void OnMouseEnter()
    {
        if (UnitHighlighted != null)
            UnitHighlighted.Invoke(this, new EventArgs());
    }
    protected virtual void OnMouseExit()
    {
        if (UnitDehighlighted != null)
            UnitDehighlighted.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Method is called at the start of each turn.
    /// </summary>
    public virtual void OnTurnStart()
    {
        MovementPoints = TotalMovementPoints;
        ActionPoints = TotalActionPoints;
        //set attack to half if Hp is half, this adds an element of strategy to the gameplay
        //now you have to think about hitting hp despite higher enemy armor
        if ((float)TotalHitPoints/HitPoints >= 2)
        {
            AttackFactor = BaseAttack/2;
        }
        SetState(new UnitStateMarkedAsFriendly(this));
    }

    /// <summary>
    /// Method is called at the end of each turn. Handles the duration of buffs and resets the state of the current unit
    /// Used to make sure that buffs get taken off the board once their time is up and that the unit's state is 
    /// clean for the next turn
    /// </summary>
    public virtual void OnTurnEnd()
    {
        Buffs.FindAll(b => b.Duration == 0).ForEach(b => { b.Undo(this); });
        Buffs.RemoveAll(b => b.Duration == 0);
        Buffs.ForEach(b => { b.Duration--; });

        SetState(new UnitStateNormal(this));
    }

    /// <summary>
    /// Method is called when units HP drops below 1. Destroys the unit such that
    /// there is one less unit on the board, advancing the combat game state to
    /// either victory or defeat
    /// </summary>
    public virtual void OnDestroyed()
    {
        Cell.IsTaken = false;
        try
        {
            sfx.LoadSFX(deathAud, 0f);
        }
        catch { }
        MarkAsDestroyed();
        Destroy(gameObject);

    }

    /// <summary>
    /// Method is called when unit is selected.
    /// </summary>
    public virtual void OnUnitSelected()
    {
        SetState(new UnitStateMarkedAsSelected(this));
        if (UnitSelected != null)
            UnitSelected.Invoke(this, new EventArgs());
    }
    /// <summary>
    /// Method is called when unit is deselected.
    /// </summary>
    public virtual void OnUnitDeselected()
    {
        SetState(new UnitStateMarkedAsFriendly(this));
        if (UnitDeselected != null)
            UnitDeselected.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// Method indicates if it is possible to attack unit given as parameter, from cell given as second parameter.
    /// Used to determine if attacks are possible against the unit in question
    /// </summary>
    public virtual bool IsUnitAttackable(Unit other, Cell sourceCell)
    {
        if (sourceCell.GetDistance(other.Cell) <= AttackRange + AttackAOE - 1)
            return true;

        return false;
    }

    /// <summary>
    /// Method deals damage to unit given as parameter. This method also handles non-possible actions (i.e. no action points)
    /// This is used to handle most of the processes surrounding damage calculation related to an attack.
    /// Allows units to deal damage, and therefore advancing the combat game state
    /// </summary>
    public virtual void DealDamage(Unit other, bool isHp, bool isTrueDamage)
    {
        print("deal Damage branch: ");
        if (isMoving)
        {
            print("isMoving");
            return;
        }
        if (ActionPoints == 0)
        {
            print("no action points");
            return;
        }
        if (!IsUnitAttackable(other, Cell))
        {
            print("not attackable");
            return;
        }

        try
        {
            if (isHp & !isTrueDamage)
            {
                sfx.LoadSFX(atkHpAud, 0f);
            }
            else if (isHp) { sfx.LoadSFX(gunAud,0f); }
            else { sfx.LoadSFX(atkArmorAud, 0f); }

        }
        catch { }
        print("attacked");
        MarkAsAttacking(other);
        other.Defend(this, AttackFactor, isHp, isTrueDamage);
        ActionPoints--;
        print("actionpoints after attack:" + ActionPoints.ToString());
        
        if (ActionPoints == 0)
        {
            SetState(new UnitStateMarkedAsFinished(this));
            MovementPoints = 0;
        }  
    }

    public virtual void DealDamage(Unit other, bool isHp, bool isTrueDamage, bool isAoe)
    {
        print("deal Damage branch: ");
        if (isMoving)
        {
            print("isMoving");
            return;
        }
        if (ActionPoints == 0)
        {
            print("no action points");
            return;
        }
        if (!IsUnitAttackable(other, Cell))
        {
            print("not attackable");
            return;
        }

        try
        {
            if (isAoe)
            {
                sfx.LoadSFX(aoeAud, 0f);
            }else if (isTrueDamage)
            {
                sfx.LoadSFX(gunAud, 0f);
            }else if (isHp)
            {
                sfx.LoadSFX(atkHpAud , 0f);
            }
            else { sfx.LoadSFX(atkArmorAud, 0f); }

        }
        catch { }
        print("attacked");
        MarkAsAttacking(other);
        other.Defend(this, AttackFactor, isHp, isTrueDamage);
        ActionPoints--;
        print("actionpoints after attack:" + ActionPoints.ToString());

        if (ActionPoints == 0)
        {
            SetState(new UnitStateMarkedAsFinished(this));
            MovementPoints = 0;
        }
    }

    /// <summary>
    /// Simple method for unit taking damage. Used in testing and some items to do flat damage instead
    /// doing actual damage calculation.
    /// </summary>
    public virtual void TakeDamage(int amount, bool isHp)
    {
        if(isHp)
        {
            HitPoints -= amount;
        }
        else
        {
            Armor -= amount;
        }
        if (HitPoints <= 0)
        {
            if (UnitDestroyed != null)
                UnitDestroyed.Invoke(this, new AttackEventArgs(this, this, amount));
            OnDestroyed();
        }
    }

    /// <summary>
    /// Attacking unit calls Defend method on defending unit. This method is used for calculating
    /// damage done to the unit. This is to fulfil the requirement that units take damage and 
    /// advances the combat game state
    /// </summary>
    protected virtual void Defend(Unit other, int damage, bool isHp, bool isTrueDamage)
    {
        MarkAsDefending(other);
        if(isHp)
        {
            if (isTrueDamage)
            {
                HitPoints -= damage;
            }
            else
            {
                
                HitPoints -= Mathf.Clamp(damage - Armor, 1, damage); //Damage is calculated by subtracting attack factor of attacker and defence factor of defender. If result is below 1, it is set to 1.
                                                                     //This behaviour can be overridden in derived classes.
            }

        }
        else
        {
            Armor -= damage;
            if (Armor < 0)
            {
                Armor = 0;
            }
        }

        if (UnitAttacked != null)
            UnitAttacked.Invoke(this, new AttackEventArgs(other, this, damage));

        if (HitPoints <= 0)
        {
            if (UnitDestroyed != null)
                UnitDestroyed.Invoke(this, new AttackEventArgs(other, this, damage));
            OnDestroyed();
        }
    }

    /// <summary>
    /// Method for moving one unit from one cell to another. This method fulfills the requirement of
    /// units being able to move, and moving with movment points. Allows for the flow of combat and 
    /// to add a dimension to the turn based combat system
    /// </summary>
    public virtual void Move(Cell destinationCell, List<Cell> path)
    {
        if (isMoving)
            return;

        var totalMovementCost = path.Sum(h => h.MovementCost);
        if (MovementPoints < totalMovementCost)
            return;

        MovementPoints -= totalMovementCost;
        Cell.unit = null;
        Cell.IsTaken = false;
        Cell = destinationCell;
        Cell.unit = this;
        destinationCell.IsTaken = true;

        if (MovementSpeed > 0)
        {
            try
            {
                    sfx.LoadSFX(moveAud, 0f);
            }
            catch { }
            StartCoroutine(MovementAnimation(path));
        }
        else
            transform.position = Cell.transform.position + Offset;

        if (UnitMoved != null)
            UnitMoved.Invoke(this, new MovementEventArgs(Cell, destinationCell, path));

        foreach (SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sortingOrder = Cell.cellNumber;
        }  
    }
    protected virtual IEnumerator MovementAnimation(List<Cell> path)
    {
        isMoving = true;

        path.Reverse();
        foreach (var cell in path)
        {
            while (new Vector2(transform.position.x,transform.position.y) != new Vector2(cell.transform.position.x,cell.transform.position.y))
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(cell.transform.position.x,cell.transform.position.y,transform.position.z), Time.deltaTime * 1.5f);
                yield return 0;
            }
        }

        isMoving = false;
    }

    ///<summary>
    /// Method indicates if unit is capable of moving to cell given as parameter.
    /// </summary>
    public virtual bool IsCellMovableTo(Cell cell)
    {
        return !cell.IsTaken;
    }

    /// <summary>
    /// Method for determining if the cell in question in attackable. Used as helper to handle attack actions
    /// </summary>
    public virtual bool IsCellAttackable(Cell cell)
    {
        if (!cell.IsTaken)
        {
            return true;
        }
        else if (cell.unit != null && cell.unit.PlayerNumber != PlayerNumber)
        {
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Method indicates if unit is capable of moving through cell given as parameter. Used to make sure units do not overlap,
    /// or try to move to obstacles
    /// </summary>
    public virtual bool IsCellTraversable(Cell cell)
    {
        return !cell.IsTaken;
    }

    /// <summary>
    /// Method returns all cells that the unit is capable of moving to. This is used by the Cell grid state to handling highlighting cells
    /// and to determine which cells are movable to based on the unit's movment points. The method returns a list of movable cells for
    /// later methods to use
    /// </summary>
    public List<Cell> GetAvailableDestinations(List<Cell> cells)
    {
        var ret = new List<Cell>();
        var cellsInMovementRange = cells.FindAll(c => IsCellMovableTo(c) && c.GetDistance(Cell) <= MovementPoints);

        var traversableCells = cells.FindAll(c => IsCellTraversable(c) && c.GetDistance(Cell) <= MovementPoints);
        traversableCells.Add(Cell);

        foreach (var cellInRange in cellsInMovementRange)
        {
            if (cellInRange.Equals(Cell)) continue;

            var path = FindPath(traversableCells, cellInRange);
            var pathCost = path.Sum(c => c.MovementCost);
            if (pathCost > 0 && pathCost <= MovementPoints)
                ret.AddRange(path);
        }
        return ret.FindAll(IsCellMovableTo).Distinct().ToList();
    }

    /// <summary>
    /// Method returns all cells that the unit is capable of Attacking. Ignores friendly units and obstacles
    /// Used for highlighting, and for determining if an attack with succeed
    /// </summary>
    public List<Cell> GetAvailableAttacks(List<Cell> cells)
    {
        var cellsInMovementRange = cells.FindAll(c => IsCellMovableTo(c) && c.GetDistance(Cell) <= MovementPoints);
        List<Cell> cellsInAttackRange = new List<Cell>();
        if (!cellsInMovementRange.Any())
        {
            return cells.FindAll(c => IsCellAttackable(c) && c.GetDistance(Cell) <= AttackRange);
        }
        foreach (Cell cell in cellsInMovementRange)
        {
            cellsInAttackRange.AddRange(cells.FindAll(c => IsCellAttackable(c) && c.GetDistance(cell) <= AttackRange));
        }
        return cellsInAttackRange.Distinct().ToList();
    }

    /// <summary>
    /// Method for finding path from one cell to another. Used to determine movement, perform the move animation
    /// and to highliht the path for user feedback. Useful for allowing proper movement on the grid without
    /// teleporting
    /// </summary>
    public List<Cell> FindPath(List<Cell> cells, Cell destination)
    {
        return _pathfinder.FindPath(GetGraphEdges(cells), Cell, destination);
    }

    /// <summary>
    /// Method returns graph representation of cell grid for pathfinding.
    /// </summary>
    protected virtual Dictionary<Cell, Dictionary<Cell, int>> GetGraphEdges(List<Cell> cells)
    {
        Dictionary<Cell, Dictionary<Cell, int>> ret = new Dictionary<Cell, Dictionary<Cell, int>>();
        foreach (var cell in cells)
        {
            if (IsCellTraversable(cell) || cell.Equals(Cell))
            {
                ret[cell] = new Dictionary<Cell, int>();
                foreach (var neighbour in cell.GetNeighbours(cells).FindAll(IsCellTraversable))
                {
                    ret[cell][neighbour] = neighbour.MovementCost;
                }
            }
        }
        return ret;
    }

    /// <summary>
    /// Gives visual indication that the unit is under attack.
    /// </summary>
    /// <param name="other"></param>
    public abstract void MarkAsDefending(Unit other);
    /// <summary>
    /// Gives visual indication that the unit is attacking.
    /// </summary>
    /// <param name="other"></param>
    public abstract void MarkAsAttacking(Unit other);
    /// <summary>
    /// Gives visual indication that the unit is destroyed. It gets called right before the unit game object is
    /// destroyed, so either instantiate some new object to indicate destruction or redesign Defend method. 
    /// </summary>
    public abstract void MarkAsDestroyed();

    /// <summary>
    /// Method marks unit as current players unit.
    /// </summary>
    public abstract void MarkAsFriendly();
    /// <summary>
    /// Method mark units to indicate user that the unit is in range and can be attacked.
    /// </summary>
    public abstract void MarkAsReachableEnemy();
    /// <summary>
    /// Method marks unit as currently selected, to distinguish it from other units.
    /// </summary>
    public abstract void MarkAsSelected();
    /// <summary>
    /// Method marks unit to indicate user that he can't do anything more with it this turn.
    /// </summary>
    public abstract void MarkAsFinished();
    /// <summary>
    /// Method returns the unit to its base appearance
    /// </summary>
    public abstract void UnMark();
}

public class MovementEventArgs : EventArgs
{
    public Cell OriginCell;
    public Cell DestinationCell;
    public List<Cell> Path;

    public MovementEventArgs(Cell sourceCell, Cell destinationCell, List<Cell> path)
    {
        OriginCell = sourceCell;
        DestinationCell = destinationCell;
        Path = path;
    }
}

public class AttackEventArgs : EventArgs
{
    public Unit Attacker;
    public Unit Defender;

    public int Damage;

    public AttackEventArgs(Unit attacker, Unit defender, int damage)
    {
        Attacker = attacker;
        Defender = defender;

        Damage = damage;
    }
}
