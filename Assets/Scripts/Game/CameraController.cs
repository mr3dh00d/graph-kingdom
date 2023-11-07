using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class CameraController {
    [SerializeReference] public GameObject cameraTransport;
    private float speed = 15f;
    private bool isMoving = false;

    public void MoveCameraToCity(City targetCity) {
        if (!isMoving) {
            Vector3 position = targetCity.getPosition();
            GameController.instance.StartCoroutine(MoveToTarget(position));
        }
    }

    public void SetCameraPosition(Vector3 position) {
        cameraTransport.transform.position = new Vector3(
            position.x,
            cameraTransport.transform.position.y,
            position.z
        );
    }

    private IEnumerator MoveToTarget(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = new Vector3(
            cameraTransport.transform.position.x,
            cameraTransport.transform.position.y,
            cameraTransport.transform.position.z
        );
        float distance = Vector3.Distance(startPosition, targetPosition);

        float journeyLength = distance / speed;
        float startTime = Time.time;

        while (Time.time - startTime < journeyLength)
        {
            float journeyFraction = (Time.time - startTime) / journeyLength;
            cameraTransport.transform.position = Vector3.Lerp(startPosition, targetPosition, journeyFraction);
            yield return null;
        }

        cameraTransport.transform.position = targetPosition;
        isMoving = false;
    }

}