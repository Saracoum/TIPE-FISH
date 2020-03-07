using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Une implémentation de Curve qui modélise une fonction linéaire
*/
public class LinearCurve : Curve
{
    
    //créé un polynome de degré 1 qui passe par les clé d'identifiant id et id+1 si ils existent
    public override Polynomial CreatePoly( int id ){
        
            return (id>=0 && id+1 < keys.Count) ? new Polynomial( keys[id], keys[id+1] ) : new Polynomial();
    }
    
    public LinearCurve() : base() {}
    
    public LinearCurve( List<Vector2> keys ) : base(keys) {}
    
}
