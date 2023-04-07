using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Combat : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private Dice dicePrefab;
    [SerializeField] private Transform diceSpawnPos;

    private int currentTurn;
    private int previousTurn;
    private bool isInCombat = false;
    private List<IParticipant> _participants = new List<IParticipant>();

    private List<GameObject> dices = new List<GameObject>();

    #region Singleton

    private static Combat instance;

    public static Combat Instance
    {
        get { return instance; }
    }

    #endregion

    private void Awake()
    {
        instance = this;
    }
    public void StartCombat(IParticipant enemy)
    {
        Transform target = enemy.GetTransform();
        playerMovement.LookAtTheEnemy(target);
        playerMovement.CanMove = false;
        _participants.Add(player);
        _participants.Add(enemy);
        isInCombat = true;
        currentTurn = 0;
        previousTurn = 1;
        _participants[currentTurn].StartTurn(this);
    }

    public void EndCombat()
    {
        StopAllCoroutines();
        foreach (var item in _participants)
        {
            item.CombatEnded(this);
        }
        _participants = new List<IParticipant>();
        CleanUpDices();
        isInCombat = false;
        playerMovement.CanMove = true;
    }

    private IEnumerator AttackIEnumerator(Action<bool> callback)
    {
        IParticipant attacker = _participants[currentTurn];
        IParticipant target = _participants[previousTurn];

        // var dmg = attacker.GetStats().Strength;
        target.Damage(this, 999999);
        
        if (attacker is Player)
        {
            player.AddHealth(target.GetStats().MaxHealth);
        }
        
        callback.Invoke(true);
        
        yield return new WaitForSeconds(1);
        
        // int newDamage = 0;
        // if (!attacker.GetStats().isInstakill)
        // {
        //     var dice = Instantiate(dicePrefab, diceSpawnPos.position, Quaternion.identity);
        //     dices.Add(dice.gameObject);
        //     yield return new WaitUntil(() => dice.diceLanded);
        //     if (attacker.GetStats().Strength + dice.sideLandedOn >= target.GetStats().Defense)
        //     {
        //         newDamage = 1;
        //         target.Damage(this, newDamage);
        //         callback?.Invoke(true);
        //     }
        //     else
        //     {
        //         callback?.Invoke(false);
        //     }
        // }
        // else
        // {
        //     target.Damage(this, 1000); 
        //     callback?.Invoke(true);
        // }
       
    }

    public void Attack(Action<bool> attackFinishedCallback)
    {
        StartCoroutine(AttackIEnumerator(attackFinishedCallback));
    }

    public void NextTurn()
    {
        CleanUpDices();

        _participants[currentTurn].EndTurn(this);
        previousTurn = currentTurn;
        if (currentTurn >= _participants.Count - 1)
        {
            currentTurn = 0;
        }
        else
        {
            currentTurn++;
        }
        _participants[currentTurn].StartTurn(this);
    }

    private void CleanUpDices()
    {
        if (dices.Count > 0)
        {
            for (int i = dices.Count - 1; i >= 0; i--)
            {
                Destroy(dices[i]);
            }

            dices.Clear();
        }
    }

}

public interface IParticipant
{
    void StartTurn(Combat combat);

    void EndTurn(Combat combat);

    void CombatEnded(Combat combat);

    void HealthHitZero(Combat combat);

    bool Damage(Combat combat, int damage);

    Transform GetTransform();

    Stats GetStats();

}

[Serializable]
public class Stats
{
    public StatsUI UI;
    
    public int Strength = 0;
    public int Defense = 0;
    public int MaxHealth = 0;
    private int _currentHealth;


    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = value;
            UI?.UpdateUI(_currentHealth, MaxHealth);
        }
    }
    public bool isInstakill;
}


public abstract class StatsUI : MonoBehaviour
{
    public abstract void UpdateUI(int currentHealth, int maxHealth);
}