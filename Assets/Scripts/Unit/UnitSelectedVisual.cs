using UnityEngine;
using System;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private MeshRenderer meshRenderer;

    private Quaternion initialRotation;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChange;
        initialRotation = transform.rotation;
        UpdateVisual();
    }

    private void LateUpdate()
    {
        transform.rotation = initialRotation;
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
            meshRenderer.enabled = true;
        else
            meshRenderer.enabled = false;
    }
}
    