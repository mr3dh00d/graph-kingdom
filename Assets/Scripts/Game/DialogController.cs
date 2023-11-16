using System;
using System.Collections;
using System.Collections.Generic;
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
        GameController.instance.StartCoroutine(TogglePanel());
        NextLine();
    }

    public void ShowFinalMessage(){
        phase = 4;
        index = 0;
        NextLineFlag = true;
        animatorInputPanel.SetTrigger("toggle");
        LoadFinalDialogs();
        GameController.instance.StartCoroutine(TogglePanel());
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
        GameController.instance.StartCoroutine(TogglePanel());
        NextLine();
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
            text.text += letter;
            yield return new WaitForSecondsRealtime(textSpeed);
        }
        if (NextLineFlag) buttonObject.SetActive(true);
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
            case 8:
                animatorInputPanel.SetTrigger("toggle");
                NextLineFlag = false;
                break;
            case 9:
                if (!GameController.instance.isUserSet()) {
                    GameController.instance.feedBackController.SetBadMessage("Debes ingresar un nombre");
                    return true;
                }
                animatorInputPanel.SetTrigger("toggle");
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
                GameController.instance.IniciarRutina(TogglePanel());
                return true;
            case 2:
                animatorInputPanel.SetTrigger("toggle");
                NextLineFlag = false;
                break;
            case 3:
                if (!fetchCommandFlag) {
                    GameController.instance.feedBackController.SetBadMessage("Debes ejecutar el comando fetch");
                    return true;
                }
                NextLineFlag = true;
                break;
            case 4:
                NextLineFlag = false;
                break;
            case 5:
                if (!saveCommandFlag) {
                    GameController.instance.feedBackController.SetBadMessage("Debes ejecutar el comando save");
                    return true;
                }
                NextLineFlag = true;
                break;
            case 8:
                NextLineFlag = false;
                break;
            case 9:
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
            "Estoy aquí para ayudarte a aprender sobre un algoritmo muy importante: Dijkstra",//1
            "¿Quieres aprender cómo funciona?",//2
            "¡Genial! ¡Vamos a empezar!",//3
            "Primero, debo saber tu nombre",//4
            "Para poder decirme tu nombre debes ejecutar un comando en la consola",//5
            "Si no entiendes que es un comando, no te preocupes, yo te lo explicaré",//6
            "La consola es una especie de magia que te permitirá comunicarte conmigo",//7
            "Y un comando es una palabra mágica que me permitirá hacer cosas",//8
            "Para decirme tu nombre debes de escribir en el recuadro de abajo:\n\"name nombre\"\n(Por ejemplo, si tu nombre es Juan, debes escribir \"name Juan\")"//9
        };
    }

    private void LoadTutorialDialogs() {
        index = 0;
        dialogos = new List<string> {
            $"¡Hola {GameController.instance.getUser().username}! ¡Es un gusto conocerte!\n¡Empecemos!",//0
            "Para poder aprender sobre Dijkstra, primero debes entender un grafo",//1
            "Un grafo es un conjunto de nodos y aristas.\nEn este caso, los nodos son las ciudades y las aristas son los caminos que las conectan",//2
            "Para poder ver los costos de los caminos adyacentes utiliza el comando \"fetch\"",//3
            "Excelente, ahora puedes ver los costos de los caminos adyacentes",//4
            "Ahora, para poder guardar el costo de un camino debes utilizar el comando \"save nombre_ciudad\"",//5
            "¡Muy bien! Ahora ya sabes como guardar los costos de los caminos",//6
            "Arriba en la esquina superior izquierda puedes apretar el botón para ver el costo de los caminos que has guardado",//7
            "¡Genial! Ahora ya sabes como guardar los costos de los caminos",//8
            "Ahora para poder visitar una ciudad debes utilizar el comando \"visit nombre_ciudad\"",//9
            "¡Muy bien! Ahora ya sabes como visitar una ciudad",//10
            $"Tu objetivo es completar la matriz de costos con los caminos más cortos entre las ciudades desde {GameController.instance.getInitialCity().getName()}",//11
            "Para esto debes utilizar los comandos \"fetch\", \"save\" y \"visit\"",//12
            "Una vez que hayas completado la matriz de costos, podrás utilizar el comando \"check\" para verificar si lo has hecho bien",//13
            "Si necesitas ayuda puedes utilizar el comando \"help\"\nSuerte en tu aventura",//14
        };
    }

    private void LoadHelpDialogs(string command = "")
    {
        switch (command.ToLower())
        {
            case "fetch":
                dialogos = new List<string> {
                    "El comando se usa de la siguiente forma:\n\"fetch\"",//0
                    "Este comando te permite ver los costos de los caminos adyacentes",//1
                };
                break;
            case "save":
                dialogos = new List<string> {
                    "El comando se usa de la siguiente forma:\n\"save nombre_ciudad\"",//0
                    "Por ejemplo, si quieres guardar el costo del camino a Buenos Aires, debes escribir:\n\"save Buenos Aires\"",//1
                    "Si la ciudad no existe o no es vecina de la ciudad en la que te encuentras, no podrás guardarla",//2
                    "Si el camino ya fue guardado, podrás volver a guardarlo siempre y cuando sea menor al ya guardado",//3
                };
                break;
            case "visit":
                dialogos = new List<string> {
                    "El comando se usa de la siguiente forma:\n\"visit nombre_ciudad\"",//0
                    "Por ejemplo, si quieres visitar la ciudad de Buenos Aires, debes escribir:\n\"visit Buenos Aires\"",//1
                    "Si la ciudad no existe o no es vecina de la ciudad en la que te encuentras, no podrás visitarla",//2
                };
                break;
            case "check":
                dialogos = new List<string> {
                    "El comando se usa de la siguiente forma:\n\"check\"",//0
                    "Si la matriz de costos está completa, te diré si está bien o mal",//1
                };
                break;
            case "help":
                dialogos = new List<string> {
                    "El comando se usa de la siguiente forma:\n\"help comando\"",//0
                    "Este comando te dará una ayuda sobre cualquier comando que necesites",//1
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
            "¡Felicidades! ¡Has completado la matriz de costos!",//0
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