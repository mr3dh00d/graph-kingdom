using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] public FeedBackController feedBackController;
    [SerializeField] public DialogController dialogController;
    [SerializeField] public MatrizController matrizController;
    [SerializeField] public CameraController cameraController;
    [SerializeField] public PathsLabelsController pathsLabelsController;
    [SerializeField] public Emotions emotions;
    [SerializeField] public bool DragMovementActive = true;
    [SerializeField] public GameObject InputPanel;
    [SerializeField] public GameObject [] Merchants;
    [SerializeField] public Cities cities;
    private APIController apiController;
    private City location;
    private City initialCity;
    public int [][] pathsCosts;

    public bool isCompiling = false;
    private int rutinasActivas = 0;
    private User user;
    private TMP_InputField inputField;
    public bool isTyping = false;
    private int saveNodes = 0;
    public string sessionId;
    private Dijkstra dijkstra;

    private bool finish = false;


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
        initialCity = cities.Sedleany;
        ChangeLocation(initialCity);
        dijkstra = new Dijkstra();
        dijkstra.ApplyDijkstra();
        cameraController.SetCameraPosition(location.getPosition());
        location.title.color = ColorsConstants.HexToColor("#FFA500");
        apiController = new APIController();
        feedBackController.Load();
        matrizController.Load();
        dialogController.Load();
        dialogController.ShowWelcomeMessage();
        inputField = InputPanel.transform.Find("InputField (TMP)").gameObject.GetComponent<TMP_InputField>();
        sessionId = Guid.NewGuid().ToString();

        Debug.Log($"El ID de la sesión es {sessionId}");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)) {
            dialogController.skipDialog();
        }
        if(Input.GetKeyDown(KeyCode.E)) {
            if(!isTyping){
                dialogController.buttonPress();   
            }
        }
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
        if(user == null) {
            if (command.ToLower() != "name")
            {
                feedBackController.SetBadMessage("Debes indicar tu nombre de usuario");
                return;
            }
            string username = args[0];
            apiController.SearchUser(username);
            inputField.text = "";
            isTyping = false;
            return;
        }
        switch (command.ToLower())
        {
            case "name":
                feedBackController.SetBadMessage("Ya has indicado tu nombre de usuario");
                break;
            case "visit":
                if(!dialogController.fetchCommandFlag || !dialogController.saveCommandFlag) {
                    feedBackController.SetBadMessage("Aun no puedes visitar ciudades");
                    break;
                }
                Visit(args);
                break;
            case "fetch":
                Fetch();
                break;
            case "save":
                if(!dialogController.fetchCommandFlag) {
                    feedBackController.SetBadMessage("Aun no puedes guardar ciudades");
                    break;
                }
                Save(args);
                break;
            case "help":
                Help(args);
                break;
            case "check":
                Check();
                break;
            case "exit":
                Exit(args);
                break;
            default:
                feedBackController.SetBadMessage($"El comando \"{command}\" no existe");
                break;
        }
        inputField.text = "";
        isTyping = false;
    }

    private void Help(string[] args)
    {
        string command = string.Join(" ", args).Trim();
        if (command == null || command.Length == 0)
        {
            dialogController.ShowHelpMessage();
        } else {
            if(
                command != "fetch"
                && command != "save"
                && command != "visit"
                && command != "check"
            ) {
                feedBackController.SetBadMessage($"El comando \"{command}\" no existe");
            } else {
                dialogController.ShowThisCommandHelp(command);
            }
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
        bool result = matrizController.SetDistance(targetCity, distance);
        if(!result) return;
        saveNodes++;
        apiController.SaveRecord();
        if(!dialogController.saveCommandFlag) dialogController.ConfirmSaveCommand();
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
        if(!dialogController.visitCommandFlag) dialogController.ConfirmVisitCommand();
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
        if(!dialogController.fetchCommandFlag) dialogController.ConfirmFetchCommand();
    }

    private void Check()
    {
        // Revisar si ya guardo todas las ciudades
        foreach(DistanceInfo info in matrizController.getMatriz())
        {
            if(info.Distance == int.MaxValue || info.Predecessor == null) {
                feedBackController.SetBadMessage("Aun no has guardado todas las ciudades");
                return;
            }
        }
        // Revisar matriz
        bool result = dijkstra.RevisarCamino();
        if(result){
            feedBackController.SetGoodMessage("¡Felicidades! Has completado el juego");
            apiController.SaveRecord();
            dialogController.ShowFinalMessage();
        }

    }

    private void Exit(string[] args)
    {
        string command = string.Join(" ", args).Trim();
        if (command == null || command.Length == 0)
        {
            if(!finish) {
                feedBackController.SetBadMessage("Debes terminar el juego para salir");
            }
        }
        if(command != "force") return;

        dialogController.ShowFinalMessage();
    }

    public IEnumerator CloseGame(){
        finish = true;
        Application.OpenURL("https://forms.gle/EKWw2qzY8e7rvNkr7");
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
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

    public void setUser(string id, string username)
    {
        user = new User {
            id = id,
            username = username
        };
    }

    public int getNodosGuardados()
    {
        return saveNodes;
    }

    public bool isUserSet()
    {
        return user != null && user.username != null && user.username.Length > 0 && user.id != null && user.id.Length > 0;
    }

    public User getUser()
    {
        return user;
    }
}
