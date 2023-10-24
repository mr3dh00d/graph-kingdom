using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] public bool DragMovementActive = true;
    [SerializeField] public GameObject [] Merchants;
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
        location = Cities.Prague;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CompileCommand(string command)
    {
        Debug.Log($"Compiling command: {command}");
        fetch();
    }

    private void fetch()
    {
        Debug.Log("Fetching");
        int merchant_encommendment = 0;
        int direction;
        Animator animator;
        foreach (int neighbor in location.neighbors)
        {   
            animator = Merchants[merchant_encommendment].GetComponent<Animator>();
            direction = int.Parse($"{location.id}{neighbor}");
            Debug.Log("direction: " + direction);
            animator.SetInteger("direction", direction);
            merchant_encommendment++;
        }
    }
}
