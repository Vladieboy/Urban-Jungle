using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    Slider slider;
   [SerializeField] GameObject numericalValue;

    private void Start()
    {
        slider = GetComponent<Slider>();
        
    }
    public void SetValue(float currentValue, float value)
    {
        slider.value = Mathf.MoveTowards(currentValue, value, 2f * Time.deltaTime);
        numericalValue.GetComponent<TextMeshProUGUI>().text = $"{value}/{slider.maxValue}";


    }

    private void Update()
    {
        slider.value = Mathf.MoveTowards(100, 0, 2f * Time.deltaTime);
    }

    public void SetMaxValue(float value)
    {
        slider.maxValue = value;
        slider.value = value;
        numericalValue.GetComponent<TextMeshProUGUI>().text = $"{slider.maxValue}/{slider.maxValue}";
    }


}
