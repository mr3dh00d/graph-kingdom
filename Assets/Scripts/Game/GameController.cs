using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] public bool DragMovementActive = true;

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
        Debug.Log(Cities.Brandys_nad.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
