using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Spin : MonoBehaviour
{

    
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;
    private System.Random offset = new System.Random();
    private int timeOffset;
    private bool waiting = true;
    private int upDown;


    private Vector3 startPosition;


    
    
 
    void Start()
    {

        
        startPosition = transform.position;
        timeOffset = offset.Next(0,1001);

        int[] possibilities = {-1, 1};
        upDown = possibilities[offset.Next(-1, 2)];
    
        
        StartCoroutine(HoldOnASec());
        
      

 
        
    }

   
    void Update()
    {
        

        if(!waiting)
        {
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.Self);
        
        Vector3 tempPos = startPosition;
        tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * (amplitude +(timeOffset/1000)) * upDown;
        transform.position = tempPos;
        }

    }

    IEnumerator HoldOnASec()
    {
        yield return new WaitForSeconds(timeOffset/1000);
        waiting = false;
    }

    

    
}
