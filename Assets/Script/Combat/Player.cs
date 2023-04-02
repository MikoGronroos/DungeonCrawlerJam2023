using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IParticipant
{

    [SerializeField] private Stats currentStats;

    [SerializeField] private GameObject turnPanel;
    [SerializeField] private Button nextTurnButton;

    public void Damage(int damage)
    {
    }

    public void EndTurn(Combat combat)
    {
        nextTurnButton.onClick.RemoveAllListeners();
        turnPanel.SetActive(false);
    }

    public Stats GetStats()
    {
        return currentStats;
    }

    public void StartTurn(Combat combat)
    {
        turnPanel.SetActive(true);
        nextTurnButton.onClick.AddListener(() =>
        {
            combat.NextTurn();
        });
    }
}
