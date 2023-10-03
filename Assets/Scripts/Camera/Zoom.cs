using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    new Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        if (zoom != 0f){
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - zoom*15f, 10f, 40f);
        }

    }
}
