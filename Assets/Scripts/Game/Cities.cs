using System;
using UnityEngine;
public class Cities : MonoBehaviour {
    [SerializeField] public City Prague = new City("Prague", 1, new int[] {2,6,7,10});
    [SerializeField] public City Beroun = new City("Beroun", 2, new int[] {1,3,4,5});
    [SerializeField] public City Benesov = new City("Benesov", 3, new int[] {2,4});
    [SerializeField] public City Sedleany = new City("Sedleany", 4, new int[] {2,3,5});
    [SerializeField] public City Pribram = new City("Pribram", 5, new int[] {2,4});
    [SerializeField] public City Rakovnik = new City("Rakovnik", 6, new int[] {1});
    [SerializeField] public City Slany = new City("Slany", 7, new int[] {1,8,9});
    [SerializeField] public City Terezin = new City("Terezin", 8, new int [] {7,9});
    [SerializeField] public City Melnik = new City("Melnik", 9, new int[] {7,8,10});
    [SerializeField] public City Brandys_nad = new City("Brandys nad", 10, new int[] {1,9,11,12});
    [SerializeField] public City Kolin = new City("Kolin", 11, new int[] {10,12});
    [SerializeField] public City Kurim = new City("Kurim", 12, new int[] {10,11});

    public City[] GetCities() {
        return new City[] {
            Prague,
            Beroun,
            Benesov,
            Sedleany,
            Pribram,
            Rakovnik,
            Slany,
            Terezin,
            Melnik,
            Brandys_nad,
            Kolin,
            Kurim
        };
    }

    public int GetNumCities() {
        return GetCities().Length;
    }

    public City GetCityById(int id) {
        foreach (City city in GetCities())
        {
            if (city.getId() == id) {
                return city;
            }
        }
        return null;
    }

    public City GetCityByName(string name) {
        foreach (City city in GetCities())
        {
            if (
                city.getName().ToLower() == name.ToLower()
            ) {
                return city;
            }
        }
        return null;
    }


    // public static readonly City Prague = new City("Prague", 1, new int[] {2,6,7,10});
    // public static readonly City Beroun = new City("Beroun", 2, new int[] {1,3,4,5});
    // public static readonly City Benesov = new City("Benesov", 3, new int[] {2,4});
    // public static readonly City Sedleany = new City("Sedleany", 4, new int[] {2,3,5});
    // public static readonly City Pribram = new City("Pribram", 5, new int[] {2,4});
    // public static readonly City Rakovnik = new City("Rakovnik", 6, new int[] {1});
    // public static readonly City Slany = new City("Slany", 7, new int[] {1,9});
    // public static readonly City Terezin = new City("Terezin", 8, new int [] {7,9});
    // public static readonly City Melnik = new City("Melnik", 9, new int[] {7,8,10});
    // public static readonly City Brandys_nad = new City("Brandys nad", 10, new int[] {1,9,11,12});
    // public static readonly City Kolin = new City("Kolin", 11, new int[] {10,12});
    // public static readonly City Kurim = new City("Kurim", 12, new int[] {10,11});

}