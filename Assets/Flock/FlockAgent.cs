using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{

    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    Collider agentcollider;
    //Rigidbody rb;
    public Collider AgentCollider { 
        get { 
            return agentcollider; 
        } 
    }
    
    private float speed = 1.0f;
    private Vector3 currentVelocity = Vector3.zero;
    
    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    // Start is called before the first frame update
    void Start()
    {
        agentcollider = GetComponent<Collider>();
        //rb = GetComponent<Rigidbody>();
        
    }

    public void Move(Vector3 velocity)
    {
        currentVelocity = Vector3.Lerp( currentVelocity, velocity, speed * Time.deltaTime );
        
        transform.forward = currentVelocity;
        transform.position += (Vector3) currentVelocity * Time.deltaTime;
        //rb.AddForce(velocity, ForceMode.VelocityChange);
    }

}
