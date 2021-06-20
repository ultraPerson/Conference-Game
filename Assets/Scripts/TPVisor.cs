using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using Scoreboards;
using Menus;
using Mirror;

namespace Characters
{
    
        public class TPVisor : MonoBehaviour
        {

            /// <summary>
            /// This script controls the HUD, Raycast and click functionality
            /// as well as containing all information on the player
            /// including score and name
            /// </summary>

            //Canvas component
            Canvas visor;
            //text component beneath TBG(text background)
            Text vScan;
            //text component beneath PBG(points background)
            Text vPoints;
            //transform for TBG
            public RectTransform tBGTrans
            { get; private set; }
            //transform for PBG
            RectTransform pBGTrans;
            //transform for vScan
           public  RectTransform vTextTrans
            { get; private set; }
            //transform for vPoints
            RectTransform pTextTrans;

            public GameObject whatDefault;
            private GameObject vText;
            //text game object for points
            private GameObject vScore;
            [SerializeField]private GameObject playerCanvas;
            private GameObject tBG;
            //image game object for PBG
            private GameObject pBG;
            public int points = 0;
            public string playerName;

            private int layerMask = 1 << 8;
            //private float maxDist;
            public Ray lookAt
            {
                get;
                private set;
            }

            private Camera cam;
            private CinemachineFreeLook camControl;
            private GameObject scoreHolder;
            private bool seeingNew = false;
            private bool zoom = false;
            public float zoomRate;
            private float startingFOV;
            public float currentFOV;

            private Image retical;
            private Sprite targetEmpty;
            [SerializeField] private Sprite targetFull;
            
            private float screenW;
            private float screenH;
            //private float totalWidthT;
            //private float totalHeightT;
            private Vector2 textPos;
            private Vector2 pointPos;
            private RaycastHit seen;
            private GameObject what;
            private Text whatDesc;

            private GameObject[] allScores;
            public GameObject camTarget;
            private float zoomTarget = 0f;//have this change like the zoom amount, and have camTarget y positon set to this value in Update()

            //public GameObject chatUIPrefab;
            //private GameObject chatUIClone;
            //private ChatBehaviour chatBehaviour;
            [SerializeField] private PauseScript pauseScript;
            //private string savePath = $"{Application.}/highscores.json";
            // private RectTransform cTextRTrans;
            // private RectTransform cInputRTrans;
            public bool chatOpen
            { get; private set; }
            public bool isPaused = false;

            
            Scoreboard scoreboard;
            ScoreboardEntryData scoreData;

            // Start is called before the first frame update
            void Start()
            {
                
                screenW = Screen.width;
                screenH = Screen.height;
                

                if(string.IsNullOrEmpty(playerName))
                {
                    playerName = "New Player";
                }

                //GameObject.Find("NetworkManager").GetComponent<NetworkManagerHUD>().offsetX -= 300;

                //Cursor.lockState = CursorLockMode.Locked;
                

                //chatUIClone = Instantiate(chatUIPrefab);

                cam = Camera.main;
                retical = cam.gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>();
                targetEmpty = retical.sprite;
                camControl = GameObject.Find("TPCam").GetComponent<CinemachineFreeLook>();
                startingFOV = cam.fieldOfView;
                currentFOV = startingFOV;
                if(camControl != null){
                    Debug.Log("camControl set");
                }
                camControl.m_Follow = GameObject.Find("DollyCart1").transform;
                camControl.m_LookAt = GameObject.Find("MenuStart").transform;
                if(playerCanvas == null)
                {
                playerCanvas = GameObject.Find("Main Camera/VisorCanvas");
                }
                tBG = playerCanvas.transform.GetChild(1).gameObject;//GameObject.Find("/Main Camera/VisorCanvas/TextBG");
                pBG = playerCanvas.transform.GetChild(2).gameObject;//GameObject.Find("/Main Camera/VisorCanvas/ScoreBG");
                vText = tBG.transform.GetChild(0).gameObject;//GameObject.Find("/Main Camera/VisorCanvas/TextBG/VisorText");
                vScore = pBG.transform.GetChild(1).gameObject;//GameObject.Find("/Main Camera/VisorCanvas/ScoreBG/VisorScore");

                
                //pauseScript = GetComponent<PauseScript>();
                chatOpen = false;
                //chatBehaviour = GetComponent<ChatBehaviour>();
               // cTextRTrans = chatBehaviour.chatText.GetComponent<RectTransform>();
               // cInputRTrans = chatBehaviour.inputField.GetComponent < RectTransform > ();
                scoreHolder = pBG.transform.GetChild(0).gameObject;
                //maxDist = Mathf.Infinity;
                layerMask = ~layerMask;
                playerName = "New Player";
                scoreData.name = playerName;
                scoreData.score = points;


                scoreboard = playerCanvas.GetComponent<Scoreboard>();
                scoreboard.AddEntry(scoreData);
                tBGTrans = tBG.GetComponent<RectTransform>();
                pBGTrans = pBG.GetComponent<RectTransform>();
                vTextTrans = vText.GetComponent<RectTransform>();
                pTextTrans = vScore.GetComponent<RectTransform>();

                //cam = Camera.main;
                //visor = GetComponent < Canvas > ();
                vScan = vText.GetComponent<Text>();
                vPoints = vScore.GetComponent<Text>();
                //rectTransform = vScan.GetComponent<RectTransform>();
                
                //Vscan and TBG coordinates 
                textPos = new Vector2(0, 0);
                //vpoints and PBG coordinates
                pointPos = new Vector2(screenW, screenH);


                tBGTrans.position = textPos;
                
                pBGTrans.position = pointPos;
                pTextTrans.position = pBGTrans.position;
                pTextTrans.sizeDelta = new Vector2(pBGTrans.sizeDelta.x/2, pBGTrans.sizeDelta.y / 8);
                scoreHolder.transform.position = new Vector2(pointPos.x, pointPos.y - pBGTrans.sizeDelta.y);
                pointPos = new Vector2(screenW, screenH);


                tBGTrans.position = textPos;
                pBGTrans.sizeDelta = new Vector2(screenW / 6, screenH / 2);
                tBGTrans.sizeDelta = new Vector2(screenW, screenH / 3);
                vTextTrans.sizeDelta = new Vector2(screenW - 20, (screenH / 3) - 10);
                vTextTrans.position = new Vector2(10, 5);

                Debug.Log(Screen.currentResolution);
                //visor.renderMode = RenderMode.ScreenSpaceOverlay;


            }

            // Update is called once per frame
            void Update()

            {

                

                isPaused = pauseScript.isPaused;
                
                
                

                vPoints.text = playerName + " score: " + points.ToString();

                if(scoreData.score < points)
                {
                scoreData.score = points;
                }
                lookAt = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

                


                //playerCanvas.transform.position = cam.ScreenToWorldPoint(textPos);
                //Debug.DrawRay(cam.transform.position, Vector3.forward * 100, Color.green);
                



                if(zoom)
                {
                    if(tBG.activeInHierarchy)
                    {
                        tBG.SetActive(false);
                    }
                    zoomTarget -= zoomRate/40.37f;

                    currentFOV -= zoomRate;
                } else
                {
                    //tBG.SetActive(true);
                    zoomTarget += zoomRate/10;
                    currentFOV += zoomRate;
                }
                currentFOV = Mathf.Clamp(currentFOV, 20, startingFOV);
                cam.fieldOfView = currentFOV;
                zoomTarget = Mathf.Clamp(zoomTarget, -0.5f, 0);
                camTarget.transform.localPosition = new Vector3(zoomTarget, .414f, 0f);

                if(!isPaused)
                {

                    if(Input.GetKeyDown(KeyCode.Mouse1))
                    {
                        zoom = true;
                        Debug.Log("Zoomed in");
                    }
                    if(Input.GetKeyUp(KeyCode.Mouse1))
                    {
                        zoom = false;
                        Debug.Log("Zoomed out");
                    }

                

                    
                    
                    if(!zoom)
                    {

                        if (Physics.Raycast(lookAt, out seen, 5, layerMask))
                        {
                        
                        
                            what = seen.transform.gameObject;
                        


                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            OnClick(what);

                        }



                         
                        }
                        else
                        {
                            seeingNew = false;
                            what = whatDefault;
                        }

                        WhatAmISeeing(what);

                        if (Input.GetKeyDown(KeyCode.Tab))
                        {
                            chatOpen = !chatOpen;

                            pauseScript.OpenChatUI(chatOpen);
                            camControl.enabled = !chatOpen;

                        if(chatOpen){
                            Cursor.lockState = CursorLockMode.None;
                            Cursor.lockState = CursorLockMode.Confined;
                            Cursor.visible = true;
                        } 

                        

                    
                    
                    
                    
                }//if press Tab while unpaused and unzoomed
                    }//if not zoomed or paused
                }//if not paused
                else{//if paused

                if(Input.GetKeyDown(KeyCode.Tab))
                {
                    if(chatOpen)
                    {
                        chatOpen = false;
                        pauseScript.OpenChatUI(chatOpen);
                        camControl.enabled = !chatOpen;
                        Cursor.lockState = CursorLockMode.Locked;

                    }
                }
                }

                
                /*
                if(chatOpen){
                    Cursor.lockState = CursorLockMode.Confined;
                } else Cursor.lockState = CursorLockMode.Locked;*/

                if(Input.GetKeyDown(KeyCode.Return))
                {
                    if(chatOpen)
                    {
                        GetComponent<SendMessage>().OutsideSendMessageCall();
                    }
                }

                

            }

            void FixedUpdate()
            {

                

                // RectTransform[] heldScores = scoreHolder.GetComponentsInChildren<RectTransform>();
                // foreach(RectTransform dimensions in heldScores)
                // {
                //     dimensions.localScale = new Vector2(1,1);
                    
                //     // if(dimensions.gameObject.tag == "TextBox")
                //     // {
                //     //     dimensions.sizeDelta = new Vector2(pBGTrans.sizeDelta.x/2 - 5, pBGTrans.sizeDelta.y / 8);
                //     // } else dimensions.sizeDelta = new Vector2(pBGTrans.sizeDelta.x - 10, pBGTrans.sizeDelta.y / 8);
                // }

            }

            void VTextChange(string what, bool logCheck)
            {


                string description = what;
                if (logCheck == true)
                {

                    vScan.text = "New Information: " + description;

                }
                else
                {

                    vScan.text = description;

                }

            }

            void WhatAmISeeing(GameObject seen)
            {

                
                string whatType = seen.tag;
/*
                if(whatType == "OverFrame")
                {
                    whatType = seen.transform.parent.tag;
                } else whatType = seen.tag;*/


                Image lastBG = tBG.GetComponent<Image>();

                if(whatType == "Untagged" || whatType == null)
                {

                    tBG.SetActive(false);
                    retical.sprite = targetEmpty;
                    

                } else
                    {
                        tBG.SetActive(true);
                        retical.sprite = targetFull;
                        // float toAlpha = 1f;
                        // for(float i = 0.5f; i < toAlpha; i+= 0.1f)
                        // {
                        //     lastBG.color = new Color(lastBG.color.r, lastBG.color.b, lastBG.color.g, i);
                        // }
                    

                

              
                 if (whatType == "NPC")
                {
                    //NPCScript whatScript;
                    //Debug.Log("Yup, that's an NPC");
                    string npcName = seen.GetComponentInParent<NPCScript>().nPCName;
                    bool met = seen.GetComponentInParent<NPCScript>().met;
                    //seen.GetComponent<NPCInfo>().playerLooking = true;

                    if (!met)
                    {
                        seeingNew = true;
                        VTextChange(npcName, true);

                    }
                    else
                    {
                        seeingNew = false;
                        VTextChange(npcName, false);
                    }
                }
                else if (whatType == "PictureFrame" || whatType == "OtherPoster")
                {


                    bool logCheck = what.GetComponent<OpenPage>().newLog;
                    string description = seen.GetComponent<Text>().text;
                    seeingNew = logCheck;
                    VTextChange(description, logCheck);

                } else if(whatType == "MiscObj")
                {
                    if(seen.transform.name == "Ball")
                    {
                        VTextChange("Kick", false);
                    }
                }
                 else VTextChange(" ", false);

            }

            }

            void OnClick(GameObject what)
            {

                

                if (seeingNew)
                {

                    points++;



                    scoreboard.AddEntry(scoreData);

                    if (what.tag == "PictureFrame" || what.tag == "OtherPoster")
                    {
                        what.GetComponent<OpenPage>().newLog = false;
                        what.GetComponent<OpenPage>().GoToURL();
                    }
                    else if (what.tag == "NPC")
                    {

                        what.GetComponent<NPCScript>().Interact();
                        //what.GetComponent<NPCScript>().met = true;

                    }

                }
                else if (!seeingNew)
                {
                    if (what.tag == "PictureFrame" || what.tag == "OtherPoster")
                    {
                        what.GetComponent<OpenPage>().newLog = false;
                        what.GetComponent<OpenPage>().GoToURL();
                    }
                    else if (what.tag == "NPC")
                    {
                        what.GetComponent<NPCScript>().Interact();
                    }
                }

                if(what.transform.name == "Ball")
                {
                    Debug.Log(what.transform.name);
                    what.GetComponent<BallBehaviour>().Kick(cam.transform.forward);
                }

                // Debug.Log(logCheck);

            }
            

           
        }
        }
    

