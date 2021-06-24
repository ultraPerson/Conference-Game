using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Characters;

namespace UIDecor{

public class UIDecor : MonoBehaviour
{

    [SerializeField] private Image tScreen;
    [SerializeField] private Image tBorder;
    [SerializeField] private AudioSource onSound;
    [SerializeField] private AudioSource offSound;
    private float currentAlpha;
    private float targetAlpha;
    private bool powerOn;
    public float currentPower
    {get; private set;}
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    //    targetAlpha = Mathf.Clamp(targetAlpha, 0f, 1f);
    //    tBorder.color.a = targetAlpha;
    //    //currentAlpha = tBorder.tintColor.a;

    //     if(powerOn)
    //     {
    //         targetAlpha += 0.1f;
    //         if(targetAlpha == 1)
    //         {
    //             currentPower = true;
    //         }

    //     } else 
    //     {
    //         targetAlpha -= 0.1f;
    //         if(targetAlpha == 0)
    //         {
    //             currentPower = false;
    //         }
    //     }
        


        
    }


    public void PowerToggle(bool power)
    {
        powerOn = power;

    }

    
}//class
}//namespace