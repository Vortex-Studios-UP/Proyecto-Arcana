/* 
@Author: Christian Matos
@Date: 2023-06-21 16:27:39
@Last Modified by: Christian Matos
@Last Modified Date: 2023-06-21 16:27:39

* Functionality:
* Approach:
* To Use:
* Dependencies:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [Header("Stats")]
    public float vitality = 1;
    public float defense = 1;
    public float strength = 1;
    public float magic = 1;

    [Header("Runtime")]
    [SerializeField] private float currentVitality;
    [SerializeField] private float currentDefense;
    [SerializeField] private bool stunnable = false;
    [SerializeField] private bool stunned = false;

    private void Awake()
    {
        currentVitality = vitality;
        currentDefense = defense;
    }

    public void TakeDamage(float damage)
    {
        // Substracts damage from defense
        currentDefense -= damage;

        // If defense is broken, enemy is suceptible to stun
        if (currentDefense <= 0)
        {
            stunnable = true;

            // Excess damage is substracted from vitality
            currentVitality += currentDefense;

            // currentDefense cannot go below 0
            currentDefense = 0;
        }

        if (currentVitality <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public float GetStat(Stat stat)
    {
        switch(stat)
        {
            case Stat.Vitality:
                return vitality;
            case Stat.Defense:
                return defense;
            case Stat.Strength:
                return strength;
            case Stat.Magic:
                return magic;
            default:
                return 0;
        }
    }
}
