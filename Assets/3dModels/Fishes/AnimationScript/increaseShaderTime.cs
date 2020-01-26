using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class increaseShaderTime : MonoBehaviour
{
    
    [SerializeField]
    private string propertyName = "_Phase";
    
    private Material mat;
    
    private void Start() {
        mat = GetComponent<Renderer>().material;
    }
    
    private void Update() {
        mat.SetFloat(propertyName, mat.GetFloat(propertyName) + Time.deltaTime);
    }
    
    
}
