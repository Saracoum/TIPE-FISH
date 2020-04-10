using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderRemoveButton : MonoBehaviour
{
    
    private float maxMouseDistSqr;
    
    private Image img;
    private Button button;
    
    
    [SerializeField] [Range(0,0.2f)]
    private float _maxMouseDist;
    public float MaxMouseDist {
        get{ return _maxMouseDist; }
        set{ 
            _maxMouseDist = value;
            maxMouseDistSqr = _maxMouseDist * _maxMouseDist;
        }
    }
    
    
    private void Awake() {
        img = GetComponent<Image>();
        button = GetComponent<Button>();
        OnValidate();
    }

    private void Update()
    {
        
        Vector2 mouseScreenPos = Input.mousePosition;
        
        Vector2 buttonSreenPos = new Vector2( transform.position.x, transform.position.y );
        
        
        bool enabled = (mouseScreenPos - buttonSreenPos).SqrMagnitude() < maxMouseDistSqr * Screen.width * Screen.width;
        img.enabled = enabled;
        button.enabled = enabled;
        
    }
    
    
    private void OnValidate() {
        MaxMouseDist = _maxMouseDist;
    }
}
