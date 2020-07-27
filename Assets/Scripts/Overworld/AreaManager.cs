using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaManager : MonoBehaviour
{
    public static AreaManager manager;
    public GameObject[] enemies;
    public OWEnemyAI[] enemyAI;
    public bool[] killed; //array of which enemies to set inactive
    public int enemiesInArea; //number of enemies in specific area


    void Awake()
    {

        if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }

        killed = new bool[enemiesInArea];
    }

    public void UpdateKilled()
    {
        int i = 0;
        
        foreach (OWEnemyAI enemy in enemyAI)
        {
            if (!killed[i])
            {
                killed[i] = enemy.isKilled; //only updates killed if not already dead
            }
            

            i++;
        }

       
    }

    public void ReturnToArea()
    {        
        
        enemies = new GameObject[enemiesInArea];
        enemies = GameObject.FindGameObjectsWithTag("Enemy"); //refinds enemies in area
        
        int i = 0;
        enemyAI = new OWEnemyAI[enemiesInArea];
        foreach (GameObject enemy in enemies)
        {
            enemyAI[i] = enemy.GetComponent<OWEnemyAI>();

            i++;
        }
    }

    public void DespawnKilled()
    {
                
        for (int j = 0; j < enemies.Length; j++)
        {
            if (killed[j])
                enemies[j].SetActive(false);
        }
        
    }
   
    public void NewArea()
    {
        Destroy(gameObject);
    }
    //destroy this on move to new area
    //get chest status from gamecontrol
    
}
