using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using TMPro;

namespace Characters{
public class NPCInfo : MonoBehaviour
{

    //Image textBox
    public GameObject player;
    [SerializeField]
    private GameObject textBox;
    private Transform npccTransform;
    private NPCScript npcScript;
    private RaycastHit looking;
    private RaycastHit rCH;

    //image for met
    [SerializeField]
    private Image metImg;
    //image for hasQuiz
    [SerializeField]
    private Image quizImg;
    
    // ParticleSystem qParticles;
    
    // ParticleSystem mParticles;
    
    // Light qLight;
    
    // Light mLight;
    //image for available
    [SerializeField]
    private Sprite available;
    [SerializeField]
    private Color availableCol;
    //Image for not available
    [SerializeField]
    private Sprite unavailable;
    [SerializeField]
    private Color unavailableCol;
    //image for in progress
    [SerializeField]
    private Sprite inProgress;
    [SerializeField]
    private Color inProgressCol;
    //image for complete
    [SerializeField]
    private Sprite complete;
    [SerializeField]
    private Color completeCol;
    
//points?


    //string character name = npcScript name
    private string npcName;
    //bool met = npc script met
    private bool met;
    private bool hasQuiz;
    public bool playerLooking;
    private bool textInUse;
    public bool playerNear
    {get; private set;}
    //Vector3 eyeContact = where TPVisor raycast intersects capsule collider
    private Vector3 eyeContact;
    
    
    void Start()
    {
       
        GetComponent<Canvas>().worldCamera = player.GetComponent<TPVisor>().cam;
        npccTransform = GetComponentInParent(typeof(Transform)) as Transform;
        // qParticles = quizImg.gameObject.GetComponent<ParticleSystem>();
        // mParticles = metImg.gameObject.GetComponent<ParticleSystem>();
        // qLight = quizImg.gameObject.GetComponent<Light>();
        // mLight = metImg.gameObject.GetComponent<Light>();
       
      


    }


    void FixedUpdate()
    {
        textInUse = player.GetComponent<TPVisor>().thirdPerson;
        
        if(textInUse)
        {
            if(!playerLooking)
            {
                CardClear();
            } else
            {
                textBox.transform.position = eyeContact;
                transform.LookAt(player.transform.GetChild(8).GetChild(1));
            }
        } else CardClear();
    }


    public void TextPlacingFunction(GameObject NPC, Vector3 hit)
    {

        npcScript = NPC.GetComponent<NPCScript>();
        met = npcScript.met;
        hasQuiz = npcScript.hasQuiz;
        npcName = npcScript.nPCName;
        playerLooking = true;
        
        eyeContact = hit;
        
        // var qEmission = qParticles.emission;
       
        // var mEmission = mParticles.emission;
        

        
        // - activate image
        textBox.transform.GetChild(0).gameObject.SetActive(true);
        textBox.transform.GetChild(1).gameObject.SetActive(true);
        textBox.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = npcName;
        
        // - populate text box with name
        
        
        if(met)
        {
            metImg.sprite = complete;
            metImg.color = completeCol;
            
        } else
        { 
            if(npcScript.dialoguePos >= 1)
        {
            metImg.sprite = inProgress;
            metImg.color = inProgressCol;
        } else {

            metImg.sprite = inProgress;
            metImg.color = Color.red;
        }

        } 

        if(hasQuiz)
        {

            if(npcScript.qLevel > 0 && npcScript.qLevel < npcScript.quizQuestions.Length - 1)
            {
                quizImg.sprite = inProgress;
                quizImg.color = inProgressCol;

            } else
            {
                quizImg.sprite = available;
                quizImg.color = availableCol;

            } 


        } else if(!hasQuiz && npcScript.qLevel > 0)
        {
            quizImg.sprite = complete;
            quizImg.color = completeCol;
        } else 
        {
            quizImg.sprite = unavailable;
            quizImg.color = unavailableCol;
        }

        
        
        
        
    }

    public void CardClear()
    {
        textBox.transform.GetChild(0).gameObject.SetActive(false);
        textBox.transform.GetChild(1).gameObject.SetActive(false);
        Debug.Log("CardClear");
    }
   
}
}
