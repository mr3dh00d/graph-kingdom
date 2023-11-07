using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class CameraController {
    [SerializeReference] public GameObject cameraTransport;

    public void MoveCameraToCity(City city) {
        Vector3 position = city.getPosition();
        SetCameraPosition(position);
    }

    public void SetCameraPosition(Vector3 position) {
        cameraTransport.transform.position = new Vector3(
            position.x,
            cameraTransport.transform.position.y,
            position.z
        );
    }

}