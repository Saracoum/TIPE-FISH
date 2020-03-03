using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearCurve : Curve
{
    
    private SortedList<float, float> keyLst;
    
    public float Get(float x) {
        if ( keyLst.Count == 0 ) {
            return 0;
        }
        
        int i = 0;
        while( i<keyLst.Count-1 && keyLst.Keys[i+1] < x ) {
            i++;
        }
        float a = (keyLst.Values[i+1] - keyLst.Values[i] ) / (keyLst.Keys[i+1] - keyLst.Keys[i]);
        float b = keyLst.Values[i] - a * keyLst.Keys[i];
        return a*x + b;
    }
    
    public void AddKey(float x, float y){
        if ( !keyLst.ContainsKey(x) ) {
            keyLst.Remove(x);
        }
        keyLst.Add(x, y);
    }
    
    public List<Vector2> GetKeyList(){
        List<Vector2> lst = new List<Vector2>();
        foreach( float x in keyLst.Keys ) {
            lst.Add( new Vector2(x, keyLst[x]) );
        }
        return lst;
    }
    
    public void RemoveKey(int keyId){
        if ( keyId < keyLst.Count && keyId >= 0 ) {
            keyLst.RemoveAt(keyId);
        }
    }
    
    public LinearCurve() {
        keyLst = new SortedList<float, float>();
    }
    
}
