using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
   [SerializeField] 
    private Slider slider;
   [SerializeField] 
    Transform target;
   [SerializeField] 
    private Camera Camera;
   [SerializeField]
    private Vector3 offset;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        //slider.value = currentValue / maxValue;
    }

    public void Update()
    {
        
    }

}
