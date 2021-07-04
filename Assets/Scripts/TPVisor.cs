using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
            TMP_Text vScan;
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
            [SerializeField]
            private ParticleSystem scoreBurst;
            [SerializeField]private GameObject playerCanvas;
            private GameObject tBG;
            [SerializeField] 
            private GameObject textScrollArea;
            private Scrollbar textScrollBar;
            public float vTextScrollPos;
            //image game object for PBG
            private GameObject pBG;
            public int points = 0;
            public string playerName;

            private int layerMask = 1 << 8;
            public int LayerMask
            {get {return layerMask;}}
            
            //private float maxDist;
            public Ray lookAt
            {
                get;
                private set;
            }
            [SerializeField]
            private NPCInfo littleInfoCanv;

            public Camera cam
            {get; private set;}
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
            public RaycastHit Seen
            {get {return seen;}}
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
            public bool thirdPerson
            {get; private set;}
            
            public bool isPaused = false;
            private bool singlePlayer = false;

            
            Scoreboard scoreboard;
            ScoreboardEntryData scoreData;

            // Start is called before the first frame update
            void Start()
            {
                thirdPerson = true;
                screenW = Screen.width;
                screenH = Screen.height;
                

                if(string.IsNullOrEmpty(playerName))
                {
                    playerName = "Your ";
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
                vText = tBG.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
                vScore = pBG.transform.GetChild(1).gameObject;//GameObject.Find("/Main Camera/VisorCanvas/ScoreBG/VisorScore");

                
                //pauseScript = GetComponent<PauseScript>();
                chatOpen = false;
                //chatBehaviour = GetComponent<ChatBehaviour>();
               // cTextRTrans = chatBehaviour.chatText.GetComponent<RectTransform>();
               // cInputRTrans = chatBehaviour.inputField.GetComponent < RectTransform > ();
                scoreHolder = pBG.transform.GetChild(0).gameObject;
                //maxDist = Mathf.Infinity;
                layerMask = ~layerMask;
                
                scoreData.name = playerName;
                scoreData.score = points;


                scoreboard = playerCanvas.GetComponent<Scoreboard>();
                scoreboard.AddEntry(scoreData);
                tBGTrans = tBG.GetComponent<RectTransform>();
                pBGTrans = pBG.GetComponent<RectTransform>();
                vTextTrans = textScrollArea.GetComponent<RectTransform>();
                pTextTrans = vScore.GetComponent<RectTransform>();

                //cam = Camera.main;
                //visor = GetComponent < Canvas > ();
                vScan = vText.GetComponent<TMP_Text>();
                vPoints = vScore.GetComponent<Text>();
                //rectTransform = vScan.GetComponent<RectTransform>();
                
                //Vscan and TBG coordinates 
                textPos = new Vector2(0, 0);
                //vpoints and PBG coordinates
                pointPos = new Vector2(screenW, screenH);


                tBGTrans.position = textPos;
                
                pBGTrans.position = pointPos;
                pTextTrans.position = new Vector2(screenW + 10, screenH - 5);
                pTextTrans.sizeDelta = new Vector2(pBGTrans.sizeDelta.x/2, pBGTrans.sizeDelta.y / 8);
                //scoreHolder.transform.position = new Vector2(pointPos.x, pointPos.y - pBGTrans.sizeDelta.y);
                scoreHolder.transform.position = new Vector2(10000, 10000);//sent away for single player experience
                pointPos = new Vector2(screenW, screenH);


                tBGTrans.position = textPos;
                pBGTrans.sizeDelta = new Vector2(screenW / 6, screenH / 7.2f);
                tBGTrans.sizeDelta = new Vector2(screenW, screenH / 3);
                vTextTrans.sizeDelta = new Vector2(screenW, 124.693f);
                vTextTrans.position = new Vector2(screenW/2, 62.5f);
                vText.GetComponent<RectTransform>().sizeDelta = new Vector2(vTextTrans.sizeDelta.x -20, 300);
                vText.transform.position = new Vector2(0, 62.5f);
                

                textScrollBar = textScrollArea.transform.GetChild(2).GetComponent<Scrollbar>();

                

               
                


            }

            // Update is called once per frame
            void Update()

            {

                

                isPaused = pauseScript.isPaused;
                
                
                

                vPoints.text = playerName + " score: " + System.Environment.NewLine + points.ToString();

                if(scoreData.score < points)
                {
                scoreData.score = points;
                }
                lookAt = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

                


                //playerCanvas.transform.position = cam.ScreenToWorldPoint(textPos);
                //Debug.DrawRay(cam.transform.position, Vector3.forward * 100, Color.green);
                

            

                

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

                        
                        if (Physics.Raycast(lookAt, out seen, 5, layerMask) && thirdPerson)
                        {
                        
                            
                            what = seen.transform.gameObject;
                        


                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            OnClick(what);

                        }


                        WhatAmISeeing(seen);
                         
                        }
                        else
                        {
                            seeingNew = false;
                            what = whatDefault;
                            littleInfoCanv.playerLooking = false;
                            tBG.SetActive(false);
                            if(littleInfoCanv.transform.GetChild(0).gameObject.activeInHierarchy)
                            {

                                littleInfoCanv.CardClear();

                            }
                     
                        }
                        

                        

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
            

                cam.fieldOfView = currentFOV;

            }

            void FixedUpdate()
            {

                // if(textScrollBar == null)
                // {
                //     textScrollBar = textScrollArea.transform.GetChild(2).GetComponent<Scrollbar>();
                // }

                vTextScrollPos = Mathf.Clamp(vTextScrollPos, 0f, 1f);
                textScrollBar.value = vTextScrollPos;
                if(zoom)
                {
                    if(tBG.activeInHierarchy)
                    {
                        tBG.SetActive(false);
                    }
                    //zoomTarget -= zoomRate/40.37f;

                    currentFOV -= zoomRate;
                } else
                {
                    //tBG.SetActive(true);
                    //zoomTarget += zoomRate/10;
                    currentFOV += zoomRate;
                }
                currentFOV = Mathf.Clamp(currentFOV, 20, startingFOV);

                //Debug.Log(Input.mouseScrollDelta);

                if(Input.mouseScrollDelta.y > 0 && textScrollBar.value < 1)
                {

                    

                    vTextScrollPos += 0.1f;

                } else if(Input.mouseScrollDelta.y < 0 && textScrollBar.value > 0)
                {

                    vTextScrollPos -= 0.1f;

                }
                
                
            

            }

            

            void VTextChange(string what, bool logCheck)
            {


                string description = what;
                

                vScan.text = description;

                

            }

            void WhatAmISeeing(RaycastHit seen)
            {

                
                string whatType = seen.transform.gameObject.tag;


                



                Image lastBG = tBG.GetComponent<Image>();

                if(whatType == "Untagged" || whatType == null)
                {
                    vTextScrollPos = 1;
                    tBG.SetActive(false);
                    retical.sprite = targetEmpty;
                    
                    
                    

                } else
                    {
                        
                        retical.sprite = targetFull;
                      
                    

                

              
                 if (whatType == "NPC")
                {
                    //NPCScript whatScript;
                    //Debug.Log("Yup, that's an NPC");
                    string npcName = seen.transform.GetComponentInParent<NPCScript>().nPCName;
                    bool met = seen.transform.GetComponentInParent<NPCScript>().met;
                    //seen.GetComponent<NPCInfo>().playerLooking = true;
                    littleInfoCanv.TextPlacingFunction(seen.transform.gameObject, seen.point);
                    littleInfoCanv.playerLooking = true;

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
                    tBG.SetActive(true);


                    bool logCheck = what.GetComponent<OpenPage>().newLog;
                    string description = seen.transform.GetComponent<Text>().text;
                    seeingNew = logCheck;
                    VTextChange(description, logCheck);

                } else if(whatType == "MiscObj")
                {
                    tBG.SetActive(true);
                    if(seen.transform.name == "Ball")
                    {
                        VTextChange("Kick", false);
                    }
                }
                 else
                 {
                     
                      VTextChange(" ", false);
                      
                 }

            }

            }

            void OnClick(GameObject what)
            {

                

                if (seeingNew)
                {

                    
                    scoreBurst.Play(true);
                    if (what.tag == "PictureFrame" || what.tag == "OtherPoster")
                    {
                        what.GetComponent<OpenPage>().newLog = false;
                        what.GetComponent<OpenPage>().GoToURL();
                        points++;
                        scoreboard.AddEntry(scoreData);
                        
                    }
                    else if (what.tag == "NPC")
                    {

                        what.GetComponent<NPCScript>().Interact();
                        littleInfoCanv.playerLooking= false;
                        PerspectiveChange(false);
                        littleInfoCanv.CardClear();
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
                        PerspectiveChange(false);
                        littleInfoCanv.CardClear();
                    }
                }

                if(what.transform.name == "Ball")
                {
                    Debug.Log(what.transform.name);
                    what.GetComponent<BallBehaviour>().Kick(cam.transform.forward);
                }

                // Debug.Log(logCheck);

            }

            public void PerspectiveChange(bool tp)
            {
                thirdPerson = tp;
                Debug.Log($"thirdPerson set to {tp}");
            }
            

           
        }
        }
    

