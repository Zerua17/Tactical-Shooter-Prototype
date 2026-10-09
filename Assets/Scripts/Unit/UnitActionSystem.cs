using UnityEngine;
using System;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitLayerMask;

    public event EventHandler OnSelectedUnitChange;

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

    private void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;
            
                selectedUnit.Move(MouseWorld.GetPosition());         
        }
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitLayerMask))
        {
            if(raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit newSelectedUnit)
    {
        selectedUnit = newSelectedUnit;
        OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);     //Checks if the event is null (the "?" symbol), then Invokes (Calls) it
    }

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }

}
