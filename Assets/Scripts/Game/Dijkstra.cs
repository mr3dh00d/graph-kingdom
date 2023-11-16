using System;
using Unity.VisualScripting;
using UnityEngine;

public class Dijkstra
{
    public DistanceInfo [] matrizSolucion;

    public void ApplyDijkstra()
    {
        Cities cities = GameController.instance.cities;
        int numCities = cities.GetNumCities();

        matrizSolucion = new DistanceInfo[numCities];

        // Inicializar matriz con distancias infinitas y sin predecesor
        foreach (City city in cities.GetCities())
        {
            if (city.getId() == GameController.instance.getInitialCity().getId())
            {
                matrizSolucion[city.getId() - 1] = new DistanceInfo{
                    Distance = 0,
                    Predecessor = "",
                    Visited = true
                };
            } else {
                matrizSolucion[city.getId() - 1] = new DistanceInfo{
                    Distance = int.MaxValue,
                    Predecessor = city.getName(),
                    Visited = false
                };
            }
            
        }

        ApplyDijkstraAlgorithm();
    }

    private void ApplyDijkstraAlgorithm()
    {
        Cities cities = GameController.instance.cities;
        City initialCity = GameController.instance.getInitialCity();
        int numCities = cities.GetNumCities();
        int[][] pathsCosts = GameController.instance.pathsCosts;

        // Crear una clase para almacenar la información de la solución
        // Inicializar la ciudad inicial con distancia 0
        City location = initialCity;
        for (int count = 0; count < numCities - 1; count++)
        {
            int minDistance = int.MaxValue;
            int minIndex = -1;

            foreach (int neighbor in location.getNeighbors())
            {
                int neighborIndex = neighbor - 1;
                int distance = pathsCosts[location.getId() - 1][neighborIndex];
                
                if (distance > 0 && !matrizSolucion[neighborIndex].Visited &&
                    matrizSolucion[location.getId() - 1].Distance != int.MaxValue &&
                    matrizSolucion[location.getId() - 1].Distance + distance < matrizSolucion[neighborIndex].Distance)
                {
                    matrizSolucion[neighborIndex].Distance = matrizSolucion[location.getId() - 1].Distance + distance;
                    matrizSolucion[neighborIndex].Predecessor = location.getName();
                }
            }

            matrizSolucion[location.getId() - 1].Visited = true;

            // Buscar el próximo nodo con la distancia mínima no visitado
            for (int i = 0; i < numCities; i++)
            {
                if (!matrizSolucion[i].Visited && matrizSolucion[i].Distance <= minDistance)
                {
                    minDistance = matrizSolucion[i].Distance;
                    minIndex = i;
                }
            }

            // Actualizar la ubicación a la ciudad con la distancia mínima no visitada
            if (minIndex != -1)
            {
                location = cities.GetCityById(minIndex + 1);
            }
        }
    }

    public bool RevisarCamino()
    {
        DistanceInfo [] matrizUsuario = GameController.instance.matrizController.getMatriz();
        bool response = true;
        for (int i = 0; i < matrizUsuario.Length; i++)
        {
            if (
                matrizUsuario[i].Distance != matrizSolucion[i].Distance
                || matrizUsuario[i].Predecessor != matrizSolucion[i].Predecessor
            )
            {
                City city = GameController.instance.cities.GetCityById(i+1);
                GameController.instance.feedBackController.SetBadMessage($"La matriz no es correcta, el camino a {city.getName()} no es el correcto");
                response = false;
                break;
            }
        }
        return response;

    }

    public void PrintMatrizSolucion()
    {
        foreach (DistanceInfo distanceInfo in matrizSolucion)
        {
            Debug.Log($"Distancia: {distanceInfo.Distance}, Predecesor: {distanceInfo.Predecessor}");
        }
    }


}