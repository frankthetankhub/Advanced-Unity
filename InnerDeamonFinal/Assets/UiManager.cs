using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class UiManager : MonoBehaviour
{
    public Text textDisplay1;
    // Start is called before the first frame update
    public static UiManager instance;
    public UITextDisplay combatLog;
    public Button button;
    public Unit[] units;
    public Healthbar healthbar;
    public Window_CharacterPortrait windowCharacterPortrait;
    public Window_CharacterPortrait windowEnemyPortrait;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        combatLog = GameObject.Find("Content").GetComponent<UITextDisplay>();
        button = FindObjectOfType<Button>();
        units = FindObjectsOfType<Unit>();

        windowCharacterPortrait = GameObject.Find("PlayerPortrait").GetComponent<Window_CharacterPortrait>();
        windowEnemyPortrait = GameObject.Find("EnemyPortrait").GetComponent<Window_CharacterPortrait>();
    }
    void Start()
    {
        healthbar = FindObjectOfType<Healthbar>();
        
        //set up the initial transform of unit to follow
        windowCharacterPortrait.Show(GameManager.instance.selectedUnit.transform);
        windowEnemyPortrait.Show(GameObject.Find("Chaser").transform);
        print(windowEnemyPortrait.transform.parent.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("p"))
        {
            windowCharacterPortrait.healthbar.SetHealth(20);
            /*healthbar.slider.value -= 2;*/
            print("p");
        }
        if (Input.GetKeyUp("o"))
        {
            windowCharacterPortrait.healthbar.SetMaxHealth(20);
            print("o");
        }
    }

    /*public void addToCombatlog(string str)
    {
        combatLog.GetComponent<Text>().text +=  str;
    }*/
    
    /*public void setText()
    {
        string text = "";
        textDisplay1.text = map.selectedUnit.name + map.selectedUnit.remainingMovement;
    }*/
    
}
