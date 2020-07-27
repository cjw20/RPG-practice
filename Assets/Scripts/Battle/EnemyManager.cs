using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject MobA;
    public GameObject MobB; //will need to create more slots if I want encounters with more than 3 enemies
    public GameObject MobC;

    Unit StatsA;
    Unit StatsB;
    Unit StatsC;


    int mobSize;
    public GameObject[] mob;
    public Unit[] mobStats; //array containing all stats for enemies

    void Start()
    {

        CountMob();
        CreateMob();
        GetStats();

    }
    void CountMob()  //Counts number of enemies
    {
        mobSize = 0;
        if (MobA != null)
        {
            mobSize++;
        }
        if (MobB != null)
        {
            mobSize++;
        }
        if (MobC != null)
        {
            mobSize++;
        }
    }

    void CreateMob()
    {
        mob = new GameObject[mobSize];
        if (mobSize >= 1)
        {
            mob[0] = MobA;
        }
        if (mobSize >= 2)
        {
            mob[1] = MobB;
        }
        if (mobSize >= 3)
        {
            mob[2] = MobC;
        }

    }

    void GetStats()
    {
        mobStats = new Unit[mobSize];

        if (MobA != null)
        {
            StatsA = MobA.GetComponent<Unit>();
            mobStats[0] = StatsA;
        }
        if (MobB != null)
        {
            StatsB = MobB.GetComponent<Unit>();
            mobStats[1] = StatsB;
        }
        if (MobC != null)
        {
            StatsC = MobC.GetComponent<Unit>();
            mobStats[2] = StatsC;
        }

    }
   
        
   
        
    
}
