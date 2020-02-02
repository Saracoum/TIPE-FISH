using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Cohesion")]
public class CohesionBehavior : FilteredFlockBehavior
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //Si y'a pas de voisins, alors on change rien
        if (context.Count == 0)
            return Vector3.zero;

        //on fait la moyenne des ptits potes
        Vector3 cohesionMove = Vector3.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            cohesionMove += (Vector3) item.position;
        }
        cohesionMove /= context.Count;

        //on decale par rapport a la position de l'agent
        cohesionMove -= (Vector3) agent.transform.position;
        return cohesionMove;

    }


}
