using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    
    public List<GameObject> UIElements;
    public Control playerController;
    
    [SerializeField]
    private bool _pause = false;
    public bool Pause {
        get { return _pause; }
        set {
            _pause = value;
            foreach ( GameObject element in UIElements ) {
                element.SetActive(_pause);
            }
            playerController.enabled = !_pause;
        }
    }
    
    [SerializeField]
    private KeyCode pauseKey = KeyCode.Escape;
    
    
    public void Start() {
        Pause = false;
    }
    
    private void Update() {
        if ( Input.GetKeyDown(pauseKey) ) {
            Pause = !Pause;
        }
    }
    
    
    private void OnValidate() {
        //Pause = _pause;
    }
    
}
