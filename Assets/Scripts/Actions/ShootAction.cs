using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootAction : BaseAction
{

    private enum State
    {
        Aiming,
        Shooting,
        Cooloff,
    }


    private State state;
    private float stateTimer;
    private int maxShootRange = 7;

    private Unit targetUnit;

    private bool canShoot;

    [SerializeField] private float rotateSpeed = 360f;
    [SerializeField] private float aimAngleTolerance = 1f;
    private bool aimingTimeCompleted;

    public event EventHandler<OnShotEventArgs> OnShoot;

    public class OnShotEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }



    private void Update()
    {
        if (!isActive)
        {
            return;
        }

        switch (state)
        {
            case State.Aiming:
                RotateTowardsTarget();

                stateTimer -= Time.deltaTime;

                if (stateTimer <= 0f)
                {
                    aimingTimeCompleted = true;
                }

                if (aimingTimeCompleted && IsFacingTarget())
                {
                    NextState();
                }
                break;

            case State.Shooting:
                stateTimer -= Time.deltaTime;

                if (canShoot)
                {
                    Shoot();
                    canShoot = false;
                }

                if (stateTimer <= 0f)
                {
                    NextState();
                }
                break;

            case State.Cooloff:
                stateTimer -= Time.deltaTime;

                if (stateTimer <= 0f)
                {
                    NextState();
                }
                break;
        }

    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                stateTimer = 1f;
                canShoot = true;
                break;

            case State.Shooting:
                state = State.Cooloff;
                stateTimer = 0.5f;
                break;

            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShotEventArgs
        {
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        targetUnit.Damage(40, unit.transform.position);
    }


    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxShootRange; x <= maxShootRange; x++)
        {
            for (int z = -maxShootRange; z <= maxShootRange; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                {
                    continue;
                }

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if(testDistance > maxShootRange)
                {
                    //Checks for diagonal distances
                    continue;
                }
              

                if (!LevelGrid.Instance.IsOccupied(testGridPosition))
                {
                    //Check if any position within range is empty
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy == unit.IsEnemy)
                {
                    //Both units on the same team
                    continue;
                }
                validGridPositionList.Add(testGridPosition);

            }
        }


        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        stateTimer = 1f;

        aimingTimeCompleted = false;
        canShoot = false;
    }


    private void RotateTowardsTarget()
    {
        if (targetUnit == null)
        {
            return;
        }

        Vector3 direction =
            targetUnit.transform.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotateSpeed * Time.deltaTime
        );
    }

    private bool IsFacingTarget()
    {
        if (targetUnit == null)
        {
            return false;
        }

        Vector3 direction =
            targetUnit.transform.position - transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
        {
            return true;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        return Quaternion.Angle(transform.rotation, targetRotation)
            <= aimAngleTolerance;
    }
}
