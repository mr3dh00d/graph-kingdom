using System;
using System.ComponentModel;
using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;
[Serializable]
public class City {
    [SerializeField] private string name;
    [SerializeField] private int id;
    [SerializeField] private int [] neighbors;
    [SerializeField] public TextMeshPro title;
    [SerializeField] public GameObject city;

    public City(string name, int id, int [] neighbors) {
        this.name = name;
        this.id = id;
        this.neighbors = neighbors;
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

}