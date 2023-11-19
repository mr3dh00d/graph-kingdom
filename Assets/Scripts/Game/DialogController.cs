using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DialogController {
    [SerializeField] public GameObject dialogPanel;
    [SerializeField] public float textSpeed = 0.01f;
    public List<string> dialogos;
    private Animator animator;
    private TextMeshProUGUI text;

    private GameObject buttonObject;
    private Button button;
    private int phase = 1;
    private int index;
    public bool fetchCommandFlag = false;
    public bool saveCommandFlag = false;
    public bool visitCommandFlag = false;
    private bool NextLineFlag = true;
    private Animator animatorInputPanel;


    public void Load() {
        LoadStartDialogs();
        animator = dialogPanel.GetComponent<Animator>();
        animatorInputPanel = GameController.instance.InputPanel.GetComponent<Animator>();
        text = dialogPanel.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        buttonObject = dialogPanel.transform.Find("Button").gameObject;
        buttonObject.SetActive(false);
        button = buttonObject.GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void ShowWelcomeMessage() {
        // @todo: mostrar mensaje de bienvenida
        // animatorInputPanel.SetTrigger("toggle");
        // fetchCommandFlag = true;
        // saveCommandFlag = true;
        // visitCommandFlag = true;

        index = 0;
        GameController.instance.StartCoroutine(OpenPanel());
        NextLine();
    }

    public void ShowFinalMessage(){
        phase = 4;
        index = 0;
        NextLineFlag = true;
        animatorInputPanel.SetTrigger("toggle");
        LoadFinalDialogs();
        GameController.instance.StartCoroutine(OpenPanel());
        NextLine();
    }

    public void ShowHelpMessage() {
        phase = 3;
        index = 0;
        LoadHelpDialogs();
        GameController.instance.StartCoroutine(TogglePanel());
        NextLine();
    }

    public void ShowThisCommandHelp(string command) {
        phase = 3;
        index = 0;
        LoadHelpDialogs(command);
        GameController.instance.StartCoroutine(OpenPanel());
        NextLine();
    }

    private IEnumerator OpenPanel()
    {
        animator.SetTrigger("open");
        yield return new WaitForSeconds(1f);
    }
    private IEnumerator ClosePanel()
    {
        animator.SetTrigger("close");
        yield return new WaitForSeconds(1f);
    }
    private IEnumerator TogglePanel()
    {
        animator.SetTrigger("toggle");
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator ShowText(string message) {
        text.text = "";
        buttonObject.SetActive(false);
        foreach (char letter in message.ToCharArray()) {
            if (text.text == message) break;
            text.text += letter;
            yield return new WaitForSecondsRealtime(textSpeed);
        }
        if (NextLineFlag) buttonObject.SetActive(true);
    }

    public void skipDialog() {
        text.text = dialogos[index];
    }

    public void buttonPress(){
        if(buttonObject.activeSelf) OnClick();
    }


    private void NextLine() {
        GameController.instance.IniciarRutina(ShowText(dialogos[index]));
    }

    private void OnClick() {
        switch (phase)
        {
            case 1:
                if(conditionsStart()) return;
                break;
            case 2:
                if(conditionsTutorial()) return;
                break;
            case 3:
                if(ConditionsHelp()) return;
                break;
            case 4:
                if(conditionsFinish()) return;
                break;
        }   
        index++;
        NextLine();
    }

    private bool conditionsStart() {
        switch (index)
        {
            case 4:
                animatorInputPanel.SetTrigger("show");
                NextLineFlag = false;
                break;
            case 5:
                if (!GameController.instance.isUserSet()) {
                    GameController.instance.feedBackController.SetBadMessage("Debes ingresar un nombre");
                    return true;
                }
                animatorInputPanel.SetTrigger("hidde");
                phase = 2;
                LoadTutorialDialogs();
                NextLineFlag = true;
                break;
        }
        return false;
    }

    private bool conditionsTutorial() {
        switch (index)
        {
            case int n when n >= dialogos.Count-1:
                text.text = "";
                buttonObject.SetActive(false);
                GameController.instance.IniciarRutina(ClosePanel());
                return true;
            case 8:
                animatorInputPanel.SetTrigger("show");
                NextLineFlag = false;
                break;
            case 9:
                if (!fetchCommandFlag) {
                    GameController.instance.feedBackController.SetBadMessage("Debes ejecutar el comando fetch");
                    return true;
                }
                NextLineFlag = true;
                break;
            case 11:
                NextLineFlag = false;
                break;
            case 12:
                if (!saveCommandFlag) {
                    GameController.instance.feedBackController.SetBadMessage("Debes ejecutar el comando save");
                    return true;
                }
                NextLineFlag = true;
                break;
            case 15:
                NextLineFlag = false;
                break;
            case 16:
                if (!visitCommandFlag) {
                    GameController.instance.feedBackController.SetBadMessage("Debes ejecutar el comando save");
                    return true;
                }
                NextLineFlag = true;
                break;
            

        }
        return false;
    }
    private bool ConditionsHelp() {
        if (index >= dialogos.Count-1) {
            text.text = "";
            buttonObject.SetActive(false);
            GameController.instance.IniciarRutina(TogglePanel());
            return true;
        }
        return false;
    }

    private bool conditionsFinish() {
        if (index >= dialogos.Count-1) {
            text.text = "";
            buttonObject.SetActive(false);
            GameController.instance.IniciarRutina(TogglePanel());
            GameController.instance.IniciarRutina(GameController.instance.CloseGame());
        }
        return false;
    }

    private void LoadStartDialogs() {
        dialogos = new List<string> {
            $"Hola, soy Fernet, Rey de {GameController.instance.getInitialCity().getName()}", //0
            "Estoy aquí para ayudarte a aprender sobre un algoritmo muy importante: Dijkstra\n¡Vamos a empezar!",//1
            "Primero, debo saber tu nombre",//2
            "Para poder decirme tu nombre debes usar la casilla de instrucciones",//3
            "La casilla de instrucciones sirve para comunicarte conmigo, y decirme que hacer, comenzaremos con que me diags tu nombre",//4
            "Para decirme tu nombre debes de escribir en el recuadro de abajo:\n\"name nombre\"\n(Por ejemplo, si tu nombre es Juan, debes escribir \"name Juan\")"//5
        };
    }

    private void LoadTutorialDialogs() {
        index = 0;
        int [] neighbors = GameController.instance.getInitialCity().getNeighbors();
        string [] neighborsName = neighbors.Select(n => GameController.instance.cities.GetCityById(n).getName()).ToArray();
        string neighbor = neighborsName[UnityEngine.Random.Range(0, neighbors.Length)];
        dialogos = new List<string> {
            $"¡Hola {GameController.instance.getUser().username}! ¡Es un gusto conocerte!\n¡Empecemos!",//0
            "Para poder aprender sobre Dijkstra, primero debes entender un grafo.",//1
            "Un grafo es un conjunto de nodos y aristas.\nEn este caso, los nodos son las ciudades y las aristas son los caminos que las conectan",//2
            "Dijkstra es un algoritmo que permite encontrar el camino más corto desde un nodo de origen a todos los demás nodos en un grafo.",//3
            "Dijkstra funciona de forma iterativa, es decir, se repite varias veces hasta que se cumpla una condición",//4
            "En cada iteración, Dijkstra visita un nodo y actualiza la distancia de sus vecinos",//5
            $"{GameController.instance.getInitialCity().getName()} es la ciudad en la que te encuentras y lo puedes reconocer por que su nombre esta subrayado.",//6
            "Tu misión es seguir los pasos de Dijkstra para encontrar el camino más corto desde la ciudad en la que te encuentras hasta todas las demás ciudades",//7
            "Para esto utilizaras 3 instrucciones que debes repetir constantemente.",//8
            "Empezaremos con la instrucción \"fetch\".\nTe permitirá ver los costos de los caminos a las ciudades adyacentes",//9
            "Excelente, ahora puedes ver los costos de los caminos adyacentes",//10
            $"{GameController.instance.getInitialCity().getName()} tiene como vecinas a {string.Join(", ", neighborsName)}",//11
            $"La segunda instrucción te permite guardar el costo a una ciudad adyacente.\nUtilizarás la instrucción \"save ciudad_vecina\"\nEjemplo: \"save {neighbor}\"",//12
            "¡Muy bien! Ahora ya sabes como guardar los costos de los caminos",//13
            "Arriba en la esquina superior izquierda puedes apretar el botón para ver/ocultar el costo de los caminos que has guardado",//14
            "Repite esto con todas las ciudades adyacentes",//15
            $"La última instrucción te permitirá visitar una ciudad adyacente.\nDebes utilizar la instrucción \"visit ciudad\"\nEjemplo: \"visit {neighbor}\"",//16
            "¡Muy bien! Ahora ya sabes como visitar una ciudad",//17
            $"Tu objetivo es completar la matriz de costos con los caminos más cortos entre las ciudades desde {GameController.instance.getInitialCity().getName()}",//18
            "Para esto debes repetir este proceso utilizando las instrucciones \"fetch\", \"save\" y \"visit\"",//19
            "Una vez que hayas completado la matriz de costos, podrás utilizar la instrucción \"check\" para verificar si lo has hecho bien",//20
            "Si necesitas ayuda puedes utilizar la instrucción \"help\"\nSuerte en tu aventura",//21
        };
    }

    private void LoadHelpDialogs(string command = "")
    {
        switch (command.ToLower())
        {
            case "fetch":
                dialogos = new List<string> {
                    "La instrucción se usa de la siguiente forma:\n\"fetch\"",//0
                    "Esta instrucción te permite ver los costos de los caminos adyacentes",//1
                };
                break;
            case "save":
                dialogos = new List<string> {
                    "La instrucción se usa de la siguiente forma:\n\"save nombre_ciudad\"",//0
                    $"Por ejemplo, si quieres guardar el costo del camino a {GameController.instance.cities.Pribram.getName()}, debes escribir:\n\"save {GameController.instance.cities.Pribram.getName()}\"",//1
                    "Si la ciudad no existe o no es vecina de la ciudad en la que te encuentras, no podrás guardarla",//2
                    "Si el camino ya fue guardado, podrás volver a guardarlo siempre y cuando sea menor al ya guardado",//3
                };
                break;
            case "visit":
                dialogos = new List<string> {
                    "La instrucción se usa de la siguiente forma:\n\"visit nombre_ciudad\"",//0
                    $"Por ejemplo, si quieres visitar la ciudad de {GameController.instance.cities.Pribram.getName()}, debes escribir:\n\"visit {GameController.instance.cities.Pribram.getName()}\"",//1
                    "Si la ciudad no existe o no es vecina de la ciudad en la que te encuentras, no podrás visitarla",//2
                };
                break;
            case "check":
                dialogos = new List<string> {
                    "La instrucción se usa de la siguiente forma:\n\"check\"",//0
                    "Si la matriz de costos está completa, te diré si está bien o mal",//1
                };
                break;
            case "help":
                dialogos = new List<string> {
                    "La instrucción se usa de la siguiente forma:\n\"help instrucción\"",//0
                    "Esta instrucción te dará una ayuda sobre cualquier instrucción que necesites",//1
                };
                break;
            default:
                dialogos = new List<string> {
                    "Los comandos que puedes utilizar son:\n- fetch\n- save nombre_ciudad\n- visit nombre_ciudad\n- check",//0
                };
                break;
        }
    }

    public void LoadFinalDialogs(){
        dialogos = new List<string> {
            $"¡Felicidades {GameController.instance.getUser().username}! ¡Has completado la matriz de costos mínimos con éxito!",//0
            "Ahora ya sabes como funciona el algoritmo de Dijkstra",//1
            "¡Gracias por jugar!\n¡Espero que hayas aprendido mucho!",//2
        };
    }

    public void ConfirmFetchCommand() {
        fetchCommandFlag = true;
        OnClick();
    }
    public void ConfirmSaveCommand() {
        saveCommandFlag = true;
        OnClick();
    }
    public void ConfirmVisitCommand() {
        visitCommandFlag = true;
        OnClick();
    }
    public void showButton() {
        buttonObject.SetActive(true);
    }
}