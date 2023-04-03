using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Combat : MonoBehaviour
{

    [SerializeField] private Enemy enemy;
    [SerializeField] private Player player;

    [SerializeField] private Dice dicePrefab;
    [SerializeField] private Transform diceSpawnPos;

    private int currentTurn;
    private int previousTurn;
    private bool isInCombat = false;
    private List<IParticipant> _participants = new List<IParticipant>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) StartCombat(enemy, player);
    }

    public void StartCombat(params IParticipant[] values)
    {
        foreach (var item in values)
        {
            _participants.Add(item);
        }
        isInCombat = true;
        currentTurn = 0;
        previousTurn = 1;
        _participants[currentTurn].StartTurn(this);
    }

    public void EndCombat()
    {
        isInCombat = false;
    }

    private IEnumerator AttackIEnumerator()
    {
        IParticipant attacker = _participants[currentTurn];
        IParticipant target = _participants[previousTurn];
        int newDamage = 0;
        var dice = Instantiate(dicePrefab, diceSpawnPos.position, Quaternion.identity);
        Debug.Log(target);
        yield return new WaitUntil(() => dice.diceLanded);
        if (attacker.GetStats().Strength + dice.sideLandedOn >= target.GetStats().Defense)
        {
            newDamage = attacker.GetStats().Strength + dice.sideLandedOn - target.GetStats().Defense;
        }
        Debug.Log(dice.sideLandedOn);
        Debug.Log(newDamage);
        target.Damage(newDamage);
    }

    private IEnumerator TryToHitIEnumerator()
    {
        IParticipant attacker = _participants[currentTurn];
        IParticipant target = _participants[previousTurn];
        var dice = Instantiate(dicePrefab, diceSpawnPos.position, Quaternion.identity);
        yield return new WaitUntil(() => dice.diceLanded);
        if (attacker.GetStats().Accuracy + dice.sideLandedOn >= target.GetStats().Evasion)
        {
            StartCoroutine(AttackIEnumerator());
            Debug.Log("Hit");
        }
    }

    public void Attack()
    {
        StartCoroutine(TryToHitIEnumerator());
    }

    public void NextTurn()
    {
        _participants[currentTurn].EndTurn(this);
        previousTurn = currentTurn;
        if (currentTurn >= _participants.Count)
        {
            currentTurn = 0;
        }
        else
        {
            currentTurn++;
        }
        _participants[currentTurn].StartTurn(this);
    }

}

public interface IParticipant
{
    void StartTurn(Combat combat);

    void EndTurn(Combat combat);

    void HealthHitZero();

    void Damage(int damage);

    Stats GetStats();

}

[Serializable]
public class Stats
{
    public int Strength = 0;
    public int Evasion = 0;
    public int Accuracy = 0;
    public int Defense = 0;
    public int MaxHealth = 0;
    public int CurrentHealth = 0;
}