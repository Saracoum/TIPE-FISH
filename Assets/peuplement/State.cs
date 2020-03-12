using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    
    private Curve _tempCurve = new LinearCurve();
    public Curve TempCurve {
        get{ return _tempCurve; }
    }
    public float time;
    
    public State( Curve tempCurve) : this(tempCurve, 0) {}
    
    public State( Curve tempCurve, float time ) {
        _tempCurve = tempCurve;
        this.time = time;
    }
    
}
