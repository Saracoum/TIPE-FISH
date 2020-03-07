using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class BoxSliderKey : BoxSlider
{
    
    public UIEditableCurve editableCurve;
    
    
    public void SendChanges() {
        if ( editableCurve != null ) {
            editableCurve.UpdateCurveKey(this);
        }
    }
    
}
