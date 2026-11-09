using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private HealthSystem healthSystem;

    private void Start()
    {
        Unit.OnAnyActionPointsChange += Unit_OnAnyActionPointsChange;
        healthSystem.OnHealthChange += HealthSystem_OnHealthChange;
        UpdateActionPointsText();
        UpdateHealthbar();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = unit.ActionPoints.ToString();
    }

    private void Unit_OnAnyActionPointsChange(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void UpdateHealthbar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void HealthSystem_OnHealthChange(object sender, EventArgs e)
    {
        UpdateHealthbar();
    }

}
