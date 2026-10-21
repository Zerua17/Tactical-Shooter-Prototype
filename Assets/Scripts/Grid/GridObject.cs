using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class GridObject 
{
    private GridSystem gridSystem;
    private GridPosition gridPosition;

    private List<Unit> unitList;

    public GridObject (GridSystem gridSystem, GridPosition gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
    }

    public override string ToString()
    {
        string unitString = "";

        foreach(Unit unit in unitList)
        {
            unitString += unit + "\n";
        }

        return gridPosition.ToString() + "\n" + unitString;
    }

    public List<Unit> GetUnitList()
    {
        return this.unitList;
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public void AddUnit(Unit unit)
    {
        this.unitList.Add(unit);   
    }

    public bool IsOccupied()
    {
        return unitList.Count > 0;
    }
}
