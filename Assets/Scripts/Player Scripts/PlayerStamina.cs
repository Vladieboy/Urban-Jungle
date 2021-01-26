using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float stamina = 100f;
    [SerializeField] float depletionRate = 2f;

    bool reduceStaminaNaturally = true;
    bool _initialRun = true;

    public SliderScript slider;
    // Start is called before the first frame update
    void Start()
    {
        slider.SetMaxValue(maxStamina);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(reduceStaminaNaturally) StartCoroutine("DepleteStamina");
    }

    public void ReduceStaminaBy(float value)
    {
        
        slider.SetValue(stamina, stamina -= value);
    }

    IEnumerator DepleteStamina()
    {
        reduceStaminaNaturally = false;
        if(!_initialRun) ReduceStaminaBy(10);
        yield return new WaitForSeconds(depletionRate);
        reduceStaminaNaturally = true;
        _initialRun = false;
    }
}
