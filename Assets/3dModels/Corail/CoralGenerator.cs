using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    
    public GameObject coralPrefab;
    
    public void generateCoral() {
        Instantiate( coralPrefab, transform.position, transform.rotation );
    }
}
