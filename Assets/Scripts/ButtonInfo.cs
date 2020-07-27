using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInfo : MonoBehaviour
{
    public int amount; // amount to show for items
    public string buttonText; //Text to be shown on button

    public void SetAmount(int num)
    {
        amount = num;
    }

    public void setText(string text)
    {
        buttonText = text;
    }
}
