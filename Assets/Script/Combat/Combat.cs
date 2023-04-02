using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Combat : MonoBehaviour
{

    [SerializeField] private int baseDamage = 1;

    [SerializeField] private Enemy enemy;
    [SerializeField] private Player player;

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

    public bool Attack()
    {
        IParticipant attacker = _participants[currentTurn];
        IParticipant target = _participants[previousTurn];
        bool attack = false;
        int newDamage = 0;
        var dice = new Dice();
        var roll = dice.Roll();
        if (attacker.GetStats().Strength + roll >= target.GetStats().Defense)
        {
            newDamage = baseDamage + attacker.GetStats().Strength + roll - target.GetStats().Defense;
            attack = true;
        }
        Debug.Log(newDamage);
        target.Damage(newDamage);
        return attack;
    }

    public bool TryToHit()
    {
        IParticipant attacker = _participants[currentTurn];
        IParticipant target = _participants[previousTurn];
        bool hit = false;
        var dice = new Dice();
        if (attacker.GetStats().Accuracy + dice.Roll() >= target.GetStats().Evasion)
        {
            hit = true;
        }
        return hit;
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

[Serializable]
public class Dice
{

    int maxValue;
    int minValue;

    public int Roll()
    {
        return Random.Range(minValue, maxValue) + 1;
    }
}

public interface IParticipant
{
    void StartTurn(Combat combat);

    void EndTurn(Combat combat);

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