using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypingListener : MonoBehaviour
{
    // Start is called before the first frame update
    public TMP_InputField ChatInputField;

    void Start()
    {
        ChatInputField = GetComponent<TMP_InputField>();
        if (ChatInputField != null)
        {
            ChatInputField.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
            ChatInputField.onEndEdit.AddListener(delegate { EndEdit(); });
        }
        
    }

    public void ValueChangeCheck()
    {
        GameController.instance.isTyping = true;
    }

    public void EndEdit()
    {
        GameController.instance.isTyping = false;
    }
}
