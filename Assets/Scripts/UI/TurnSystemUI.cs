using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private Button endTurnBtn;


    private void Start()
    {
        endTurnBtn.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;

        UpdateTurnText();
    }


    public void UpdateTurnText()
    {
        turnText.text = "Turn: " + TurnSystem.Instance.TurnNumber;
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        UpdateTurnText();
    }



}
