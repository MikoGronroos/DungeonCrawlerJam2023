using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IParticipant
{

    [SerializeField] private Stats currentStats;

    public void Damage(int damage)
    {
        currentStats.CurrentHealth = Mathf.Clamp(currentStats.CurrentHealth - damage, 0, currentStats.MaxHealth);
        if (currentStats.CurrentHealth <= 0)
        {
            HealthHitZero();
        }
    }

    public void EndTurn(Combat combat)
    {
    }

    public Stats GetStats()
    {
        return currentStats;
    }

    public void HealthHitZero()
    {
    }

    public void StartTurn(Combat combat)
    {
        combat.Attack();
    }
}
