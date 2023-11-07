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

        pathsCosts = new int[cities.getCities().Length][];
        foreach (City city in cities.getCities())
        {
            pathsCosts[city.getId() - 1] = new int[cities.getCities().Length];
            foreach (int neighbor in city.getNeighbors())
            {
                pathsCosts[city.getId() - 1][neighbor - 1] = UnityEngine.Random.Range(1, 10);
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
        switch (command)
        {
            case "fetch":
                Fetch();
                break;
            default:
                feedBackController.SetBadMessage($"El comando \"{command}\" no existe");
                break;
        }        
        
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
