using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DialogController {
    [SerializeField] public GameObject dialogPanel;
    private Animator animator;
    private TextMeshProUGUI text;

    private Button button;


    public void Load() {
        animator = dialogPanel.GetComponent<Animator>();
        text = dialogPanel.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        button = dialogPanel.transform.Find("Button").gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void SetText(string message) {
        text.text = message;
    }

    private void OnClick() {
        Debug.Log("Click en el boton");
    }
}