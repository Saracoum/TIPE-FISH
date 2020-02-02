using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Stay in radius")]
public class StayInRadiusBehavior : FlockBihavior
{
    public Vector3 center;
    public float radius = 15f;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector3 centeroffset = center - (Vector3) agent.transform.position;
        float t = centeroffset.magnitude / radius;
        if (t < 0.9f)
        {
            return Vector3.zero;
        }

        return centeroffset * t * t;
    }

}
