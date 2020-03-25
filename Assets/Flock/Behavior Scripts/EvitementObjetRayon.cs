using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/EvitementDeMathieu")]
public class EvitementObjetRayon : FlockBihavior
{
    [Range(0, 5)]
    public float distance = 1f;
    [Range(0,0.5f)]
    public float fishSize = 0.2f;
    
    public LayerMask masque;

    //Projete un rayon devant la poisscaille a x metre, et regarde si ca traverse un objet, et ca change la direction du poisson en fct
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector3 mouvementDevitement = Vector3.zero;
        RaycastHit hit;

        /*if (Physics.Raycast(agent.transform.position, agent.transform.forward, out hit, distance, masque))
        {
            mouvementDevitement = hit.normal * (1-hit.distance/distance);
        }*/
        if (Physics.SphereCast(agent.transform.position, fishSize, agent.transform.forward, out hit, distance, masque))
        {
            mouvementDevitement = hit.normal * (1-hit.distance/distance);
        }
        
        return mouvementDevitement;

    }
}
