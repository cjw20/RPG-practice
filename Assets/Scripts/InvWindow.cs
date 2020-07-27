using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InvWindow : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject window; //window that contains inventory buttons

    

    public Inventory inventory;
    Dictionary<string, int> inINV;

    RectTransform buttonSize;
    RectTransform windowSize;

    float windowWidth, windowHeight;
    float buttonWidth, buttonHeight;
    float desiredWGap, desiredHGap;

    int maxButtonsWidth, maxButtonsHeight;

    List<GameObject> itemButtons;

    
    public void SetInvUI()
    {
        window.SetActive(true);

        inINV = inventory.itemsHeld;
        string[] itemNames = inINV.Keys.ToArray();  //makes array with only items' names
        int[] itemAmounts = inINV.Values.ToArray(); //array with corresponding amount of each item

        buttonSize = (RectTransform)buttonPrefab.transform;   //may need to initialize this somewhere else?? if load to much
        buttonWidth = buttonSize.rect.width;
        buttonHeight = buttonSize.rect.height;

        windowSize = (RectTransform)window.transform;
        windowWidth = windowSize.rect.width;
        windowHeight = windowSize.rect.height;

        desiredWGap = 30f;
        desiredHGap = 30f;

        maxButtonsWidth = (int)(windowWidth / (desiredWGap + buttonWidth));
        maxButtonsHeight = (int)(windowHeight / (desiredHGap + buttonHeight));
        
        itemButtons = new List<GameObject>();

        for (int y = 0; y < maxButtonsWidth - 1; y++)
        {
            for (int x = 0; x < maxButtonsHeight - 1; x++)
            {
                GameObject newButton = Instantiate(buttonPrefab, window.transform, false);

                RectTransform buttonRect = newButton.GetComponent<RectTransform>();
                newButton.name = "Item Button";
                newButton.transform.SetParent(this.transform);

                buttonRect.localPosition =  new Vector3(desiredWGap * (y + 1) + (buttonWidth * y), (-desiredHGap * (x + 1) - buttonHeight * x), 0);

                buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, buttonWidth);
                buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonHeight);

                itemButtons.Add(newButton);

            }
        }

        int i = 0;
        foreach(GameObject button in itemButtons)
        {
            if(i + 1 > itemNames.Length)
            {
                Destroy(button);
            }
            else
            {
                Text itemText = button.transform.Find("Text").GetComponent<Text>();
                
                itemText.text = itemNames[i]; //will need to add a line showing amount held. Maybe in hover message.

                Button btn = button.GetComponent<Button>();
                btn.onClick.AddListener(delegate { CustomOnClick(itemText.text); });

                i++;
            }
            

          
        }
    }

    void CustomOnClick(string ID)
    {
        inventory.useItem(ID);
        CloseInv();
    }

    public void CloseInv()
    {
        foreach(GameObject button in itemButtons)
        {
            Destroy(button);
        }
        window.SetActive(false);
        
        
    }
}
