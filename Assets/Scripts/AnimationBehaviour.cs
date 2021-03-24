using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;


namespace Conference.Characters
{
    namespace Conference.Scoreboards
    {
        public class AnimationBehaviour : MonoBehaviour
        {

            public GameObject face;

            private SpriteRenderer currentFace;
            private int layerMask = 1 << 8;
           // private Animation faceAnim;
           // private Animator faceState;
            private GameObject player;
            private NPCScript convoPartner;
           
            // Start is called before the first frame update
            void Start()
            {
                currentFace = face.GetComponent<SpriteRenderer>();
                layerMask = ~layerMask;
                //faceState = face.GetComponent<Animator>();
                //faceAnim = face.GetComponent<Animation>();
                //faceAnim["TalkingFace"].wrapMode = WrapMode.PingPong;
               // faceAnim.playAutomatically = false;
                player = GameObject.FindWithTag("Player");
                convoPartner = GetComponent<NPCScript>();

            }

            // Update is called once per frame
            void Update()
            {

                

               // faceState.SetBool("Conversing", convoPartner.animPlay);

               // Debug.Log(faceState.GetBool("Conversing"));
                
                
                
                

            }

            public Sprite FaceMaker()
            {

                return convoPartner.faces[new System.Random().Next(convoPartner.faces.Length - 1)];

            }
        }
    }
}
