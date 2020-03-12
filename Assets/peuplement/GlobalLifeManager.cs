using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLifeManager : MonoBehaviour
{
    
    // [Range(0,1)]
    // public float globalHealth = 1.0f;
    
    public UIEditableCurve uiTempCurve;
    
    
    private List<LifeManager> genLst;
    
    public void Start() {
        genLst = new List<LifeManager>( GetComponentsInChildren<LifeManager>() );
        
        
        InitLife();
    }
    
    
    
    public void InitLife () {
        
        State state = new State(uiTempCurve.CurrentCurve);
        
        if ( uiTempCurve != null ) {
            foreach ( LifeManager gen in genLst ) {
                gen.CreateLife( state );
            }
        }
    }
    
    
}
