using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fishAnim : MonoBehaviour
{
    
    public float speed = 0;
    public float maxBendAngle = 30;
    public float maxXtranslation = 0.1f;
    public float fishSize = 0.38f;
    public float period = 0.4f;
    
    private float tps = 0;
    private float bendAngle = 0;
    
    private Mesh mesh;
    private Vector3[] verticesBasis;
    private Vector3[] verticesBend;
    
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        
        verticesBasis = mesh.vertices;
        verticesBend = new Vector3[verticesBasis.Length];
    }

    
    void Update()
    {
        tps += Time.deltaTime;
        
        for ( int i=0; i<verticesBasis.Length; i++ ) {
            verticesBend[i] = verticesBasis[i];
            
            
            //rotation
            bendAngle = Mathf.Deg2Rad*maxBendAngle * Mathf.Sin( speed * (tps - verticesBasis[i].y/period) * 2*Mathf.PI );
            bendAngle *= verticesBasis[i].sqrMagnitude / (fishSize * fishSize);
            
            float cos = Mathf.Cos( bendAngle );
            float sin = Mathf.Sin( bendAngle );
            verticesBend[i].x = verticesBasis[i].x * cos - verticesBasis[i].y * sin;
            verticesBend[i].y = verticesBasis[i].x * sin + verticesBasis[i].y * cos;
            
            
            //translation
            verticesBend[i].x += maxXtranslation * Mathf.Cos( speed * tps * 2*Mathf.PI );
        }
        
        mesh.vertices = verticesBend;
        mesh.RecalculateNormals();
    }
}
