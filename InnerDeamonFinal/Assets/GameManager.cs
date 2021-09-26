using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<Unit> units;
    public int positionInTurnQueue;
    public GameObject selectedObject;
    public Unit selectedUnit;
    public TileMap map;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        // map = UIManager.instance.map;
        //combatLog = GameObject.Find("Content").GetComponent<UITextDisplay>();
        units = FindObjectsOfType<Unit>().ToList();
        selectedUnit = GameObject.Find("DemonDudeSculpt").GetComponent<Unit>();
        map = FindObjectOfType<TileMap>();
    }
    void Start()
    {
        StartNewRound();
    }

    public void StartNewRound()
    {
        foreach (var unit in units)
        {
            unit.NewRoundPrep();
        }
        //Array.Sort(units, ComparisonInitiativeRolls);
        units.Sort(ComparisonInitiativeRolls);
        positionInTurnQueue = 0;
        units[0].turn = true;
        if (units[0].CompareTag("Player"))
        {
            //select current unit and update character window display
            selectedUnit = units[positionInTurnQueue];
            UiManager.instance.windowCharacterPortrait.Show(selectedUnit.transform);
        }
        if (units[0].TryGetComponent(out PlayerUnit firstunit))
        {
            selectedUnit = firstunit;
        }
        UiManager.instance.combatLog.AddToCombatlog("A new Round beginns");
        foreach (var unit in units)
        {
            UiManager.instance.combatLog.AddToCombatlog(unit.name + " InitiativeRoll: " + unit.roundInitiative);
        }
        UiManager.instance.combatLog.AddToCombatlog(units[0].name +"'s turn.");
    }

    private int ComparisonInitiativeRolls(Unit x, Unit y)
    {
        return y.roundInitiative.CompareTo(x.roundInitiative);
    }

    void Update()
    {
        MouseHandling();
    }

    public void RoundOver()
    {
        units[positionInTurnQueue].turn = false;
        if (units[positionInTurnQueue].CompareTag("Player") & units[positionInTurnQueue].currentPath != null)
        {
            map.highlightPath(selectedUnit.currentPath.ToArray());
            units[positionInTurnQueue].currentPath = null;
        }
        positionInTurnQueue++;
        if (positionInTurnQueue < units.Count)
        {
            units[positionInTurnQueue].turn = true;
            if (units[positionInTurnQueue].CompareTag("Player"))
            {
                //select current unit and update character window display
                selectedUnit = units[positionInTurnQueue];
                UiManager.instance.windowCharacterPortrait.Show(selectedUnit.transform);
            }
            UiManager.instance.combatLog.AddToCombatlog(units[positionInTurnQueue].name +"'s turn.");
                
        }
        else
        {
            StartNewRound();
        }
    }
    public GameObject SelectObject()
    {
    
        Ray ray;
        RaycastHit hitInfo;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.gameObject == selectedObject)
            {
                return null;
            }

            /*if (!hitInfo.transform.CompareTag("Player"))
            {
                UiManager.instance.windowEnemyPortrait.Show(hitInfo.transform);
            }*/
            return hitInfo.collider.gameObject;
        }
        return selectedObject;
    }
    public void MouseHandling()
    {
        if (Input.GetMouseButtonUp(0))
        {
            selectedObject = SelectObject();
            if (!selectedUnit.ismoving)
            {
                selectedUnit.GetPath(selectedObject);
            }
        }
    }

    
}
