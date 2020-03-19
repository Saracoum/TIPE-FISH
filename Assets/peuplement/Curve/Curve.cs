using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Curve
{

    protected List<Polynomial> polyLst = new List<Polynomial>();
    protected List<Vector2> keys = new List<Vector2>();
    
    public PolynomialGenerator polyGen = new LinearPolyGenerator();
    
    //évaluter la courbe en x
    public float Get(float x)
    {
        if ( keys.Count == 0 ) {
            return 0;
        }
        
        int id = GetRangeId(x);
        if ( id == -2 ) {
            return keys[0].y;
        } else if ( id == -1 ) {
            return keys[keys.Count-1].y;
        }
        return (id < 0) ? 0 : polyLst[id].Get(x);
    }
    
    ////////////////////////////////////////////
    //opérations sur les clés
    ////////////////////////////////////////////
    
    //ajouter une clé
    public void AddKey(float x, float y)
    {
        AddKey(new Vector2(x, y));
    }

    public void AddKey(Vector2 key)
    {
        int insertId = GetRangeId(key.x);
        if (insertId == -1)
        {
            keys.Add(key);
            insertId = keys.Count - 1;
        }
        else
        {
            if (insertId == -2)
            {
                insertId = 0;
            }
            else
            {
                insertId++;
            }
            keys.Insert(insertId, key);
        }

        if (insertId > 0)
        {
            polyLst.Insert(insertId - 1, polyGen.CreatePoly(keys, insertId-1));
        }
        if (insertId + 1 < keys.Count)
        {
            if ( insertId == 0 ) {
                polyLst.Insert( 0, polyGen.CreatePoly(keys, insertId));
            } else {
                polyLst[insertId] = polyGen.CreatePoly(keys, insertId);
            }
        }
    }

    //récupère les clé.
    public List<Vector2> GetKeys()
    {
        return keys;
    }

    
    //modifier la clé a l'id spécifié
    public void ChangeKeyById(int id, float x, float y)
    {
        ChangeKeyById(id, new Vector2(x, y));
    }

    public void ChangeKeyById(int id, Vector2 key)
    {
        if (id >= 0 && id < keys.Count && (id==0 || key.x >= keys[id-1].x) && (id==keys.Count-1 || key.x <= keys[id+1].x) )
        {
            keys[id] = key;
            if (id + 1 < keys.Count)
            {
                polyLst[id] = polyGen.CreatePoly(keys, id);
            }
            if (id > 0)
            {
                polyLst[id - 1] = polyGen.CreatePoly(keys, id - 1);
            }

        }
    }
    
    //supprimer une clé
    public bool RemoveKeyAt( int id ) {
        if ( id < 0 || id >= keys.Count  ) {
            return false;
        }
        
        //suppression de la clé
        keys.RemoveAt(id);
        
        //suppression d'un polynome a l'indice de la clé et création d'un nouveau polynome avant
        if ( polyLst.Count != 0 ) {
            polyLst.RemoveAt( (id != polyLst.Count) ? id : id-1 );
        }
        if ( id != 0 && id != keys.Count ) {
            polyLst[id-1] = polyGen.CreatePoly(keys, id-1);
        }
        
        
        
        return true;
    }

    //retourne l'id du polynome qui doit être évalué en x
    //-1 si x est suppérieur a la dernière clé
    //-2 si x est inférieur a la première clé, ou qu'il n'y a aucun polynome
    private int GetRangeId(float x)
    {
        if (keys.Count <= 1 || x > keys[keys.Count - 1].x)
        {
            return -1;
        }
        else if (x < keys[0].x)
        {
            return -2;
        }

        int i = 0;
        while (keys[i + 1].x < x)
        {
            i++;
        }
        return i;
    }
    
    
    //////////////////////////////
    //opération de base
    //////////////////////////////
    
    //addition
    public static Curve operator +( Curve curve, float f ) {
        Curve clone = curve.Clone();
        clone.Add(f);
        return clone;
    }
    
    public void Add(float f) {
        for( int i=0; i<keys.Count; i++ ) {
            keys[i] = new Vector2( keys[i].x, keys[i].y + f );
            
        }
        for ( int i = 0; i<polyLst.Count; i++) {
            polyLst[i].Add(f);
        }
    }
    
    //soustraction
    public static Curve operator -(Curve curve, float f) {
        return curve + (-f);
    }
    public void Sub( float f ) {
        Add( -f );
    }
    
    //multiplication
    public static Curve operator *( Curve curve, float f ) {
        Curve clone = curve.Clone();
        clone.Mul(f);
        return clone;
    }
    
    public void Mul(float f) {
        for( int i=0; i<keys.Count; i++ ) {
            keys[i] = new Vector2( keys[i].x, keys[i].y * f );
        }
        for( int i=0; i<polyLst.Count; i++ ) {
            polyLst[i].Mul(f);
        }
    }
    
    //division
    public static Curve operator / ( Curve curve, float f ) {
        return curve * (1.0f/f);
    }
    
    //multiplication sur x
    
    
    //mise au carré
    public Curve Squared() {
        Curve squaredCurve = this.Clone();
        squaredCurve.Square();
        return squaredCurve;
    }
    
    public void Square() {
        for( int i=0; i<keys.Count; i++ ) {
            keys[i] = new Vector2( keys[i].x, keys[i].y * keys[i].y );
        }
        for( int i=0; i<polyLst.Count; i++ ) {
            polyLst[i] = polyLst[i].Squared();
        }
    }
    
    
    //////////////////////////////////////////
    // dérivation et intégration
    //////////////////////////////////////////
    
    //retourne une nouvelle courbe, qui est la primitive de l'ancienne courbe qui passe par (0,0)
    public Curve GetPrimitive() {
        List<Vector2> keysPrim = new List<Vector2>();
        List<Polynomial> polyLstPrim = new List<Polynomial>();
        if ( keys.Count != 0 ) {
            keysPrim.Add(Vector2.zero);
        }
        
        for ( int i=0; i<polyLst.Count; i++ ) {
            Polynomial newPoly = polyLst[i].GetPrimitive();
            newPoly.ChangeHeight( keysPrim[i] );
            float x = keys[i+1].x;
            
            polyLstPrim.Add(newPoly);
            keysPrim.Add( new Vector2( x, newPoly.Get(x) ) );
        }
        return new Curve(keysPrim, polyLstPrim, polyGen);
    }
    
    
    
    
    
    
    //Clone
    public Curve Clone() {
        List<Vector2> cloneKeys = new List<Vector2>( keys );
        List<Polynomial> clonePoly = new List<Polynomial>();
        foreach ( Polynomial poly in polyLst) {
            clonePoly.Add( poly.Clone() );
        }
        return new Curve( cloneKeys, clonePoly, polyGen );
    }
    
    
    
    //constructeurs
    public Curve() : this(null) { }


    public Curve(List<Vector2> keys)
    {
        this.keys = new List<Vector2>();
        if (keys != null)
        {
            foreach (Vector2 key in keys)
            {
                AddKey(key);
            }
        }
    }
    
    //un constructeur privé qui ne fait pas de vérif sur les données d'entrée
    protected Curve( List<Vector2> keys, List<Polynomial> polyLst, PolynomialGenerator polyGen ) {
        this.keys = keys;
        this.polyLst = polyLst;
        this.polyGen = polyGen;
    }

}