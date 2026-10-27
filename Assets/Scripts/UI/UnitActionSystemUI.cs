using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SearchService;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private GameObject tacticalHUD;

    private List<ActionButtonUI> actionButtonList;
    
    private void Awake()
    {
        actionButtonList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnBusyChange += UnitActionSystem_OnBusyChange;
        UnitActionSystem.Instance.OnActionStart += UnitActionSystem_OnActionStart;
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        Unit.OnAnyActionPointsChange += Unit_OnAnyActionPointsChange;

        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UnitActionSystem_OnBusyChange(this, UnitActionSystem.Instance.IsBusy);
        UpdateActionPoints();
    }

    private void OnDisable()
    {
        if (UnitActionSystem.Instance == null)
            return;

        UnitActionSystem.Instance.OnSelectedUnitChange -= UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChange -= UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnBusyChange -= UnitActionSystem_OnBusyChange;
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
        UpdateActionPoints();
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

    private void UnitActionSystem_OnBusyChange(object sender, bool isBusy)

    {
        tacticalHUD.gameObject.SetActive(!isBusy);
    }

    private void UpdateActionPoints()
    {
        actionPointsText.text = "Action Points: " + UnitActionSystem.Instance.GetSelectedUnit().ActionPoints;
    }

    private void UnitActionSystem_OnActionStart(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointsChange(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }
}
