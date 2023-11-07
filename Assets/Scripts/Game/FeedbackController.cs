using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class FeedBackController
{
    [SerializeField] public GameObject panel;
    [SerializeField] public Sprite important;
    [SerializeField] public Sprite check;

    private Image background;
    private Image icon;
    private TextMeshProUGUI text;
    private float fadeInAndOutDuration = 1f;
    private float visibleDuration = 5f;

    public void Load()
    {
        background = panel.GetComponent<Image>();
        icon = panel.transform.Find("icon").gameObject.GetComponent<Image>();
        text = panel.transform.Find("text").gameObject.GetComponent<TextMeshProUGUI>();
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0f);
    }

    public void SetGoodMessage(string message)
    {
        background.color = ColorsConstants.HexToColor(ColorsConstants.GREEN_BG);
        icon.sprite = check;
        text.color = ColorsConstants.HexToColor(ColorsConstants.GREEN_TEXT);
        text.text = message;
        GameController.instance.StartCoroutine(MostrarPanelConFade());
    }

    public void SetBadMessage(string message)
    {
        background.color = ColorsConstants.HexToColor(ColorsConstants.RED_BG);
        icon.sprite = important;
        text.color = ColorsConstants.HexToColor(ColorsConstants.RED_TEXT);
        text.text = message;
        GameController.instance.StartCoroutine(MostrarPanelConFade());
    }

    IEnumerator MostrarPanelConFade()
    {
        // Fade In
        float tiempoInicio = Time.time;
        while (Time.time - tiempoInicio < fadeInAndOutDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, (Time.time - tiempoInicio) / fadeInAndOutDuration);
            background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
            yield return null;
        }
        
        // Panel visible
        yield return new WaitForSeconds(visibleDuration);
        
        // Fade Out
        tiempoInicio = Time.time;
        while (Time.time - tiempoInicio < fadeInAndOutDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, (Time.time - tiempoInicio) / fadeInAndOutDuration);
            background.color = new Color(background.color.r, background.color.g, background.color.b, alpha);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
            yield return null;
        }
    }

}