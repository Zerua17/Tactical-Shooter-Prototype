using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    private int turnNumber;

    public int TurnNumber => turnNumber;

    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChange;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one TurnSystem!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        turnNumber = 1;
    }



    public void NextTurn()
    {
        turnNumber++;
        OnTurnChange?.Invoke(this, EventArgs.Empty);
    }
}
