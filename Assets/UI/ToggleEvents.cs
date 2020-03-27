using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleEvents : MonoBehaviour
{
    
    private Toggle toggle;
    
    public UnityEvent onValueOn;
    public UnityEvent onValueOff;
    
    
    private void Start() {
        toggle = GetComponent<Toggle>();
    }
    
    public void Trigger() {
        print(toggle.isOn);
        (toggle.isOn ? onValueOn : onValueOff ).Invoke();
    }
    
}
