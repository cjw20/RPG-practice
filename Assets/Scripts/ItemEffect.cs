using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEffect : MonoBehaviour
{
    public TargetManager targetManager;
    public BattleSystem battleSystem;
    public Text dialogueText;
    public GameObject descriptionWindow;
    public Text descriptionText;
    public Inventory inventory;
    public GameObject confirmWindow;
    bool confirmed;
    
    public void ItemUse(string ID)
    {
        switch (ID)
        {
            case "Potion":
                StartCoroutine(Potion());
                break;
            case "Bomb":
                StartCoroutine(Bomb());
                break;
            case "Rez Stone":
                StartCoroutine(Rez_Stone());
                break;
            default:
                Debug.Log("Item not found");
                break;
        }

       
    }
    
    void SetDescriptionWindow(string description)
    {
        descriptionWindow.SetActive(true);
        descriptionText.text = description; //add number later
    }

    public void SetConfirmWindow()
    {
        confirmWindow.SetActive(true);
    }

    public void CloseConfirmWindow(bool selection)
    {
        confirmed = selection;
        confirmWindow.SetActive(false);
    }

    IEnumerator Potion()
    {
        string itemDescription = "Restores 50 HP to target ally.";
        int healing = 50;
        //numHeld = Inventory.itemsHeld["Potion"];
        targetManager.TargetSetup(battleSystem.partyStats);

        SetDescriptionWindow(itemDescription);


        while (targetManager.window.activeInHierarchy)
        {
            yield return new WaitForEndOfFrame(); //waits for target to be selected
        }
        descriptionWindow.SetActive(false);
        if (battleSystem.currentTarget == null)
            yield break;  //backs out if return button is pressed

        battleSystem.currentTarget.RestoreHP(healing);
        if (battleSystem.currentTarget.alive)
        {
            dialogueText.text = "The potion restored " + healing + "HP to " + battleSystem.currentTarget.unitName;
        }
        else
        {
            dialogueText.text = "Nothing happened...";
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(battleSystem.NextTurn());
        
        
        
        
    }

    IEnumerator Bomb()
    {
        string itemDescription = "Deals 30 damage to all enemies.";
        int damage = 30;
        SetDescriptionWindow(itemDescription);
        SetConfirmWindow();
        while (confirmWindow.activeInHierarchy)
        {
            yield return new WaitForEndOfFrame(); //waits for confirmation
        }
        if (!confirmed)
            yield break;
        foreach(Unit enemy in battleSystem.enemyStats)
        {
            bool isDead = enemy.TakeDamage(damage);
            if (isDead)
            {
                battleSystem.UpdateEnemy(enemy);
                enemy.KillCharacter();
                
            }
                

        }
        descriptionWindow.SetActive(false);
        dialogueText.text = "Each enemy was dealt " + damage + " damage";
        yield return new WaitForSeconds(1f);
        StartCoroutine(battleSystem.NextTurn());
    }

    IEnumerator Rez_Stone()
    {
        string itemDescription = "Revives a KO'd party member.";
        SetDescriptionWindow(itemDescription);

        targetManager.TargetSetup(battleSystem.partyStats);

        while (targetManager.window.activeInHierarchy)
        {
            yield return new WaitForEndOfFrame(); //waits for target to be selected
        }

        descriptionWindow.SetActive(false); 

        if (battleSystem.currentTarget == null)
            StopCoroutine(Potion());  //backs out if return button is pressed

        if (battleSystem.currentTarget.alive)
        {
            dialogueText.text = "Nothing Happened...";
        }
        else
        {
            battleSystem.currentTarget.RezCharacter();
            dialogueText.text = battleSystem.currentTarget.unitName + " was revived!";
        }

        yield return new WaitForSeconds(1f);
        StartCoroutine(battleSystem.NextTurn());
    }
}
