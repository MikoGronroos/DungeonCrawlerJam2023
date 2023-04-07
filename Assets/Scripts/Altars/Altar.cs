using System;
using UnityEngine;
using UnityEngine.Events;

public class Altar : MonoBehaviour
{
    public UnityEvent OnBloodSacrificed;

    [HideInInspector] public bool hasBlood;
    
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Collider boxCollider;
    [SerializeField] private GameObject text;
    [SerializeField] private GameObject blood;
    [SerializeField] private int altarDamage;

    private void Awake()
    {
        boxCollider.enabled = true;
        blood.SetActive(false);
        text.SetActive(true);
        hasBlood = false;
    }

    public void SacrificeBlood(Player player)
    {
        if (hasBlood)
        {
            return;
        }
        
        boxCollider.enabled = false;
        blood.SetActive(true);
        text.gameObject.SetActive(false);
        player.Damage(altarDamage);
        particles.Play();
        hasBlood = true;
        OnBloodSacrificed?.Invoke();
    }
    
}
