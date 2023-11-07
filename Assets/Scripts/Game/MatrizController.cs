using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MatrizController
{
    [SerializeField] public GameObject matriz;
    private GameObject cabecera;
    private GameObject contenido;

    public void Load() {
        LoadObjects();
        LoadCitiesNames();
        SetStartCity();
    }

    public void SetStartCity() {
        City city = GameController.instance.getLocation();
        GameObject fila = contenido.transform.Find($"Fila {city.getId()}").gameObject;
        TextMeshProUGUI label = fila.transform.Find("Ciudad").gameObject.GetComponent<TextMeshProUGUI>();
        label.color = ColorsConstants.HexToColor("#FFA500");
        TextMeshProUGUI distancia = fila.transform.Find("Distancia").gameObject.GetComponent<TextMeshProUGUI>();
        distancia.text = "0";
    }

    private void LoadObjects() {
        cabecera = matriz.transform.Find("Cabecera").gameObject;
        contenido = matriz.transform.Find("Contenido").gameObject;
    }
    
    private void LoadCitiesNames() {
        int i = 1;
        foreach (City city in GameController.instance.cities.GetCities())
        {
            GameObject fila = contenido.transform.Find($"Fila {i}").gameObject;
            TextMeshProUGUI label = fila.transform.Find("Ciudad").gameObject.GetComponent<TextMeshProUGUI>();
            label.text = city.getName();
            i++;
        }
    }

}