﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/SteeredCohesion")]
public class SteeredCohesionBehavior : FilteredFlockBehavior
{
    Vector3 currentVelocity;
    public float agentSmoothTime = 0.5f;



    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //Si y'a pas de voisins, alors on change rien
        if (context.Count == 0)
            return Vector3.zero;

        //on fait la moyenne des ptits potes
        Vector3 cohesionMove = Vector3.zero;
        float weightSum = 0;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            float weight = 1 - ((item.position - agent.transform.position).sqrMagnitude / flock.SquareNeighborRadius);
            cohesionMove += (item.position - agent.transform.position).normalized * weight;
            weightSum += weight;
        }
        // cohesionMove /= context.Count;
        if ( weightSum != 0 ) {
            cohesionMove /= weightSum;
        }

        //on decale par rapport a la position de l'agent
        cohesionMove -= (Vector3)agent.transform.position;
        cohesionMove = Vector3.SmoothDamp(agent.transform.forward, cohesionMove, ref currentVelocity, agentSmoothTime);
        return cohesionMove;

    }


}
