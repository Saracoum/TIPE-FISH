﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBihavior : ScriptableObject
{
    public abstract Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock);

}
