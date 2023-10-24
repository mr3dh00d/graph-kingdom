class City {
    public string name;
    public int id;
    public int [] neighbors;

    public City(string name, int id, int [] neighbors) {
        this.name = name;
        this.id = id;
        this.neighbors = neighbors;
    }

}
class Cities {
    
    public static readonly City Prague = new City("Prague", 1, new int[] {2,6,7,10});
    public static readonly City Beroun = new City("Beroun", 2, new int[] {1,3,4,5});
    public static readonly City Benesov = new City("Benesov", 3, new int[] {2,4});
    public static readonly City Sedleany = new City("Sedleany", 4, new int[] {2,3,5});
    public static readonly City Pribram = new City("Pribram", 5, new int[] {2,4});
    public static readonly City Rakovnik = new City("Rakovnik", 6, new int[] {1});
    public static readonly City Slany = new City("Slany", 7, new int[] {1,9});
    public static readonly City Terezin = new City("Terezin", 8, new int [] {7,9});
    public static readonly City Melnik = new City("Melnik", 9, new int[] {7,8,10});
    public static readonly City Brandys_nad = new City("Brandys nad", 10, new int[] {1,9,11,12});
    public static readonly City Kolin = new City("Kolin", 11, new int[] {10,12});
    public static readonly City Kurim = new City("Kurim", 12, new int[] {10,11});

}