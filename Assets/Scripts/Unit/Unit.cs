using System;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour
{
    
    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;


    [SerializeField] private bool isEnemy;

    public bool IsEnemy => isEnemy;

    public static event EventHandler OnAnyActionPointsChange;

    [SerializeField] private const int MAX_ACTION_POINTS = 2;
    [SerializeField]private int actionPoints = MAX_ACTION_POINTS;
    
    public int ActionPoints => actionPoints;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }
   
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;

        healthSystem.OnDeath += HealthSystem_OnDeath;
       
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

    public Vector3 GetWorldPosition()
    {
        return transform.position;
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
        if ((isEnemy && !TurnSystem.Instance.IsPlayerTurn) || (!isEnemy && TurnSystem.Instance.IsPlayerTurn))
        {
            actionPoints = MAX_ACTION_POINTS;
            OnAnyActionPointsChange?.Invoke(this, EventArgs.Empty);
        }

    }

    public void Damage(int damage, Vector3 damageSourcePosition)
    {
        healthSystem.TakeDamage(damage, damageSourcePosition);
    }
   

    private void HealthSystem_OnDeath(object sender, HealthSystem.OnDeathEventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
    }

}
