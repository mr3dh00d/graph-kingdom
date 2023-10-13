using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] float speed = 20;
    Vector3 direction;
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        direction = new Vector3(horizontal, 0f, vertical);
        if (direction.magnitude > 1f)
            direction = direction.normalized;
        direction *= speed * Time.deltaTime;
        transform.Translate(direction);
    }
}
