using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLifeManager : MonoBehaviour
{
    
    public UIEditableCurve uiTempCurve;
    
    
    private List<LifeManager> genLst;
    
    public void Start() {
        FindGenLst();
        
        List<Vector2> tempKeyLst = new List<Vector2>( new Vector2[]{new Vector2(0,25), new Vector2(1,25)});
        uiTempCurve.CurrentCurve = new Curve( tempKeyLst );
        
        InitLife();
    }
    
    
    public void InitLife () {
        
        State state = new State(uiTempCurve.CurrentCurve, 1);
        
        if ( uiTempCurve != null ) {
            if ( genLst == null ) {
                FindGenLst();
            }
            foreach ( LifeManager gen in genLst ) {
                gen.CreateLife( state );
            }
        }
    }
    
    private void FindGenLst() {
        genLst = new List<LifeManager>( GetComponentsInChildren<LifeManager>() );
    }
    
    
}
