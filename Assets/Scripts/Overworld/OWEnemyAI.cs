using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OWEnemyAI : MonoBehaviour
{
    public Transform target;
    public GameObject player;
    Transform playerPos;
    public float speed = 5f;
    public float minDistance = 0.5f;
    public Transform[] waypoints;
    public int destination = 0;
    public float detectionDistance;
    public List<GameObject> encounter;

    public bool isKilled = false; //tells game control to kill enemy if beated in this instance of area already

    NavMeshAgent2D agent;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent2D>();

        
        player = GameObject.FindWithTag("Player");
        playerPos = player.GetComponent<Transform>();
        

        GoToNextPoint();
    }

    // Update is called once per frame
    void Update()
    {
        
        
        if (!agent.pathPending && agent.remainingDistance < minDistance)
            GoToNextPoint();
        
        if(Vector3.Distance(transform.position, playerPos.position) < detectionDistance)
        {
            
            agent.destination = playerPos.transform.position;
        }
    }

    void GoToNextPoint()
    {
        if (agent.destination == new Vector2(playerPos.position.x, playerPos.position.y))
            return;

        agent.destination = waypoints[destination].position;

        destination = (destination + 1) % waypoints.Length;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        //Time.timeScale = 0f;
        isKilled = true; 
        AreaManager.manager.UpdateKilled();
        GameControl.control.SetEncounter(encounter);
    }
}
