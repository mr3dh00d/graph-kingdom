using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] public FeedBackController feedBackController;
    [SerializeField] public MatrizController matrizController;
    [SerializeField] public CameraController cameraController;
    [SerializeField] public PathsLabelsController pathsLabelsController;
    [SerializeField] public bool DragMovementActive = true;
    [SerializeField] public GameObject [] Merchants;
    [SerializeField] public Cities cities; 
    private City location;
    int [][] pathsCosts;


    private void Awake()
    {
        // Verifica si ya hay una instancia existente
        if (instance == null)
        {
            // Si no hay una instancia, esta se convierte en la instancia
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si ya hay una instancia, destruye esta
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SetPathsCosts();
        ChangeLocation(cities.Pribram);
        location.title.color = ColorsConstants.HexToColor("#FFA500");
        feedBackController.Load();
        matrizController.Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetPathsCosts() {

        pathsCosts = new int[cities.GetCities().Length][];
        for (int i = 0; i < pathsCosts.Length; i++)
        {
            pathsCosts[i] = new int[cities.GetCities().Length];
            for (int j = 0; j < pathsCosts[i].Length; j++)
            {
                pathsCosts[i][j] = -1;
            }
        }

        foreach (City city in cities.GetCities())
        {
            foreach (int neighbor in city.getNeighbors())
            {
                if(pathsCosts[city.getId() - 1][neighbor - 1] == -1) {
                    int cost = UnityEngine.Random.Range(1, 10);
                    
                    pathsCosts[city.getId() - 1][neighbor - 1] = cost;
                    pathsCosts[neighbor - 1][city.getId() - 1] = cost;

                }
            }
        }
    }

    public void ChangeLocation(City city)
    {
        location = city;
        city.title.color = Color.yellow;

        cameraController.SetCameraPosition(city.getPosition());
    }

    public void CompileInput(string input)
    {
        // Normaliza el input
        input = input.ToLower().Replace("\u200B", "").Trim();
        if (input.Length == 0)
        {
            feedBackController.SetBadMessage("No has ingresado ningún comando");
            return;
        }

        string[] command_with_args = input.Split(' ');
        string command = command_with_args[0];
        string[] args = new string[command_with_args.Length - 1];
        Array.Copy(command_with_args, 1, args, 0, command_with_args.Length - 1);
        switch (command)
        {
            case "visit":
                Visit(args);
                break;
            case "fetch":
                Fetch();
                break;
            default:
                feedBackController.SetBadMessage($"El comando \"{command}\" no existe");
                break;
        }        
        
    }

    private void Visit(string[] args)
    {
        string neighbor = args[0].Trim();
        if (neighbor == null || neighbor.Length == 0)
        {
            feedBackController.SetBadMessage("No has ingresado ninguna ciudad");
            return;
        }
        City targetCity = cities.GetCityByName(neighbor);
        if(targetCity == null) {
            feedBackController.SetBadMessage($"La ciudad \"{neighbor}\" no existe");
            return;
        } else if (!location.isNeighbor(targetCity.getId())) {
            feedBackController.SetBadMessage($"La ciudad \"{neighbor}\" no es vecina de \"{location.getName()}\"");
            return;
        }
        Animator animator = Merchants[0].GetComponent<Animator>();
        int direction = int.Parse($"{location.getId()}{targetCity.getId()}");
        animator.SetInteger("direction", direction);
        cameraController.MoveCameraToCity(targetCity);
        ChangeLocation(targetCity);
    }

    private void Fetch()
    {
        int merchant_encommendment = 0;
        int direction;
        Animator animator;
        foreach (int neighbor in location.getNeighbors())
        {   
            animator = Merchants[merchant_encommendment].GetComponent<Animator>();
            direction = int.Parse($"{location.getId()}{neighbor}");
            animator.SetInteger("direction", direction);
            pathsLabelsController.RevealCost(location.getId(), neighbor, pathsCosts[location.getId() - 1][neighbor - 1]);
            merchant_encommendment++;
        }
    }

    public City getLocation()
    {
        return location;
    }
}
