using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float horizontalRotationSpeed = 20f;
    [SerializeField] private float verticalMoveSpeed = .4f;

    [SerializeField] private Transform cam;

    private float horizontalMovement;
    private float verticalMovement;

    void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up, -horizontalMovement * horizontalRotationSpeed * Time.deltaTime);
        cam.transform.localPosition += Vector3.up * verticalMovement * verticalMoveSpeed * Time.deltaTime;

        cam.LookAt(transform.position);
    }
}
