using System;
using System.ComponentModel;
using UnityEngine;
public class City {
    [SerializeField] private string name;
    [SerializeField] private int id;
    [SerializeField] private int [] neighbors;
    [SerializeField] public GameObject title;
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

}