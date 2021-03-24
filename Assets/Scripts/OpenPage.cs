using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Conference.Characters
{
    namespace Conference.Scoreboards
    {
        public class OpenPage : MonoBehaviour
        {


            public string destination;

            public bool newLog = true;

            //constructor to specify JSLib file(in progress)
            /*
            static JSPlug()
            {
                string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
                string 
            }
            */
            [DllImport("__Internal")]
            private static extern void OpenExtURL(string url);



            // Start is called before the first frame update
            void Start()
            {
                //Debug.Log(newLog);


            }

            public void GoToURL()
            {

                OpenExtURL(destination);
                newLog = false;



            }


        }

    }
}