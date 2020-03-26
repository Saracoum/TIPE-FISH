using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    CharacterController characterController;

    [Range(0f, 10f)]
    public float speed = 1.0f;
    [Range(0f, 360f)]
    public float rotSpeed = 90.0f; // rotate at 90 degrees/second

    private Vector3 moveDirection = Vector3.zero;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime, 0, relativeTo:Space.World);
        transform.Rotate(Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime, 0, 0, relativeTo: Space.Self);
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= speed;


        characterController.Move(moveDirection * Time.deltaTime);

    }
}
