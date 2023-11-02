using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class ColorsConstants {
    public readonly static string RED_BG = "#ffd5ce";
    public readonly static string GREEN_BG = "#daffce";
    public readonly static string RED_TEXT = "#ea4242";
    public readonly static string GREEN_TEXT = "#5d8d0a";

    public static Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
        {
            return color;
        }
        else
        {
            Debug.LogError("No se pudo convertir el valor hexadecimal a Color.");
            return Color.white;
        }
    }

}
[Serializable] public class FeedBackController {

    [SerializeField] public GameObject panel;
    [SerializeField] public Sprite important;
    [SerializeField] public Sprite check;

    private Image background;
    private Image icon;
    private TextMeshProUGUI text;
    private float fadeInAndOutDuration = 1f;
    private float visibleDuration = 5f;

    public void LoadObjects()
    {
        background = panel.GetComponent<Image>();
        icon = panel.transform.Find("icon").gameObject.GetComponent<Image>();
        text = panel.transform.Find("text").gameObject.GetComponent<TextMeshProUGUI>();
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0f);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 0f);
    }

    public void setGoodMessage(string message)
    {
        background.color = ColorsConstants.HexToColor(ColorsConstants.GREEN_BG);
        icon.sprite = check;
        text.color = ColorsConstants.HexToColor(ColorsConstants.GREEN_TEXT);
        text.text = message;
        GameController.instance.StartCoroutine(MostrarPanelConFade());
    }

    public void setBadMessage(string message)
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