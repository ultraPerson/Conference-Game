using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{

    //GameObject indoor;

    AudioSource music;
    AudioSource ocean;
    // Start is called before the first frame update
    void Start()
    {

        //indoor = transform.GetChild(0).gameObject;
        music = GetComponent<AudioSource>();
        ocean = GameObject.Find("Beach").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Collision with " + collider.name);
        if(collider.name == "Player" || collider.name == "Player(Clone)")
        {

            music.priority = 0;
            ocean.priority = 1;
            
            while(music.minDistance < 12)
            {
                music.minDistance++;
            }
            while(ocean.maxDistance > 100)
            {
                ocean.maxDistance --;
                
            }

        }

        
    }

    void OnTriggerExit(Collider collider)
        {
            if(collider.name == "Player" || collider.name == "Player(Clone)")
        {

                ocean.priority = 0;
                music.priority = 1;
            
            while(ocean.maxDistance < 150)
            {
                ocean.maxDistance ++;
            }
            while(music.minDistance > 1)
            {
                music.minDistance--;
            }
                

        }
        }
}
