using System.Net;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    private GridSystem gridSystem;
    [SerializeField] private Transform gameDebugObjectPrefab;
    
    private void Start()
    {
        gridSystem = new GridSystem(10, 10, 3f);
        gridSystem.CreateDebugObject(gameDebugObjectPrefab);
    }

    private void Update()
    {
        Debug.Log(gridSystem.GetGridPosition(MouseWorld.GetPosition()));
    }
}
