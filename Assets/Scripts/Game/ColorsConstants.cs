using UnityEngine;

public class ColorsConstants {
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