using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Chaser : Unit
{
    public Collider[] unitsInVision;
    public bool isfinished;
    public void Awake()
    {
        movementSpeed = 3;
        maxMoveDistance = 4;
        remainingMovement = maxMoveDistance;
        visionRadius = 10;
        baseInitiative = 5;
        health = 40;
        hitPercentage = 0.75f;
        baseDamage = 6;
        attackRange = 1.2f;
    }

    public void Start()
    {
        Debug.Log("I am a chaser!");
        // has to be changed to update target to nearby Playerunits
        //targetGameObject = GameManager.instance.selectedUnit.gameObject;
        
    }

    public void Update()
    {
        if (health <= 0)
        {
            removeFromGame();
        }

        
        if (turn )
        {
            unitsInVision = Physics.OverlapSphere(transform.position, visionRadius);
            foreach (var hitCollider in unitsInVision)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    targetGameObject = hitCollider.gameObject;
                }
            }
            if (targetGameObject & !ismoving)
            {
                StartCoroutine(chase());
                StartCoroutine(DoLast());
            }
            else if (turn)
            {
                UiManager.instance.combatLog.AddToCombatlog(name + " Skips it turn");
            }
        }
    }
    // helpfunction to visualize vision radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
    public IEnumerator chase()
    {
        turn = false;
        isfinished = false;
        currentPath = GameManager.instance.map.FindPath(GameManager.instance.map.GetTileAtPosition(transform.position), GameManager.instance.map.GetTileAtPosition(targetGameObject.transform.position));
        try
        {
            currentPath.RemoveAt(currentPath.Count-1);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        yield return StartCoroutine(Movement());
        
        
        turn = false;
        isfinished = true;
    }

    public IEnumerator DoLast()
    {
        while (!isfinished)
        {
            yield return new WaitForSeconds(0.1f);
        }

        isfinished = true;
        print("check damage");
        if (remainingMovement >= 2 & !ismoving)
        {
            // Generic attack method
            BasicAttack();
        }
        Debug.Log("chase!!!");
        GameManager.instance.RoundOver();
    }

}