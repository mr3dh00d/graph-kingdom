using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] public GameObject GO;
    private TextMeshProUGUI _textMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        _textMeshProUGUI = GO.GetComponent<TextMeshProUGUI>();
        Debug.Log(_textMeshProUGUI);
        if (_textMeshProUGUI == null) return;
        _textMeshProUGUI.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
