using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatStyler : MonoBehaviour
{

    [SerializeField] private RectTransform mainBG;
    [SerializeField] private RectTransform bG2;
    [SerializeField] private RectTransform bG3;

    private Vector2 screen;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        screen = new Vector2(Screen.width, Screen.height);
        mainBG.sizeDelta = new Vector2(screen.x/3, screen.y);
        bG2.sizeDelta = new Vector2(mainBG.sizeDelta.x -50, mainBG.sizeDelta.y - 30);

        
    }
}
