using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AbilityWindow : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject window; //window that contains ability buttons
    public Abilities Abilites;

    RectTransform buttonSize;
    RectTransform windowSize;

    float windowWidth, windowHeight;
    float buttonWidth, buttonHeight;
    float desiredWGap, desiredHGap;

    int maxButtonsWidth, maxButtonsHeight;

    List<GameObject> abilityButtons;
    string[] abilities;
    Unit ally;

    public void SetAbility(Unit currentAlly)
    {
        window.SetActive(true);

        List<string> abilityList = currentAlly.abilities;
        abilities = abilityList.ToArray();
        ally = currentAlly;

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

        abilityButtons = new List<GameObject>();

        for (int y = 0; y < maxButtonsWidth - 1; y++)
        {
            for (int x = 0; x < maxButtonsHeight - 1; x++)
            {
                GameObject newButton = Instantiate(buttonPrefab, window.transform, false);

                RectTransform buttonRect = newButton.GetComponent<RectTransform>();
                newButton.name = "Ability Button";
                newButton.transform.SetParent(this.transform);

                buttonRect.localPosition = new Vector3(desiredWGap * (y + 1) + (buttonWidth * y), (-desiredHGap * (x + 1) - buttonHeight * x), 0);

                buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, buttonWidth);
                buttonRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonHeight);

                abilityButtons.Add(newButton);

            }
        }

        int i = 0;
        foreach (GameObject button in abilityButtons)
        {
            if (i + 1 > abilities.Length)
            {
                Destroy(button);
            }
            else
            {
                Text abilityText = button.transform.Find("Text").GetComponent<Text>();

                abilityText.text = abilities[i]; //will need to add a line showing amount held. Maybe in hover message.

                Button btn = button.GetComponent<Button>();
                btn.onClick.AddListener(delegate { CustomOnClick(abilityText.text); });

                i++;
            }



        }
    }

    void CustomOnClick(string ID)
    {
        Abilites.UseAbility(ally, ID);
        CloseAbi();
    }

    public void CloseAbi()
    {
        foreach (GameObject button in abilityButtons)
        {
            Destroy(button);
        }
        window.SetActive(false);


    }
}

