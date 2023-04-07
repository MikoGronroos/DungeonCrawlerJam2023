using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IParticipant
{
    [SerializeField] private Stats currentStats;

    private bool _dead = false;

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

    public void EndTurn(Combat combat)
    {
        
    }

    public Stats GetStats()
    {
        return currentStats;
    }

    public void HealthHitZero(Combat combat)
    {
        Debug.Log("health hit zero enemy");
        combat.EndCombat();
    }

    public void StartTurn(Combat combat)
    {
        combat.Attack((bool state) =>
        {
            AttackFinished(state, combat);
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
        gameObject.SetActive(false);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
