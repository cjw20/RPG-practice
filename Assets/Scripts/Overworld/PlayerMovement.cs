using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D body;
    Vector2 movement;
    bool nearNPC = false;
    GameObject NPC;
    bool nearExit = false;
    GameObject exit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (nearNPC)
            {
                NPC.GetComponent<DialogueTrigger>().TriggerDialogue();
                
                Time.timeScale = 0f;
            }

            if (nearExit)
            {
                GameControl.control.NewArea(exit.GetComponent<AreaExit>().destinationName, exit.GetComponent<AreaExit>().exitDirection);
            }
        }
        
    }


    void FixedUpdate()
    {
        body.MovePosition(body.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "NPC")
        {
            nearNPC = true;
            NPC = other.gameObject;
        }

        if(other.tag == "Exit")
        {
            nearExit = true;
            exit = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        nearNPC = false;
        nearExit = false;
    }
}
