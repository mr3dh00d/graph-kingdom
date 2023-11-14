using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections;

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
    private City initialCity;
    int [][] pathsCosts;

    public bool isCompiling = false;
    private int rutinasActivas = 0;


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
        initialCity = cities.Pribram;
        ChangeLocation(initialCity);
        cameraController.SetCameraPosition(location.getPosition());
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
        if (location != null)
        {
            location.title.fontStyle = FontStyles.Normal;
        }
        location = city;
        if (city.getId() != initialCity.getId()) {
            city.title.color = Color.yellow;
        }
        city.title.fontStyle = FontStyles.Underline;

    }

    public void CompileInput(string input)
    {
        if (isCompiling) return;
        isCompiling = true;
        // Normaliza el input
        input = input.Replace("\u200B", "").Trim();
        if (input.Length == 0)
        {
            feedBackController.SetBadMessage("No has ingresado ningún comando");
            return;
        }

        string[] command_with_args = input.Split(' ');
        string command = command_with_args[0];
        string[] args = new string[command_with_args.Length - 1];
        Array.Copy(command_with_args, 1, args, 0, command_with_args.Length - 1);
        switch (command.ToLower())
        {
            case "visit":
                Visit(args);
                break;
            case "fetch":
                Fetch();
                break;
            case "save":
                Save(args);
                break;
            case "check":
                Check();
                break;
            default:
                feedBackController.SetBadMessage($"El comando \"{command}\" no existe");
                break;
        }        
        
    }

    private void Save(string[] args)
    {
        string neighbor = string.Join(" ", args);
        if (neighbor == null || neighbor.Length == 0)
        {
            feedBackController.SetBadMessage("Es necesario indicar la ciudad a la que se desea guardar");
            return;
        }
        City targetCity = cities.GetCityByName(neighbor);
        if(targetCity == null) {
            feedBackController.SetBadMessage($"La ciudad \"{neighbor.FirstCharacterToUpper()}\" no existe");
            return;
        } else if (!location.isNeighbor(targetCity.getId())) {
            feedBackController.SetBadMessage($"La ciudad \"{neighbor.FirstCharacterToUpper()}\" no es vecina de \"{location.getName()}\"");
            return;
        }
        int distance = pathsCosts[location.getId() - 1][targetCity.getId() - 1];
        matrizController.SetDistance(targetCity, distance);
    }

    private void Visit(string[] args)
    {
        string neighbor = string.Join(" ", args);
        if (neighbor == null || neighbor.Length == 0)
        {
            feedBackController.SetBadMessage("Es necesario indicar la ciudad a la que se desea visitar");
            return;
        }
        City targetCity = cities.GetCityByName(neighbor);
        if(targetCity == null) {
            feedBackController.SetBadMessage($"La ciudad \"{neighbor.FirstCharacterToUpper()}\" no existe");
            return;
        } else if (!location.isNeighbor(targetCity.getId())) {
            feedBackController.SetBadMessage($"La ciudad \"{neighbor.FirstCharacterToUpper()}\" no es vecina de \"{location.getName()}\"");
            return;
        }
        if (location.fetchedNeighbors < location.getNeighbors().Length)
        {
            feedBackController.SetBadMessage($"No se puede visitar \"{neighbor.FirstCharacterToUpper()}\" porque no se se sabe la distancia.");
            return;
        }
        Animator animator = Merchants[0].GetComponent<Animator>();
        int direction = int.Parse($"{location.getId()}{targetCity.getId()}");
        animator.SetInteger("direction", direction);
        cameraController.MoveCameraToCity(location, targetCity);
        matrizController.SetVisited(targetCity);
        ChangeLocation(targetCity);
    }

    private void Fetch()
    {
        int merchant_encommendment = 0;
        int direction;
        Animator animator;
        foreach (int neighbor in location.getNeighbors())
        {
            if (location.fetchedNeighbors < location.getNeighbors().Length) location.fetchedNeighbors++;
            animator = Merchants[merchant_encommendment].GetComponent<Animator>();
            direction = int.Parse($"{location.getId()}{neighbor}");
            animator.SetInteger("direction", direction);
            pathsLabelsController.RevealCost(location.getId(), neighbor, pathsCosts[location.getId() - 1][neighbor - 1]);
            merchant_encommendment++;
        }
    }

    private void Check()
    {

    }

    public void IniciarRutina(IEnumerator func)
    {
        
        StartCoroutine(HandleCoroutine(func));
    }

    private IEnumerator HandleCoroutine(IEnumerator func)
    {
        rutinasActivas++;

        // Lógica de la corutina especificada
        yield return func;


        rutinasActivas--;
        if (rutinasActivas <= 0)
        {
            isCompiling = false;
        }
    }

    public City getLocation()
    {
        return location;
    }

    public City getInitialCity()
    {
        return initialCity;
    }
}
