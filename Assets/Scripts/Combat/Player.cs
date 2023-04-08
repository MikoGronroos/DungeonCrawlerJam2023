using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IParticipant
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Image deathUIImage;
    [SerializeField] private TextMeshProUGUI deathTextUGUI;
    [SerializeField] private Stats currentStats;

    [SerializeField] private GameObject turnPanel;
    [SerializeField] private Button attackButton;

    private bool _dead;
    private bool _canClickAttack;

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
        currentStats.CurrentHealth = 15;
    }

    [ContextMenu("Kill player")]
    private void Damage()
    {
        Damage(currentStats.MaxHealth);
    }

    public bool Damage(Combat combat, int damage)
    {
        currentStats.CurrentHealth = Mathf.Clamp(currentStats.CurrentHealth - damage, 0, currentStats.MaxHealth);

        if (currentStats.CurrentHealth <= 0)
        {
            HealthHitZero();
            _dead = true;
        }
        return _dead;
    }

    public bool Damage(int damage)
    {
        currentStats.CurrentHealth = Mathf.Clamp(currentStats.CurrentHealth - damage, 0, currentStats.MaxHealth);

        if (currentStats.CurrentHealth <= 0)
        {
            HealthHitZero();
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
    
    public void HealthHitZero()
    {
        KillPlayer();
    }

    private void KillPlayer()
    {
        Debug.Log("Kill player");
        StartCoroutine(KillPlayerSequence());

        IEnumerator KillPlayerSequence()
        {
            GetComponent<PlayerMovement>().CanMove = false;

            yield return new WaitForSeconds(0.2f);
            deathUIImage.gameObject.SetActive(true);
            deathUIImage.DOFillAmount(1, 0.2f);
            deathTextUGUI.DOFade(1, 0.2f).SetDelay(0.1f);
            yield return new WaitForSeconds(3f);
            deathUIImage.fillOrigin = 0;
            deathUIImage.DOFillAmount(0, 0.2f);
            yield return new WaitForSeconds(0.2f);
            gameManager.LoseGame();
        }
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
        combat.Attack((bool state) =>
        {
            AttackFinished(state, combat);
        });
        
        // turnPanel.SetActive(true);
        // _canClickAttack = true;
        // attackButton.onClick.AddListener(() =>
        // {
        //     if (_canClickAttack)
        //     {
        //         combat.Attack((bool state) =>
        //         {
        //             AttackFinished(state, combat);
        //         });
        //     }
        //     _canClickAttack = false;
        // });
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

    public void TryToOpenDoor()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, 1f))
        {
            if (raycastHit.collider.gameObject.GetComponent<Door>())
            {
                raycastHit.collider.gameObject.GetComponent<Door>().Interact();
            }
        }
    }
    
    public Transform GetTransform()
    {
        return transform;
    }

}
