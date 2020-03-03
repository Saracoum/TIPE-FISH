using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Curve
{
    
    float Get(float x);
    
    void AddKey(float x, float y);
    
    List<Vector2> GetKeyList();
    
    void RemoveKey(int keyId);
    
}
