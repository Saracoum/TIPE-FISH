using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFollow : MonoBehaviour {

    public Transform playerTransform;
    
    [SerializeField]
    private Vector3 camOffset = Vector3.zero;

    // [Range(0.01f, 1.0f)]
    // public float SmoothFacteur = 0.5f;
    [SerializeField]
    private float smoothSpeed = 1.0f;
    
    void Start()
    {
        // camOffset = transform.position - PlayerTransform.position;
    }
    
    
    void FixedUpdate()
    {
        Vector3 camGlobalOffset =  playerTransform.rotation * camOffset;
        
        Vector3 newPos = playerTransform.position + camGlobalOffset;
        
        // transform.position = Vector3.Lerp(transform.position, newPos, smoothSpeed * Time.deltaTime);
        
        float currentSpeed = smoothSpeed * (newPos - transform.position).magnitude;
        transform.position = Vector3.MoveTowards( transform.position, newPos, currentSpeed * Time.deltaTime );
        // transform.position = newPos;
        
        //rotation
        transform.rotation = playerTransform.rotation;
    }
}
