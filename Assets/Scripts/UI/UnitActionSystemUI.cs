using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;

    private List<ActionButtonUI> actionButtonList;
    
    private void Awake()
    {
        actionButtonList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void CreateUnitActionButtons()
    {
        foreach(Transform buttonTransform in actionButtonContainer)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach(BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainer);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonList.Add(actionButtonUI);
        }
        
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {

        foreach (ActionButtonUI actionButtonUI in actionButtonList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }

    }

}
