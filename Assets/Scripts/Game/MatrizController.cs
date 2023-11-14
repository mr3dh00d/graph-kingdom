using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class DistanceInfo {
    public int distance;
    public string previousCity;
    public DistanceInfo(int distance, string previousCity) {
        this.distance = distance;
        this.previousCity = previousCity;
    }

}

[Serializable]
public class MatrizController
{
    [SerializeField] public GameObject matrizPanel;
    private GameObject cabecera;
    private GameObject contenido;
    private DistanceInfo [] matriz;

    public void Load() {
        LoadObjects();
        LoadCitiesNames();
        SetStartMatriz();
        SetStartCity();
    }

    public void SetStartMatriz() {
        matriz = new DistanceInfo [GameController.instance.cities.GetCities().Length];
        for (int i = 0; i < matriz.Length; i++)
        {
            if (i == GameController.instance.getInitialCity().getId() - 1) {
                matriz[i] = new DistanceInfo(0, "");
            } else {
                matriz[i] = new DistanceInfo(int.MaxValue, "");
            }
        }
    }

    public void SetStartCity() {
        City city = GameController.instance.getLocation();
        GameObject fila = contenido.transform.Find($"Fila {city.getId()}").gameObject;
        TextMeshProUGUI label = fila.transform.Find("Ciudad").gameObject.GetComponent<TextMeshProUGUI>();
        label.color = ColorsConstants.HexToColor("#FFA500");
        TextMeshProUGUI distancia = fila.transform.Find("Distancia").gameObject.GetComponent<TextMeshProUGUI>();
        distancia.text = $"{matriz[city.getId() - 1].distance}";
    }

    private void LoadObjects() {
        cabecera = matrizPanel.transform.Find("Cabecera").gameObject;
        contenido = matrizPanel.transform.Find("Contenido").gameObject;
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

    public void SetVisited(City city) {
        GameObject fila = contenido.transform.Find($"Fila {city.getId()}").gameObject;
        TextMeshProUGUI label = fila.transform.Find("Ciudad").gameObject.GetComponent<TextMeshProUGUI>();
        if (city.getId() != GameController.instance.getInitialCity().getId()) {
            label.color = Color.yellow;
        }
    }

    public void SetDistance(City city, int distance) {
        int id = city.getId() - 1;
        int currentDistance = matriz[GameController.instance.getLocation().getId() - 1].distance;
        if (currentDistance == int.MaxValue) {
            GameController.instance.feedBackController.SetBadMessage($"No se puede calcular la distancia a {city.getName()} porque no se ha guardó la ciudad anterior");
            return;
        }
        distance = currentDistance + distance;
        if(distance < matriz[id].distance) {
            matriz[id].distance = distance;
            matriz[id].previousCity = GameController.instance.getLocation().getName();
            GameObject fila = contenido.transform.Find($"Fila {id+1}").gameObject;
            TextMeshProUGUI c_previaLabel = fila.transform.Find("C. Previa").gameObject.GetComponent<TextMeshProUGUI>();
            c_previaLabel.text = matriz[id].previousCity;
            TextMeshProUGUI distanceLabel = fila.transform.Find("Distancia").gameObject.GetComponent<TextMeshProUGUI>();
            distanceLabel.text = $"{matriz[id].distance}";
            GameController.instance.feedBackController.SetGoodMessage($"Se ha actualizado la distancia de {city.getName()} a {distance}");
        } else {
            string isDistance = matriz[id].distance == distance ? "igual" : "mayor";
            GameController.instance.feedBackController.SetBadMessage($"La distancia a {city.getName()} es {isDistance} a la actual distancia actual.");
        }
    }

}