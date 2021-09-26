using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Object = System.Object;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxMoveDistance;
    public float movementSpeed = 1;
    public int remainingMovement; 
    public List<Tile> currentPath;
    public float visionRadius;
    public GameObject targetGameObject;
    public int baseInitiative;
    public bool turn;
    public bool ismoving;
    public int roundInitiative;
    public int pathIndex;
    public int health;
    public LineRenderer line;
    public float hitPercentage;
    public float attackRange;
    [SerializeField] public int baseDamage;

    public void Awake()
    {
        maxMoveDistance = 10;
        movementSpeed = 7f;
        remainingMovement = maxMoveDistance;
        visionRadius = 5;
        baseInitiative = 5;
        health = 50;
        line = GetComponent<LineRenderer>();
        hitPercentage = 0.8f;
        baseDamage = 6;
        attackRange = 1.8f;
    }

    public void Start()
    {
        
    }
    public void Update()
    {
        if (Input.GetKeyUp("u"))
        {
            drawLine();
        }
        if (turn)
        {
            if (Input.GetKeyUp("w"))
            {
                StopCoroutine(MovementWithHilight());
                if (!ismoving)
                {
                    StartCoroutine(MovementWithHilight());
                }
            }
        }
    }

    public void GetPath(GameObject target)
    {
        if (currentPath != null)
        {
            GameManager.instance.map.highlightPath(currentPath.ToArray());
        }
        if (target /*& target != GameManager.instance.selectedObject*/)
        {
            currentPath = GameManager.instance.map.FindPath(GameManager.instance.map.GetTileAtPosition(transform.position), GameManager.instance.map.GetTileAtPosition(target.transform.position));
            GameManager.instance.map.highlightPath(currentPath.ToArray());
        }
        else
        {
            currentPath = null;
        }
    }
    public IEnumerator Move (Vector3 target)
    {
        //transform.rotation = Quaternion.LookRotation(transform.position - target);
        //transform.LookAt(transform.position - target, new Vector3(0,0,1));
        while(Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position,target , movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
    }
    public IEnumerator MovementWithHilight ()
    {
        if (currentPath == null)
        {
            UiManager.instance.combatLog.AddToCombatlog( "No Path to follow.");
            yield break;
        }
        if (currentPath.Count == 0)
        {
            UiManager.instance.combatLog.AddToCombatlog( "No Path to follow.");
            yield break;
        }
        pathIndex = 0;
        //as long as we are still in the path
        while( currentPath.Count > 0)
        {
            if (remainingMovement < currentPath[0].movementCost)
            {
                if (ismoving)
                {
                    UiManager.instance.combatLog.AddToCombatlog( "Unit "+ name +" moved to position: "+ transform.position.x+ ","+transform.position.y);
                    ismoving = false;
                    yield break;
                } 
                UiManager.instance.combatLog.AddToCombatlog( "Unit "+ name +" has no more movement remaining!");
                ismoving = false;
                yield break;
            }
            // walk until we arrive at the tile
            ismoving = true;
            yield return StartCoroutine(Move(currentPath[0].transform.position + new Vector3(0,0,-1.8f )));
            print("Outside of MOVE print");
            remainingMovement -= currentPath[0].movementCost;
            Debug.Log(pathIndex);
            currentPath[0].highlight(Color.cyan);
            currentPath.RemoveAt(0);
            pathIndex++;
        }
        UiManager.instance.combatLog.AddToCombatlog( "Unit "+ name +" moved to position: "+ transform.position.x+ ","+transform.position.y);
        ismoving = false;
    }

    public IEnumerator Movement ()
    {
        if (currentPath == null)
        {
            yield break;
        }
        ismoving = true;
        /*pathIndex = 0;*/
        //as long as we are still in the path
        while( currentPath.Count > 0)
        {
            print(remainingMovement);
            if (remainingMovement < currentPath[0].movementCost)
            {
                UiManager.instance.combatLog.AddToCombatlog( "Unit "+ name +" has no more movement remaining!");
                ismoving = false;
                yield break;
            }
            else
            {
                // walk until we arrive at the tile
                yield return StartCoroutine(Move(currentPath[0].transform.position + new Vector3(0,0,-1.8f )));
                remainingMovement -= currentPath[0].movementCost;
                currentPath.RemoveAt(0);
            }
            /*pathIndex++;*/
        }
        UiManager.instance.combatLog.AddToCombatlog( "Unit "+ name +" moved to position: "+ transform.position.x+ ","+transform.position.y);
        ismoving = false;
    }
    
    public void NewRoundPrep()
    {
        roundInitiative = baseInitiative + Random.Range(0, 10);
        remainingMovement = maxMoveDistance;
    }

    public void TakeDamage(int damage)
    {
        this.health -= damage;
        if (health < 0)
        {
            health = 0;
        }

        if (tag == "Player")
        {
            UiManager.instance.windowCharacterPortrait.healthbar.SetHealth(health);
        }
        else
        {
            UiManager.instance.windowEnemyPortrait.healthbar.SetHealth(health);
        }
    }

    public void drawLine()
    {
        line.SetPosition(0, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity);
        foreach (var tile in hitInfo.collider.GetComponent<Tile>().neighbors)
        {
            tile.highlight(Color.red);
        }
        line.SetPosition(1, hitInfo.point);
    }

    public void removeFromGame()
    {
        UiManager.instance.combatLog.AddToCombatlog(name +" Died an honorable Death!");

        GameManager.instance.units.Remove(this);
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void BasicAttack()
    {
        if (!targetGameObject)
        {
            print("sas");
            targetGameObject = GameManager.instance.SelectObject();
        }
        Vector3 distance = transform.position - targetGameObject.transform.position;
        if (targetGameObject.TryGetComponent(out Unit target) & Mathf.Abs(distance.sqrMagnitude) < attackRange)
        {
            if (Random.Range(0f,1f) < hitPercentage)
            {
                int damage = Random.Range(0,6) + baseDamage;
                UiManager.instance.combatLog.AddToCombatlog(name + " attacks " + target.name + " and hits for " + damage + "damage!");
                target.TakeDamage(damage);
            }
            else
            {
                UiManager.instance.combatLog.AddToCombatlog(name + " attacks " + targetGameObject.name + " but misses!");
            }
        }
        else
        {
            UiManager.instance.combatLog.AddToCombatlog("not a valid Target!");
        }
    }

}

