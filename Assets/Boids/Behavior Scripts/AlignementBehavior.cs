using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignement")]
public class AlignementBehavior : FlockBihavior
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //Si y'a pas de voisins, alors on change rien
        if (context.Count == 0)
            return agent.transform.forward;

        //on fait la moyenne des ptits potes
        Vector3 alignementMove = Vector3.zero;
        foreach (Transform item in context)
        {
            alignementMove += item.transform.forward;
        }
        alignementMove /= context.Count;

        return alignementMove;

    }
}
