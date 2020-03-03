using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLifeManager : MonoBehaviour
{
    
    [Range(0,1)]
    public float globalHealth = 1.0f;
    
    
    private List<LifeManager> genLst;
    
    public void Start() {
        genLst = new List<LifeManager>( GetComponentsInChildren<LifeManager>() );
        
        InitLife(globalHealth);
    }
    
    
    
    public void InitLife ( float health ) {
        
        foreach ( LifeManager gen in genLst ) {
            gen.CreateLife(health);
        }
    }
    
    
}
