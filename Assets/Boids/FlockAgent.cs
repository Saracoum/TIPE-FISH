using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{

    Collider2D agentcollider;
    public Collider2D AgentCollider { 
        get { 
            return agentcollider; 
        } 
    }


    // Start is called before the first frame update
    void Start()
    {
        agentcollider = GetComponent<Collider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
