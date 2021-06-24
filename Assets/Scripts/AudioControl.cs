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
    private int soundPass = 1<<8;
    private Vector3 objPos;
    private Collider backBounds;
    private Vector3 pPos;
    public GameObject player;
    [SerializeField] private GameObject oceanWall;
    // Start is called before the first frame update
    void Start()
    {
        soundPass = ~soundPass;
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

        objPos = backBounds.ClosestPointOnBounds(oceanWall.transform.position);
        Debug.DrawRay(pPos, new Vector3(-100,0,0), Color.green);

        if(Physics.Raycast(pPos, new Vector3(-100,0,0), out earshot, 100, soundPass))
        {
            if(earshot.transform.gameObject.tag == "OpenOcean")
            {

            indoors = false;
            //Debug.Log("ocean heard");
            }

        } else 
    {
        indoors = true;
        //Debug.Log("Ocean blocked");
    }

        if(indoors == false)
        {
            ocean.priority = 0;
            music.priority = 1;
            ocean.volume = 0.774f;


            
        } else 
        {
            music.priority = 0;
            ocean.priority = 1;
            ocean.volume = 0.1f;
            
            
            
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
