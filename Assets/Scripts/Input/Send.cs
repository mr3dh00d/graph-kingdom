using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Send : MonoBehaviour
{
    public TextMeshProUGUI TextComponent;
    public void SendCommand()
    {
        string command = TextComponent.text;
        GameController.instance.CompileCommand(command);
    }
}
