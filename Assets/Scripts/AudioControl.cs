using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Menus;

public class AudioControl : MonoBehaviour
{

    //GameObject indoor;

    AudioSource music;
    AudioSource ocean;
    bool indoors = false;
    public float maxVolume;
    private float targetVolume;
    private float dToP;
    private RaycastHit earshot;
    private Vector3 objPos;
    private Collider backBounds;
    private Vector3 pPos;
    public GameObject player;
    [SerializeField] private GameObject oceanWall;
    // Start is called before the first frame update
    void Start()
    {
        backBounds = oceanWall.GetComponent<Collider>();
        //indoor = transform.GetChild(0).gameObject;
        music = GetComponent<AudioSource>();
        ocean = GameObject.Find("Beach").GetComponent<AudioSource>();
        
        if(player == null)
        {
            player = GameObject.Find("Player(Clone)");
        }
         

    }

    
    void Update()
    {   
        
        if(player == null)
        {
            player = GameObject.Find("Player(Clone)");
        }

        pPos = player.transform.position;

        objPos = backBounds.  ClosestPointOnBounds(oceanWall.transform.position);

        if(Physics.Raycast(pPos, objPos, out earshot))
        {
            if(earshot.transform.gameObject.tag == "SoundBlocker")
            {

            indoors = true;
            Debug.Log("ocean blocked");
            }

        } else indoors = false;

        if(indoors == false)
        {
            ocean.priority = 0;
            music.priority = 1;

            
        } else 
        {
            music.priority = 0;
            ocean.priority = 1;
            
            
            
        }

        if(ocean.volume < .75f && ocean.priority == 0)
            {
                ocean.volume += 0.1f * Time.deltaTime;
            } else if(ocean.volume > .25f)
            {
                ocean.volume -= 1f * Time.deltaTime;
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
