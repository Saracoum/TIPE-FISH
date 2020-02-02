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
        transform.forward = velocity;
        transform.position += (Vector3) velocity * Time.deltaTime;
        //rb.AddForce(velocity, ForceMode.VelocityChange);
    }

}
