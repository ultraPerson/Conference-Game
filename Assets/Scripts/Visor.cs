using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Conference.Characters
{
    namespace Conference.Scoreboards
    {
        public class Visor : MonoBehaviour
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

            public GameObject vText;
            //text game object for points
            public GameObject vScore;
            public GameObject playerCanvas;
            public GameObject tBG;
            //image game object for PBG
            public GameObject pBG;
            public int points = 0;
            public string name;



            private Ray lookAt;
            private Camera cam;
            private float screenW;
            private float screenH;
            private float totalWidthT;
            private float totalHeightT;
            private Vector2 textPos;
            private Vector2 pointPos;
            private RaycastHit seen;
            private GameObject what;
            Scoreboard scoreboard;
            ScoreboardEntryData scoreData;

            // Start is called before the first frame update
            void Start()
            {

                name = "New Player";
                scoreData.name = name;
                scoreData.score = points;


                scoreboard = playerCanvas.GetComponent<Scoreboard>();
                scoreboard.AddEntry(scoreData);
                tBGTrans = tBG.GetComponent<RectTransform>();
                pBGTrans = pBG.GetComponent<RectTransform>();
                vTextTrans = vText.GetComponent<RectTransform>();
                pTextTrans = vScore.GetComponent<RectTransform>();

                cam = Camera.main;
                //visor = GetComponent < Canvas > ();
                vScan = vText.GetComponent<Text>();
                vPoints = vScore.GetComponent<Text>();
                //rectTransform = vScan.GetComponent<RectTransform>();

                screenH = Screen.height;
                screenW = Screen.width;

                //Vscan and TBG coordinates 
                textPos = new Vector2(screenW + (screenW * -1), screenH + (screenH * -1));
                //vpoints and PBG coordinates
                pointPos = new Vector2(screenW, screenH);
                totalWidthT = vTextTrans.rect.width;
                totalHeightT = vTextTrans.rect.height;
                tBGTrans.sizeDelta = new Vector2(totalWidthT, totalHeightT);

                tBGTrans.position = textPos;
                vTextTrans.position = tBGTrans.position;
                pBGTrans.position = pointPos;
                pTextTrans.position = pBGTrans.position;
                //visor.renderMode = RenderMode.ScreenSpaceOverlay;

                //rectTransform.localPosition = new Vector3(0, 0, 0);
                //rectTransform.sizeDelta = new Vector2(400, 200);

            }

            // Update is called once per frame
            void Update()

            {




                vPoints.text = name + " score: " + points.ToString();
                lookAt = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit seen;




                playerCanvas.transform.position = cam.ScreenToWorldPoint(textPos);
                Debug.DrawRay(cam.transform.position, Vector3.forward * 100, Color.green);



                if (Physics.Raycast(lookAt, out seen))
                {
                    what = seen.transform.gameObject;
                    Debug.DrawRay(lookAt.origin, seen.point, Color.green, 60);


                    bool whatText = what.GetComponent<Text>();

                    if (whatText)
                    {
                        bool logCheck = what.GetComponent<OpenPage>().newLog;
                        // Debug.Log(logCheck);
                        string description = what.GetComponent<Text>().text;
                        if (logCheck == true)
                        {

                            vScan.text = "New Log: " + description;

                        }
                        else
                        {

                            vScan.text = description;

                        }

                        tBGTrans.sizeDelta = new Vector2(totalWidthT, totalHeightT);

                        if (Input.GetKeyDown(KeyCode.Mouse0))
                        {
                            if (logCheck == true)
                            {

                                points++;



                                scoreboard.AddEntry(scoreData);



                            }
                            else;

                            what.GetComponent<OpenPage>().newLog = false;
                            what.GetComponent<OpenPage>().GoToURL();
                            // Debug.Log(logCheck);
                        }

                    }
                    else
                    {
                        vScan.text = "Scanning visuals...";
                    }

                }

                scoreData.score = points;


            }
        }
    }
}