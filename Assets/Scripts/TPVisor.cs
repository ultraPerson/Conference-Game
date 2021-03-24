using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Conference.Characters
{
    namespace Conference.Scoreboards
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
            RectTransform tBGTrans;
            //transform for PBG
            RectTransform pBGTrans;
            //transform for vScan
            RectTransform vTextTrans;
            //transform for vPoints
            RectTransform pTextTrans;

            public GameObject whatDefault;
            public GameObject vText;
            //text game object for points
            public GameObject vScore;
            public GameObject playerCanvas;
            public GameObject tBG;
            //image game object for PBG
            public GameObject pBG;
            public int points = 0;
            public string playerName;

            private int layerMask = 1 << 8;
            //private float maxDist;
            public Ray lookAt
            {
                get;
                private set;
            }

            public Camera cam;//public
            private GameObject scoreHolder;
            private bool seeingNew = false;
            private float screenW = Screen.width;
            private float screenH = Screen.height;
            //private float totalWidthT;
            //private float totalHeightT;
            private Vector2 textPos;
            private Vector2 pointPos;
            private RaycastHit seen;
            private GameObject what;
            private Text whatDesc;
            
            Scoreboard scoreboard;
            ScoreboardEntryData scoreData;

            // Start is called before the first frame update
            void Start()
            {
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

                

                
                //visor.renderMode = RenderMode.ScreenSpaceOverlay;


            }

            // Update is called once per frame
            void Update()

            {


                screenH = Screen.height;
                screenW = Screen.width;



                vPoints.text = playerName + " score: " + points.ToString();

                scoreData.score = points;

                lookAt = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

                //Vscan and TBG coordinates 
                textPos = new Vector2(screenW + (screenW * -1), screenH + (screenH * -1));
                //vpoints and PBG coordinates
                pointPos = new Vector2(screenW, screenH);


                tBGTrans.position = textPos;
                // vTextTrans.position = tBGTrans.position;
                pBGTrans.position = pointPos;
                pTextTrans.position = pBGTrans.position;
                pTextTrans.sizeDelta = new Vector2(pBGTrans.sizeDelta.x, pBGTrans.sizeDelta.y / 8);
                scoreHolder.transform.position = new Vector2(pointPos.x, pointPos.y - pBGTrans.sizeDelta.y);
                RectTransform[] heldScores = scoreHolder.GetComponentsInChildren<RectTransform>();
                foreach(RectTransform dimensions in heldScores)
                {
                    
                    if(dimensions.gameObject.tag == "TextBox")
                    {
                        dimensions.sizeDelta = new Vector2(pBGTrans.sizeDelta.x/2 - 5, pBGTrans.sizeDelta.y / 8);
                    } else dimensions.sizeDelta = new Vector2(pBGTrans.sizeDelta.x - 10, pBGTrans.sizeDelta.y / 8);
                }
                pBGTrans.sizeDelta = new Vector2(screenW / 8, screenH / 3);
                tBGTrans.sizeDelta = new Vector2(screenW / 6, screenH / 8);
                vTextTrans.sizeDelta = tBGTrans.sizeDelta;
                vTextTrans.position = tBGTrans.position;


                playerCanvas.transform.position = cam.ScreenToWorldPoint(textPos);
                //Debug.DrawRay(cam.transform.position, Vector3.forward * 100, Color.green);



                if (Physics.Raycast(lookAt, out seen, 10, layerMask))
                {
                    what = seen.transform.gameObject;
                    //Debug.Log(what.name);








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


                if (whatType == "NPC")
                {
                    //NPCScript whatScript;
                    Debug.Log("Yup, that's an NPC");
                    string npcName = what.GetComponent<NPCScript>().nPCName;
                    bool met = what.GetComponent<NPCScript>().met;

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
                else if (whatType == "PictureFrame")
                {


                    bool logCheck = what.GetComponent<OpenPage>().newLog;
                    string description = seen.GetComponent<Text>().text;
                    seeingNew = logCheck;
                    VTextChange(description, logCheck);

                }
                else VTextChange("Scanning...", false);

            }

            void OnClick(GameObject what)
            {

                if (seeingNew)
                {

                    points++;



                    scoreboard.AddEntry(scoreData);

                    if (what.tag == "PictureFrame")
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
                    if (what.tag == "PictureFrame")
                    {
                        what.GetComponent<OpenPage>().newLog = false;
                        what.GetComponent<OpenPage>().GoToURL();
                    }
                    else if (what.tag == "NPC")
                    {
                        what.GetComponent<NPCScript>().Interact();
                    }
                }

                // Debug.Log(logCheck);

            }
            /*
            public void CursorState(int choice)
            {
                switch (choice)
                {
                    //1 = none, 2 = restricted, 3 = locked
                    case 1:
                        Cursor.lockState = CursorLockMode.None;
                        break;
                    case 2:
                        Cursor.lockState = CursorLockMode.Confined;
                        break;


                }
            }*/

            void LateUpdate()
            {



            }
        }
    }
}
