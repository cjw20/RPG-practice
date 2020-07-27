using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public enum TravelDirection { NORTH, SOUTH, EAST, WEST };  //0 equals from north enterance, 1 south, 2 east, 3 west,
public enum GameState { BATTLE, OVERWORLD, CUTSCENE, MENU};

public class GameControl : MonoBehaviour
{
    public static GameControl control;
    public List<GameObject> party;
    public List<Unit> battlePartyStats;
    public List<GameObject> battleParty;
    public List<GameObject> encounteredEnemy;
    public List<string> itemNames;
    public List<int> itemNum;
    public Dictionary<string, int> inventory;

    public Vector3 playerPos; //positon of player before battle
    string sceneName; //name of scene in before battle
    GameObject player;

    public TravelDirection travelDirection;
    public GameState gameState;

    void Awake()
    {
        if(control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if(control != this)
        {
            Destroy(gameObject);
        }

        if(inventory == null)
        {
            int i = 0;
            inventory = new Dictionary<string, int>();
            foreach (string itemName in itemNames)
            {
                inventory.Add(itemName, itemNum[i]);
                i++;
            }
        }
        

        //sets up party units

        foreach(GameObject ally in battleParty) //will make distinction between battleparty and party if more members in party. Should stick with just 3 char for first project
        {
            Unit stats = ally.GetComponent<Unit>();
            battlePartyStats.Add(stats);
        }
        
    }

    public void SetEncounter(List<GameObject> encounter)
    {
        player = GameObject.FindWithTag("Player");
        playerPos = player.transform.position;

        Scene scene = SceneManager.GetActiveScene();
        sceneName = scene.name; //gets scene name to return to after battle


        encounteredEnemy = encounter;
        gameState = GameState.BATTLE;
        SceneManager.LoadScene("BattleScene");
    }

    public void AfterBattle()
    {
        gameState = GameState.OVERWORLD;
        SceneManager.LoadScene(sceneName);
        
        
        //set player position
        //tell area manager to set killed enemy inactive
    }

    public void NewArea(string nameScene, TravelDirection direction)
    {
        travelDirection = direction;

        AreaManager.manager.NewArea(); //destroys old area manager
        playerPos = new Vector3();

        SceneManager.LoadScene(nameScene);
        
    }

    public void UpdateUnit(Unit[] updatedStats)
    {
        for(int i = 0; i < updatedStats.Length; i++)
        {
            battlePartyStats[i].currentHP = updatedStats[i].currentHP;
            //add other updated stats such as mp, xp, level
            //call separate level up script if level up
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

        PlayerData data = new PlayerData();
        data.party = party;
        data.battleParty = battleParty;
        data.inventory = inventory;

        //may need to unload dictionary into lists if not serializable

        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            party = data.party;
            battleParty = data.battleParty;
            inventory = data.inventory;
        }
    }
}

[Serializable]
class PlayerData
{
    public List<GameObject> party;
    public List<GameObject> battleParty;
    public Dictionary<string, int> inventory;
}