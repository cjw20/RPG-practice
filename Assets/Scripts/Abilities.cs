using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    public TargetManager targetManager;
    public BattleSystem battleSystem;
    public Text dialogueText;
    public GameObject descriptionWindow;
    public Text descriptionText;

    string description;
    //float damageMultiplier; //damage scaling for attacks
    int damageScaler; //for simple scaling like x2

    public void UseAbility(Unit character, string ability)
    {
        switch (ability)
        {
            case "Critical Strike":
                StartCoroutine(CriticalStrike(character));
                break;
            case "Wave Attack":
                StartCoroutine(WaveAttack(character));
                break;
            
            default:
                Debug.Log("Ability not found");
                break;
        }


    }

    void SetDescriptionWindow(string description)
    {
        descriptionWindow.SetActive(true);
        descriptionText.text = description; 
    }

    IEnumerator CriticalStrike(Unit character)
    {
        description = "Hit an enemy for critical damage!";
        damageScaler = 2;
        targetManager.TargetSetup(battleSystem.enemyStats);
        SetDescriptionWindow(description);

        while (targetManager.window.activeInHierarchy)
        {
            yield return new WaitForEndOfFrame(); //waits for target to be selected
        }
        descriptionWindow.SetActive(false);
        if (battleSystem.currentTarget == null)
            yield break;  //backs out if return button is pressed

        bool isDead =  battleSystem.currentTarget.TakeDamage(character.damage * damageScaler);
        if (isDead)
        {
            battleSystem.UpdateEnemy(battleSystem.currentTarget);
            battleSystem.currentTarget.KillCharacter();

        }

        dialogueText.text = "The strike dealt " + character.damage * damageScaler + " damage to " + battleSystem.currentTarget.unitName;
        
        yield return new WaitForSeconds(1f);

        StartCoroutine(battleSystem.NextTurn());

        yield break;
    }

    IEnumerator WaveAttack(Unit character)
    {
        yield break;
    }
}
