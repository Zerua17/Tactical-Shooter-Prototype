using System;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;

    public static event EventHandler OnAnyActionPointsChange;

    [SerializeField] private const int MAX_ACTION_POINTS = 2;
    [SerializeField]private int actionPoints = MAX_ACTION_POINTS;
    
    public int ActionPoints => actionPoints;

    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }
   
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
       
    }

    private void Update()
    {

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(newGridPosition != gridPosition)
        {
            LevelGrid.Instance.ChangeUnitGridPosition(gridPosition, newGridPosition, this);
            gridPosition = newGridPosition;
        }

    } 

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction() 
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;    
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool HasEnoughAP(BaseAction baseAction)
    {
        return (actionPoints >= baseAction.GetAPCost());
    }

    private void SpendAP(int amount)
    {
        this.actionPoints -= amount;
        OnAnyActionPointsChange?.Invoke(this, EventArgs.Empty);
    }

    public bool TrySpendAP(BaseAction baseAction)
    {
        if (HasEnoughAP(baseAction))
            {
            SpendAP(baseAction.GetAPCost());
            return true;
            }
        else 
            return false;
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        actionPoints = MAX_ACTION_POINTS;
        OnAnyActionPointsChange?.Invoke(this, EventArgs.Empty);

    }

   

}
