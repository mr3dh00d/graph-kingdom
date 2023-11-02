using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Send : MonoBehaviour
{
    public TextMeshProUGUI TextComponent;
    public void SendInput()
    {
        string input = TextComponent.text;
        GameController.instance.CompileInput(input);
    }
}
