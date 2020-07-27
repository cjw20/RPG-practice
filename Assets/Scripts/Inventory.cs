using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, int> itemsHeld;  //name, number held, in other scriptdescription show on hover
    
    public ItemEffect itemEffect; //script that actually uses item
    
    void Start()
    {
        GetInventory();
    }

    void GetInventory()
    {
        itemsHeld = new Dictionary<string, int>();
        itemsHeld = GameControl.control.inventory;
        
    }

    public void useItem(string ID)
    {
        itemEffect.ItemUse(ID);
        itemsHeld[ID] -= 1;
        if(itemsHeld[ID] < 1)
        {
            itemsHeld.Remove(ID);
        }
        //call item specific function 
        //save changes to send to overworld inventory in results (will contain hp changes, and xp gains etc)
    }

    public void addItem(string ID, int amount)
    {
        try
        {
            itemsHeld.Add(ID, amount);
        }
        catch 
        {
            itemsHeld[ID] += amount;  //Consider max amount? Inventory space?
        }
    }

    
}
