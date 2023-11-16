using System;
using UnityEngine;
using TMPro;
[Serializable]
public class City {
    [SerializeField] private string name;
    [SerializeField] private int id;
    [SerializeField] private int [] neighbors;
    [SerializeField] public TextMeshPro title;
    [SerializeField] public GameObject city;

    public int fetchedNeighbors;

    public City(string name, int id, int [] neighbors) {
        this.name = name;
        this.id = id;
        this.neighbors = neighbors;
        this.fetchedNeighbors = 0;
    }

    public string getName() {
        return name;
    }

    public int getId() {
        return id;
    }

    public int [] getNeighbors() {
        return neighbors;
    }

    public Vector3 getPosition() {
        return city.transform.position;
    }

    public bool isNeighbor(int id) {
        return Array.IndexOf(neighbors, id) > -1;
    }

}