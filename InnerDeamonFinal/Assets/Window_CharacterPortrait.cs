using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_CharacterPortrait : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform followTransform;
    public Healthbar healthbar;
    private void Awake()
    {
        cameraTransform = transform.Find("Camera");
        healthbar = transform.GetComponentInChildren<Healthbar>();

    }

    public void Start()
    {
        //Hide();
    }

    public void Update()
    {
        if (followTransform)
        {
            cameraTransform.position = new Vector3(followTransform.position.x, followTransform.position.y, -4);
        }
    }

    public void Show(Transform followTransform)
    {
        gameObject.SetActive(true);
        this.followTransform = followTransform;
        cameraTransform.position = new Vector3(followTransform.position.x, followTransform.position.y, -4);
        print(name +" transform changed");
        followTransform.TryGetComponent<Unit>(out Unit unit);
        if (unit)
        {
            this.healthbar.SetHealth(unit.health);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
