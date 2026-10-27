using System;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;

    public override string GetActionName()
    {
        return "Spin";
    }

    private void Update()
    {
        if (!isActive)
        {
            return;
        }
       
        float spinAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);
        totalSpinAmount += spinAmount;

        if(totalSpinAmount >= 360f)
        {
            isActive = false;
            onActionComplete();
        }
        
    }
    public override void TakeAction(GridPosition gridPosition, Action onSpinComplete)
    {
        this.onActionComplete = onSpinComplete;
        totalSpinAmount = 0;
        isActive = true;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        return new List<GridPosition>
        {
            unitGridPosition
        };
    }

    public override int GetAPCost()
    {
        return 2;
    }

}
