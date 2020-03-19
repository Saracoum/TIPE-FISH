using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearPolyGenerator : PolynomialGenerator
{
    public Polynomial CreatePoly(List<Vector2> keys, int keyId)
    {
        if ( keyId < 0 || keyId+1 > keys.Count ) {
            return null;
        }
        
        return new Polynomial( keys[keyId], keys[keyId+1] );
    }
}