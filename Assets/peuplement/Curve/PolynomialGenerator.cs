using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PolynomialGenerator
{
    Polynomial CreatePoly( List<Vector2> keys, int keyId );
}
