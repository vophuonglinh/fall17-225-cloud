using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public GameObject player;

    void LateUpdate()
    {
        transform.position = player.transform.position;
        Vector3 velocity = player.GetComponent<Rigidbody>().velocity;
        Quaternion cameraRotation = transform.rotation;
        if (velocity != Vector3.zero)
        {
            cameraRotation.SetLookRotation(velocity);
        }
        transform.rotation = cameraRotation;
    }
}
