using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
    // Start is called before the first frame update
    [DllImport("__Internal")]
    private static extern void OpenExtURL(string url);

    void Start()
    {
        
        
    }
    public void LoadFP()
    {
        SceneManager.LoadSceneAsync("FirstPerson", LoadSceneMode.Single);
        
    }

    public void LoadTP()
    {
        SceneManager.LoadSceneAsync("ThirdPerson", LoadSceneMode.Single);
        
    }

    public void Help()
    {
        OpenExtURL("https://www.youtube.com/watch?v=dQw4w9WgXcQ");
    }
}
