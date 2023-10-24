class City {
    public string name;
    public int id;

    public City(string name, int id) {
        this.name = name;
        this.id = id;
    }

}
class Cities {
    
    public static readonly City Prague = new City("Prague", 1);
    public static readonly City Beroun = new City("Beroun", 2);
    public static readonly City Benesov = new City("Benesov", 3);
    public static readonly City Sedleany = new City("Sedleany", 4);
    public static readonly City Pribram = new City("Pribram", 5);
    public static readonly City Rakovnik = new City("Rakovnik", 6);
    public static readonly City Slany = new City("Slany", 7);
    public static readonly City Terezin = new City("Terezin", 8);
    public static readonly City Melnik = new City("Melnik", 9);
    public static readonly City Brandys_nad = new City("Brandys nad", 10);
    public static readonly City Kolin = new City("Kolin", 11);
    public static readonly City Kurim = new City("Kurim", 12);

}