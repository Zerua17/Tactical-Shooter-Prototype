
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class MoveAction : BaseAction
{
    [SerializeField] private float rotateSpeed = 720f;
    [SerializeField] private int maxMoveDistance = 4;

    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    
    private Vector3 targetPosition;
    private float moveSpeed = 5f;
    private float stoppingDistance = 0.1f;

    

    public override string GetActionName()
    {
        return "Move";
    }
    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
    }


    private void Update()
    {
        if (!isActive)
            return;

        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        
        if (Vector3.Distance(transform.position, targetPosition) >= stoppingDistance)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );

            
            /*if (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
            {
                unitAnimator.SetBool("IsWalking", false);
                return;
            }*/

            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }
        else
        {
            transform.position = targetPosition;
          
            ActionComplete();
            OnStopMoving?.Invoke(this, EventArgs.Empty);
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete); 
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        OnStartMoving?.Invoke(this, EventArgs.Empty);
    }

    
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }
                if(unitGridPosition == testGridPosition)
                { 
                    //The position where the unit is already at
                    continue;
                }
                if(LevelGrid.Instance.IsOccupied(testGridPosition))
                {
                    //Check if any position within range is already occupied by another unit
                    continue;
                }
                validGridPositionList.Add(testGridPosition);

            }
        }


        return validGridPositionList;
    }


}
