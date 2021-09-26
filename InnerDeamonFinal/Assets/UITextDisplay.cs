using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextDisplay : MonoBehaviour
{
    public Text combatlog;
    public Scrollbar scrollbar;
    public ScrollRect rect;

    public void Start()
    {
        combatlog = GetComponent<Text>();
        //scrollbar = GameObject.Find("Scrollbar Vertical").GetComponent<Scrollbar>();
        rect = FindObjectOfType<ScrollRect>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToCombatlog(string stringtext)
    {
        combatlog.text += Environment.NewLine + stringtext;
        Canvas.ForceUpdateCanvases();
        rect.verticalNormalizedPosition = 0;
        
        /*maybe turn this method into coroutine so we can wait a little bit before or after printing to make it more dynamic
        return yield WaitForSeconds(0.01f);*/
    }
}
