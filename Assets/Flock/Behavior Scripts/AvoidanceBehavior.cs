using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FilteredFlockBehavior
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //Si y'a pas de voisins, alors on change rien
        if (context.Count == 0)
            return Vector3.zero;

        //on fait la moyenne des ptits potes
        Vector3 avoidanceMove = Vector3.zero;
        // int nAvoid = 0;
        float nAvoid = 0;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            float distRatio = Vector3.SqrMagnitude(item.position - agent.transform.position) / flock.SquareAvoidanceRadius; 
            if (distRatio < 1)
            {
                nAvoid++;
                avoidanceMove += agent.transform.position - item.position;
            }
        }
        if (nAvoid > 0)
            avoidanceMove /= nAvoid;

        return avoidanceMove;

    }
}
