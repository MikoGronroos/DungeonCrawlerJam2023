using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IParticipant
{

    [SerializeField] private Stats currentStats;

    public void Damage(int damage)
    {
    }

    public void EndTurn(Combat combat)
    {
    }

    public Stats GetStats()
    {
        return currentStats;
    }

    public void StartTurn(Combat combat)
    {
        StartCoroutine(Turn(combat));
    }

    private IEnumerator Turn(Combat combat)
    {
        yield return new WaitForSeconds(5);
        if (combat.TryToHit())
        {
            if (combat.Attack())
            {
                combat.NextTurn();
            }
            else
            {
                combat.NextTurn();
            }
        }
        else
        {
            combat.NextTurn();
        }
    }

}
