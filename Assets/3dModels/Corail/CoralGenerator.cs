using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
Petit script qui ajoute des coraux aleatoirement autour d'un objet.
*/
public class CoralGenerator : LifeManager
{

    public GameObject coralPrefab;
    public Material aliveMat;
    public Material deadMat;
    
    public int count = 10;
    [Range(0, 5)]
    public float radius = 1;
    [Range(0, 90)]
    public float maxAngle = 20;
    [Range(0, 90)]
    public float maxRandomAngle = 10;
    public float offset = 0;
    [Range(0, 1)]
    public float sizeSpan = 0.5f;


    public void GenerateCoral( float health )
    {
        RemoveChildren();
        
        //int realCount = (int)(count * health);
        for (int i = 0; i < count; i++)
        {
            InstantiateCoral( Random.value < health );
        }
    }

    private void InstantiateCoral( bool alive )
    {
        GameObject coral = Instantiate(coralPrefab, transform.position, transform.rotation);
        coral.transform.SetParent(transform);

        ///////////////////
        // POSITION
        ///////////////////

        //trouve une position aleatoire dans une cercle du rayon specifie
        Vector2 pos2D = Random.insideUnitCircle * radius;
        Vector3 pos3D = new Vector3(pos2D.x, -offset, pos2D.y);

        //lance un rayon vers le bas pour connaitre la position du sol
        RaycastHit hit;
        if (Physics.Raycast(transform.TransformPoint(pos3D), -transform.up, out hit, 10))
        {
            pos3D.y += transform.InverseTransformPoint(hit.point).y;
        }

        coral.transform.localPosition = pos3D;


        ///////////////////
        // ROTATION
        ///////////////////
        //trouve la rotation en fonction de la distance au centre de l'objet
        Quaternion rot = Quaternion.AngleAxis(maxAngle * pos2D.magnitude / radius, Vector3.Cross(-pos3D, Vector3.up));

        //applique la rotation et ajoute une variation aleatoire
        coral.transform.localRotation = Quaternion.Lerp(rot, Random.rotationUniform, maxRandomAngle / 180);
        coral.transform.Rotate(Vector3.up * 360 * Random.value);


        ///////////////////
        // TAILLE
        ///////////////////
        coral.transform.localScale = Vector3.one * Random.Range(1 - sizeSpan, 1 + sizeSpan);
        
        //Material
        Material nextMat = (alive ? aliveMat : deadMat);
        if ( nextMat != null ) {
            coral.GetComponent<Renderer>().material = nextMat;
        }
    }

    public void RemoveChildren()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
    
    
    //DEBUG
    public UIEditableCurve displayCurve = null;
    
    override public void CreateLife( State currentState )
    {
        Curve growth = (currentState.TempCurve - 25);
        growth = growth.Squared() * -1 * 10;
        Curve population = growth.GetPrimitive() + count;
        
        float health = population.Get(currentState.time) / count;
        GenerateCoral(health);
        
        
        if (displayCurve != null) {
            Curve disp = population / 60;
            displayCurve.CurrentCurve = disp;
        }
    }
    
}
