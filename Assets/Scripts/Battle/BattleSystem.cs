using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;


public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, WAIT }
public enum ActionState { ATTACK, ITEM, ABILITY }

public class BattleSystem : MonoBehaviour
{
    public BattleState state;
    public ActionState curAction;

    public Transform[] playerSpawn; //spawn points for characters on battle screen
    public Transform[] enemySpawn;

    public Inventory inventory;
    public InvWindow invWindow;


    List<GameObject> partyInfo; //contains prefabs of characters. DON'T EDIT THESE!
    GameObject[] partyMembers; //characters chosen for battle
    public Unit[] partyStats; //contains unit scripts for instantiated party members
    GameObject[] enemyInfo; //contains prefabs of enemies. DO NOT EDIT!
    GameObject[] enemies;
    public Unit[] enemyStats; //contains unit scripts for intantiated enemies

    
    

    public Text dialogueText;

    public CharacterHUD[] AllyHUD;//script that interacts with individual hud elements

    public GameObject[] FullHUD; // array of full huds for purpose of setting inactive

    public AbilityWindow AbilityWindow;
    public TargetManager targetManager;
    public Unit currentTarget;
    Unit controlledMember; //the party member currently being used
    int currentEffect; //damage value of current action
    int actionCount; //keeps track of which party members have acted
    bool endCheck; //checks if battle should end (all of one side are dead)
    bool lost;
    bool won;

    List<Unit> aliveParty;
    List<Unit> tempEnemy; //for removing dead enemies

    void Start()
    {
        state = BattleState.START;        
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {

        partyInfo = GameControl.control.battleParty;
        partyMembers = partyInfo.ToArray();
        partyStats = GameControl.control.battlePartyStats.ToArray();

        int i = 0;
        foreach (GameObject ally in partyMembers)
        {
            
           GameObject newAlly = Instantiate(ally, playerSpawn[i].transform.position, playerSpawn[i].transform.rotation);
           partyStats[i] = newAlly.GetComponent<Unit>(); 
            
           i++;
            
        }

        SetupHUD();

        enemyInfo = GameControl.control.encounteredEnemy.ToArray();
        enemies = new GameObject[enemyInfo.Length];
        enemyStats = new Unit[enemyInfo.Length];
     
        int j = 0;
        foreach (GameObject enemy in enemyInfo)
        {
            
            enemies[j] = Instantiate(enemy, enemySpawn[j].transform.position, enemySpawn[j].transform.rotation);
            enemyStats[j] = enemies[j].GetComponent<Unit>();
            j++;
           
        }

        dialogueText.text = "Enemies approach!!";

        yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTURN;
        actionCount = 0;
        PlayerTurn();
    }


    void SetupHUD()
    {
        
        //Set up Character names and health bars. Consider making foreach loop
        if (partyMembers.Length >= 1)
        {
            AllyHUD[0].SetHUD(partyStats[0]);
            partyStats[0].HUD = AllyHUD[0];
        }
        else
        {
            FullHUD[0].SetActive(false);
        }

        if (partyMembers.Length >= 2)
        {
            AllyHUD[1].SetHUD(partyStats[1]);
            partyStats[1].HUD = AllyHUD[1];
        }
        else
        {
            FullHUD[1].SetActive(false);
        }
        if (partyMembers.Length >= 3)
        {
            AllyHUD[2].SetHUD(partyStats[2]);
            partyStats[2].HUD = AllyHUD[2];
        }
        else
        {
            FullHUD[2].SetActive(false);
        }
    }

    public void UpdateEnemy(Unit deadEnemy)
    {
        tempEnemy = enemyStats.ToList<Unit>();
        tempEnemy.Remove(deadEnemy);
        enemyStats = tempEnemy.ToArray();
    }
    public void CheckIfOver()
    {
        lost = true;  //check if battle is over
        won = false;

        foreach (Unit ally in partyStats)
        {
            if (ally.alive)
                lost = false;
        }
        
        if(enemyStats.Length == 0)
        {
            won = true;
        }
       
        if (won | lost)
        {
            if (won)
            {
                dialogueText.text = "You win!";
                state = BattleState.WON;
                StopAllCoroutines();
                StartCoroutine(Victory());
                //trigger results screne here. add xp and loot
            }
            if (lost)
            {
                state = BattleState.LOST;
                dialogueText.text = "You lose!";
                //trigger game over state here
                StopAllCoroutines();
            }
        }
    }


    IEnumerator Victory()
    {
        //xp gain / level up
        //set hp to 1 for dead allies
        foreach(Unit ally in partyStats)
        {
            if (!ally.alive)
            {
                ally.alive = true;
                ally.currentHP = 1;
            }
        }
        GameControl.control.UpdateUnit(partyStats);
        //money and item drops
        GameControl.control.inventory = inventory.itemsHeld;

        GameControl.control.AfterBattle();
        //change scene
        yield break;
    }
    void PlayerTurn()
    {

        state = BattleState.PLAYERTURN;
        controlledMember = partyStats[actionCount];
        currentTarget = null;  //resets current target
        if (controlledMember.alive)
        {
            dialogueText.text = "It is " + controlledMember.unitName + "'s turn";
        }
        else
        {
            StartCoroutine(NextTurn());
        }
    }

    public IEnumerator NextTurn()
    {
        yield return new WaitForSeconds(1f);

        actionCount++;

        CheckIfOver();
        if (won | lost)
        {
            if (won)
            {
                dialogueText.text = "You win!";
                state = BattleState.WON;
                StopAllCoroutines();
                yield break;
                //trigger results screne here. add xp and loot
            }
            if (lost)
            {
                state = BattleState.LOST;
                dialogueText.text = "You lose!";
                //trigger game over state here
                StopAllCoroutines();
                yield break;
            }
        }

        if (actionCount < partyMembers.Length)
        {
            PlayerTurn();
        }
        else
        {
            actionCount = 0;
            StartCoroutine(EnemyTurn());
        }
    }
    
    IEnumerator EnemyTurn()
    {
        state = BattleState.ENEMYTURN;
        foreach(Unit enemy in enemyStats)
        {
            if (enemy.alive)
            {
                aliveParty = new List<Unit>();
                foreach (Unit ally in partyStats)
                {
                    if (ally.alive)
                        aliveParty.Add(ally);
                }
                Unit[] targetableAlly = aliveParty.ToArray();

                int random = Random.Range(0, targetableAlly.Length); //chooses random ally to attack. can change ai per monster later with script
                currentTarget = targetableAlly[random]; // CHange ai later per monster
                bool isDead = currentTarget.TakeDamage(enemy.damage);
                dialogueText.text = enemy.unitName + " attacked " + targetableAlly[random].unitName + " dealing " + enemy.damage + " damage.";

                if (isDead)
                {
                    targetableAlly[random].KillCharacter();
                    yield return new WaitForSeconds(1.5f);
                    dialogueText.text = targetableAlly[random].unitName + " was knocked out.";
                    
                    CheckIfOver();
                }
                yield return new WaitForSeconds(1.5f);
            }
           

        }
        PlayerTurn();
    }


    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        curAction = ActionState.ATTACK;
        targetManager.TargetSetup(enemyStats);        
    }
   
    public void OnTargetButton(Unit target)
    {

        state = BattleState.WAIT; //prevents player from getting in a second attack before the turn switches
        currentTarget = target; //sets current target for use in other functions

        switch (curAction)
        {
            case ActionState.ATTACK:
                Attack();
                break;
            case ActionState.ITEM:
                break;
            case ActionState.ABILITY:
                break;
        }
          
    }

    public void OnItemButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        curAction = ActionState.ITEM;
        invWindow.SetInvUI();

    }

    public void OnAbilityButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        curAction = ActionState.ABILITY;
        AbilityWindow.SetAbility(controlledMember);
    }

    public void OnRunButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        //dialogueText.text = "Can't Escape!";  //add escape logic to full game
        //state = BattleState.WAIT;
        //StartCoroutine(NextTurn());

        StartCoroutine(Victory());
    }

    

    void Attack()
    {
        bool isDead = currentTarget.TakeDamage(controlledMember.damage);
        
        if (isDead)
        {
            dialogueText.text = "The attack dealt " + controlledMember.damage + " damage and defeated the enemy.";
            UpdateEnemy(currentTarget);
            currentTarget.KillCharacter();
            
            StartCoroutine(NextTurn());
        }
        else
        {
            dialogueText.text = "The attack dealt " + controlledMember.damage + " damage.";
            StartCoroutine(NextTurn());
        }
    }

}


