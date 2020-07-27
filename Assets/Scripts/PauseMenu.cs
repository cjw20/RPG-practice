using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseWindow;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if(GameControl.control.gameState == GameState.OVERWORLD)
            {
                GameControl.control.gameState = GameState.MENU;
                pauseWindow.SetActive(true);
                Time.timeScale = 0f;
            }

            
        }
    }

    public void Unpause()
    {
        if (GameControl.control.gameState == GameState.MENU)
        {
            GameControl.control.gameState = GameState.OVERWORLD; //this wont work with timescale 0
            Time.timeScale = 1f;
        }
    }
}
