using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{

    //GameObject indoor;

    AudioSource music;
    AudioSource ocean;
    bool indoors = false;
    // Start is called before the first frame update
    void Start()
    {

        //indoor = transform.GetChild(0).gameObject;
        music = GetComponent<AudioSource>();
        ocean = GameObject.Find("Beach").GetComponent<AudioSource>();
    }

    
    void FixedUpdate()
    {
        if(indoors == false)
        {
            ocean.priority = 0;
            music.priority = 1;

            while(ocean.volume < .75f)
            {
                ocean.volume += 0.1f;
            }
        } else 
        {
            music.priority = 0;
            ocean.priority = 1;
            
            while(ocean.volume > .25f)
            {
                ocean.volume -= 0.1f;
            }
            
        }
        
    }

    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("Collision with " + collider.name);
        if(collider.name == "Player" || collider.name == "Player(Clone)")
        {

            indoors = true;

        }

        
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.name == "Player" || collider.name == "Player(Clone)")
        {

            indoors = false;

        }
    }

    
}
