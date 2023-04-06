using System;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IParticipant
{

    [SerializeField] private Stats currentStats;

    [SerializeField] private GameObject turnPanel;
    [SerializeField] private Button attackButton;

    private bool _dead;

    #region Singleton

    private static Player instance;

    public static Player Instance
    {
        get { return instance; }
    }

    #endregion

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        currentStats.CurrentHealth = currentStats.MaxHealth;
    }

    public bool Damage(Combat combat, int damage)
    {
        currentStats.CurrentHealth = Mathf.Clamp(currentStats.CurrentHealth - damage, 0, currentStats.MaxHealth);
        if (currentStats.CurrentHealth <= 0)
        {
            HealthHitZero(combat);
            _dead = true;
        }
        return _dead;
    }

    public void AddHealth(int health)
    {
        currentStats.CurrentHealth = Mathf.Clamp(currentStats.CurrentHealth + health, 0, currentStats.MaxHealth);
    }

    public void HealthHitZero(Combat combat)
    {
    }

    public void EndTurn(Combat combat)
    {
        attackButton.onClick.RemoveAllListeners();
        turnPanel.SetActive(false);
    }

    public Stats GetStats()
    {
        return currentStats;
    }

    public void StartTurn(Combat combat)
    {
        turnPanel.SetActive(true);
        attackButton.onClick.AddListener(() =>
        {
            combat.Attack((bool state) =>
            {
                AttackFinished(state, combat);
            });
        });
    }

    private void AttackFinished(bool state, Combat combat)
    {
        if (state) // Attack was succesful
        {

        }
        else //Attack was not succesful
        {

        }
        combat.NextTurn();
    }

    public void CombatEnded(Combat combat)
    {
        EndTurn(combat);
    }
}
