using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FlockAgent : MonoBehaviour
{

    Collider agentcollider;
    public Collider AgentCollider { 
        get { 
            return agentcollider; 
        } 
    }


    // Start is called before the first frame update
    void Start()
    {
        agentcollider = GetComponent<Collider>();
        
    }

    public void Move(Vector3 velocity)
    {
        transform.forward = velocity;
        transform.position += velocity * Time.deltaTime;
    }
}
