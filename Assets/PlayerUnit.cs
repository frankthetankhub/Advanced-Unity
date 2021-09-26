using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    public float cooldownToEndTurn;
    public bool canEndTurn;
    public void Awake()
    {
        base.Awake();
    }
    public void Start()
    {
        base.Start();
        cooldownToEndTurn = 0;
    }

    // Update is called once per frame
    public void Update()
    {
        base.Update();
        if (turn)
        {
            EndTurnCooldownHandler();
        }
        // refactor to onvaluechanged
        targetGameObject = GameManager.instance.selectedObject;
        //----
        if (Input.GetKeyUp("a") & turn )
        {
            base.BasicAttack();
        }
        
        if (Input.GetKeyDown("e") & !ismoving & turn & canEndTurn)
        {
            canEndTurn = false;
            print(name +"ended my turn");
            GameManager.instance.RoundOver();
        }
    }
    public void EndTurnCooldownHandler()
    {
        if (!canEndTurn)
        {
            cooldownToEndTurn += Time.deltaTime;
        }
        if (cooldownToEndTurn >= 1)
        {
            canEndTurn = true;
            cooldownToEndTurn = 0;
        }
    }
    
}
