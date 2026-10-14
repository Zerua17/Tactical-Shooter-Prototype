using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    private GridObject gridObject;
    [SerializeField] private TextMeshPro textMeshPro;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    public void Update()
    { 
        textMeshPro.text = gridObject.ToString();
    }
}
