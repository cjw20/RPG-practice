using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public GameObject player;
    
    public Transform[] playerSpawn;

    Vector3 emptyV; //vector of 0,0,0. Checks if gamecontrol's position vector has been set yet

    public int spawnPoint; //which spawn point to use (for multiple entrances) maybe make Enum north south east west enterances
    // Start is called before the first frame update
    void Start()
    {
        AreaManager.manager.ReturnToArea();
        AreaManager.manager.DespawnKilled();

        if(GameControl.control.playerPos == emptyV)
        {
            switch (GameControl.control.travelDirection) //0 equals from north enterance, 1 south, 2 east, 3 west,
            {
                case TravelDirection.NORTH:
                    player.transform.position = playerSpawn[0].transform.position;
                    break;
                case TravelDirection.SOUTH:
                    player.transform.position = playerSpawn[1].transform.position;
                    break;
                case TravelDirection.EAST:
                    player.transform.position = playerSpawn[2].transform.position;
                    break;
                case TravelDirection.WEST:
                    player.transform.position = playerSpawn[3].transform.position;
                    break;

            }
            
            //set player position to spawn point
        }
        else
        {
            player.transform.position = GameControl.control.playerPos;
            //set invulnerable for a few seconds so that next enemy doesnt hop right on
            //set player position to playerPos
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
