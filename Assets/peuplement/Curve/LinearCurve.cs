using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
Une implémentation de Curve qui modélise une fonction linéaire
*/
public class LinearCurve : Curve
{
    
    //créé un polynome de degré 1 qui passe par les clé d'identifiant id et id+1 si ils existent
    public Polynomial CreatePoly( int id ){
        
            return (id>=0 && id+1 < keys.Count) ? new Polynomial( keys[id], keys[id+1] ) : new Polynomial();
    }

    /*public Curve Clone()
    {
        List<Vector2> cloneKeys = new List<Vector2>( keys );
        List<Polynomial> clonePoly = new List<Polynomial>();
        foreach ( Polynomial poly in polyLst) {
            clonePoly.Add( poly.Clone() );
        }
        return new LinearCurve( cloneKeys, clonePoly );
    }*/

    public LinearCurve() : base() {}
    
    public LinearCurve( List<Vector2> keys ) : base(keys) {}
    
    // private LinearCurve( List<Vector2> keys, List<Polynomial> polyLst ) : base(keys,polyLst) {}
}
