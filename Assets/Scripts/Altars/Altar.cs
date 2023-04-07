using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] private int altarDamage;

    public void SacrificeBlood(Player player)
    {
        player.Damage(altarDamage);
    }
    
}
