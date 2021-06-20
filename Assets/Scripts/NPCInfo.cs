using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Characters{
public class NPCInfo : MonoBehaviour
{

    //Image textBox
    public GameObject player;
    [SerializeField]
    private GameObject textBox;
    private Transform npcTransform;
    private NPCScript npcScript;
    //string character name = npcScript name
    private string npcName;
    //bool met = npc script met
    private bool met;
    bool playerLooking;
    public bool playerNear
    {get; private set;}
    //Vector3 eyeContact = where TPVisor raycast intersects capsule collider
    private Vector3 eyeContact;
    
    
    void Start()
    {
        player = GameObject.Find("Player");
        npcTransform = GetComponentInParent(typeof(Transform)) as Transform;
        npcScript = GetComponentInParent(typeof(NPCScript)) as NPCScript;
        npcName = npcScript.nPCName;


    }
    void Update()
    {

        //if(Physics.Raycast(npcTransform.GetChild(0).position, player.transform.position, 50f))
        //if playerLooking
        //run TextPlacingFunction
    }


    void TextPlacingFunction()
    {
        // - activate canvas
        // - populate text box with name
        // - if met, add that to the text
        // - place textBox at eyeContact
        // - rotate textBox to face player
    }
   
}
}
