using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnActionStart;
    public static event EventHandler OnActionComplete;
    protected Unit unit;
    protected bool isActive;
    protected Action onActionComplete;


    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    public abstract string GetActionName();

    public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetAPCost()
    {
        return 1;
    }

    protected void  ActionStart(Action onActionComplete)
    {
        isActive = true;
        OnActionStart?.Invoke(this, EventArgs.Empty);
        this.onActionComplete = onActionComplete;
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        OnActionComplete?.Invoke(this, EventArgs.Empty);
    }
    
    
    public Unit GetUnit()
    {
        return unit;
    }

}
