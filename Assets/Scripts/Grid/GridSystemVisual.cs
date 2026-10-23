using NUnit.Framework;
using UnityEditor.Animations;
using UnityEngine;
using System.Collections.Generic;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] Transform gridSystemVisualSinglePrefab;
    private GridVisualSingle[,] gridVisualSingleArray;

    public static GridSystemVisual Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one GridSystemVisual!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        gridVisualSingleArray = new GridVisualSingle[
            LevelGrid.Instance.GetGridWidth(), 
            LevelGrid.Instance.GetGridHeight()];

        for(int x = 0; x < LevelGrid.Instance.GetGridWidth(); x++)
        {
            for(int z = 0; z < LevelGrid.Instance.GetGridHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridVisualSingleTransform = 
                    Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                gridVisualSingleArray[x, z] = gridVisualSingleTransform.GetComponent<GridVisualSingle>();
            }
        }
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void HideAllGridPositions()
    {
        foreach(GridVisualSingle gridVisualSingle in gridVisualSingleArray)
        {
            gridVisualSingle.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach(GridPosition gridPosition in gridPositionList)
        {
            gridVisualSingleArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridPositions();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        ShowGridPositionList(selectedUnit.GetMoveAction().GetValidActionGridPositionList());
    }

}
