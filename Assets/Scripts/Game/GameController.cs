using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField ]public FeedBackController feedBackController;
    [SerializeField] public bool DragMovementActive = true;
    [SerializeField] public GameObject [] Merchants;
    [SerializeField] public Cities cities; 
    private City location;


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
        location = cities.Prague;
        feedBackController.LoadObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CompileInput(string input)
    {
        // Normaliza el input
        input = input.ToLower().Replace("\u200B", "").Trim();
        if (input.Length == 0)
        {
            feedBackController.setBadMessage("No has ingresado ningún comando");
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
                feedBackController.setBadMessage($"El comando \"{command}\" no existe");
                break;
        }        
        
    }

    private void Fetch()
    {
        Debug.Log("Fetching");
        int merchant_encommendment = 0;
        int direction;
        Animator animator;
        foreach (int neighbor in location.getNeighbors())
        {   
            animator = Merchants[merchant_encommendment].GetComponent<Animator>();
            direction = int.Parse($"{location.getId()}{neighbor}");
            animator.SetInteger("direction", direction);
            merchant_encommendment++;
        }
    }
}
