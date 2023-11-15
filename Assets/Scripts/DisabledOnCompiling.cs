using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisabledOnCompiling : MonoBehaviour
{
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();

    }

    // Update is called once per frame
    void Update()
    {
        bool isCompiling = GameController.instance.isCompiling;
        if(button != null) button.interactable = !isCompiling;
        
    }
}
