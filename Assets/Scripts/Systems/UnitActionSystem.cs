using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    public event EventHandler OnSelectedUnitChange;

    public event EventHandler OnSelectedActionChange;
    public event EventHandler<bool> OnBusyChange;
    public event EventHandler OnActionStart;


    private BaseAction selectedAction;
    private bool isBusy;

    public bool IsBusy => isBusy;

    public static UnitActionSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitActionSystem!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if(isBusy)
        {
            return;
        }

       if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (TryHandleUnitSelection())
            {
                return;
            }
       

        HandleSelectedAction();

    } 

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                if (selectedUnit.TrySpendAP(selectedAction))
                {
                    SetBusy();
                    selectedAction.TakeAction(mouseGridPosition, ClearBusy);
                    OnActionStart?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if(unit == selectedUnit)
                    {
                        return false;
                    } 
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
        
    }

    private void SetSelectedUnit(Unit newSelectedUnit)
    {
        selectedUnit = newSelectedUnit;
        SetSelectedAction(selectedUnit.GetMoveAction());
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);     //Checks if the event is null (the "?" symbol), then Invokes (Calls) it
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedActionChange?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChange?.Invoke(this, isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChange?.Invoke(this, isBusy);
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

}
