using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    [SerializeField] private Unit unit;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            unit.GetMoveAction().GetValidActionGridPositionList();
        }
    }
}
