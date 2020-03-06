using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Curve
{
    
    private List<Polynome> polyLst = new List<Polynome>();
    private List<Vector2> keys = new List<Vector2>();
    
    public float Get( float x ) {
        
        int id = GetRangeId(x);
        return (id < 0) ? 0 : polyLst[i].get(x);
    }
    
    
    public void AddKey( float x, float y ) {
        AddKey( key.x, key.y );
    }
    
    public void AddKey(Vector2 key) {
        int insertId = GetRangeId(key.x);
        if ( insertId == -1 ) {
            keys.Add( key );
            insertId = keys.Count-1;
        } else {
            if( insertId == -2 ) {
                insertId = 0;
            }
            keys.Insert(insertId, key);
        }
        
        if (insertId-1 > 0) {
            polyLst.Insert( insertId-1, CreatePoly(insertId-1) );
        }
        if ( insertId+1 < keys.Count ) {
            polyLst.Insert( insertId, CreatePoly(insertId) );
        }
    }
    
    
    private abstract Polynome CreatePoly( int id );
    
    
    private int GetRangeId( float x) {
        if (parts.Count == 0 || x > parts[parts.Count].x ) {
            return -1;
        } else if ( x < parts[0].x ) {
            return -2;
        }
        
        int i=0;
        while( parts[i+1].x < x ) {
            i++;
        }
        return i;
    }
    
}
