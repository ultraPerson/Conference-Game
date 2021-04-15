using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using Scoreboards;

using Menus;


namespace Characters
{
    
       
        public class AnimationBehaviour : MonoBehaviour
        {

            //public GameObject face;

            //private SpriteRenderer currentFace;
            private int layerMask = 1 << 8;
            private Animator animator;
            private Animation animationsTake;
            private GameObject player;
            private NPCScript convoPartner;

            private bool met;
            private bool conversing;
           
            // Start is called before the first frame update
            void Start()
            {
                animator = transform.GetChild(1).gameObject.GetComponent<Animator>();
                animationsTake = transform.GetChild(1).gameObject.GetComponent<Animation>();
                //AddClip(animationsTake, "handshake", 109, 249, true);
                //currentFace = face.GetComponent<SpriteRenderer>();
                layerMask = ~layerMask;
                //faceState = face.GetComponent<Animator>();
                //faceAnim = face.GetComponent<Animation>();
                //faceAnim["TalkingFace"].wrapMode = WrapMode.PingPong;
               // faceAnim.playAutomatically = false;
                player = GameObject.FindWithTag("Player");
                convoPartner = this.GetComponent<NPCScript>();

            }

            // Update is called once per frame
            void Update()
            {
                if(met != convoPartner.met)
                {
                met = convoPartner.met;
                Debug.Log("Met set to " + met);
                }

                if(conversing != convoPartner.animPlay)
                {
                conversing = convoPartner.animPlay;
                Debug.Log("conversing set to " + conversing);
                }

                animator.SetBool("Met", met);
                animator.SetBool("Conversation", conversing);

               /* if(conversing)
                {
                    if(met)
                    {
                        animator.Play("Talking");
                    } else
                    {
                        animator.Play("Handshake");
                        //animator.PlayQueued("Talking");
                    }
                }*/

                

               // faceState.SetBool("Conversing", convoPartner.animPlay);

               // Debug.Log(faceState.GetBool("Conversing"));
                
                
                
                

            }
/*
            public Sprite FaceMaker()
            {

                return convoPartner.faces[new System.Random().Next(convoPartner.faces.Length - 1)];

            }*/
        }
    
}

