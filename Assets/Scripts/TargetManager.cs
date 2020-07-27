using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TargetManager : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject window; //window that contains tarhet buttons
    public BattleSystem battleSystem;



    public Inventory inventory;
    
    RectTransform buttonSize;
    RectTransform windowSize;

    float windowWidth, windowHeight;
    float buttonWidth, buttonHeight;
    float desiredWGap, desiredHGap;

    int maxButtonsWidth, maxButtonsHeight;

    List<GameObject> targetButtons;
    List<Unit> aliveTargets;
    Unit[] availableTargets;

    public void TargetSetup(Unit[] targets)
    {
        window.SetActive(true);
        
                
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

        targetButtons = new List<GameObject>();

        for (int y = 0; y < maxButtonsWidth - 1; y++)
        {
            for (int x = 0; x < maxButtonsHeight - 1; x++)
            {
                GameObject newButton = Instantiate(buttonPrefab, window.transform, false);

                RectTransform buttonRect = newButton.GetComponent<RectTransform>();
                newButton.name = "Target Button";
                newButton.transform.SetParent(this.transform);

                buttonRect.localPosition = new Vector3(desiredWGap * (y + 1) + (buttonWidth * y), (-desiredHGap * (x + 1) - buttonHeight * x), 0);

                buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, buttonWidth);
                buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonHeight);

                targetButtons.Add(newButton);

            }
        }

        
        

        int i = 0;
        
        foreach (GameObject button in targetButtons)
        {
           

            if (i + 1 > targets.Length)
            {
                Destroy(button);
            }
            else
            {

                while(targets[i].faction == "enemy" && !targets[i].alive)
                    i++; //skips over all dead enemies

                Text targetText = button.transform.Find("Text").GetComponent<Text>();
                targetText.text = targets[i].unitName;
                Unit btnTarget = targets[i];
                Button btn = button.GetComponent<Button>();
                btn.onClick.AddListener(delegate { CustomOnClick(btnTarget); });

                i++;
               
            }



        }
    }

    void CustomOnClick(Unit Target)
    {
        
        CloseTarget();
        battleSystem.OnTargetButton(Target);
    }

    public void CloseTarget()
    {
        foreach (GameObject button in targetButtons)
        {
            Destroy(button);
        }
        
        window.SetActive(false);


    }
    
}
    

