using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Unit : MonoBehaviour
{
     

    public string unitName;
    public int maxHP;
    public int currentHP;
    public int damage;
    public string faction; //player or enemy
    public bool alive; //false if character has been knocked out
    public CharacterHUD HUD;

    public Renderer body;
    public Renderer ghost;

    public List<string> abilities; //list of abilities unit has access to

    public bool TakeDamage(int power)
    {
      
        currentHP -= power;
        UpdateHUD();
        if(body == null)
        {

        }
        if (currentHP <= 0)
        {
            return true;  //tells battle system that character is dead
           
        }
        else
            return false; //alive
    }

    public void RestoreHP(int amount)
    {
        if (alive)
        {
            currentHP += amount;
            if (currentHP > maxHP)
                currentHP = maxHP;
            UpdateHUD();
        }
        
    }
   

    public void UpdateHUD()
    {
        if (faction == "enemy")
            return;
        HUD.SetHP(currentHP);
    }
    public void KillCharacter()
    {
        alive = false;
        if(faction == "player")
        {
                
            body.enabled = false;
            ghost.enabled = true;
        }
        else
        {
            
            Destroy(this.gameObject);
        }
        
        //add drops to end of battle rewards
        
    }

    public void RezCharacter()
    {
        alive = true;
        ghost.enabled = false;
        body.enabled = true;
        currentHP = maxHP;
        UpdateHUD();
        //set ghost inactive
    }

    
}
