using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class MoveAction : BaseAction
{

    [SerializeField] private Animator unitAnimator;
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private int maxMoveDistance = 4;

    
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
        if (Vector3.Distance(transform.position, targetPosition) >= stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            unitAnimator.SetBool("IsWalking", true);
        }
        else
        {
            this.transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
            unitAnimator.SetBool("IsWalking", false);
            isActive = false;
            onActionComplete();
        }
    }




    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        isActive = true;
        this.targetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
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
