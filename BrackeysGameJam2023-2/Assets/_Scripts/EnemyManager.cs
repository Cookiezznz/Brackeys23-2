using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float maxEnemyAmount;
    public float enemiesCloseToPlayer;

    

    // Update is called once per frame
    void Update()
    {
        if(enemiesCloseToPlayer > maxEnemyAmount)
        {
            //end game
        }
    }

    public void AddToAttack()
    {
        enemiesCloseToPlayer++;
    }

    public void RemoveAttack()
    {
        enemiesCloseToPlayer--;
    }

}
