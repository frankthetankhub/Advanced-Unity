using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public float topBarrier;
    public float lowerBarrier;
    public float leftBarrier;
    public float rightBarrier;
    public float zoom;
    public float zoomSpeed;
    public float scrollSpeed;
    // Start is called before the first frame update
    void Start()
    {
        // the values determine at which pointerposition relative to the screen the camera starts moving
        topBarrier = 0.98f;
        lowerBarrier = 0.02f;
        leftBarrier = 0.02f;
        rightBarrier = 0.98f;
        scrollSpeed = 15f;
        zoomSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        MouseScreenScrolling();
        //ScrollwheelZoom();
    }

    public void MouseScreenScrolling()
    {
        if (Input.mousePosition.y <= Screen.height * lowerBarrier)
        {
            transform.Translate( scrollSpeed *  Time.deltaTime * new Vector3(0,-1,0) );
        }
        if (Input.mousePosition.y >= Screen.height * topBarrier)
        {
            transform.Translate(scrollSpeed *  Time.deltaTime * new Vector3(0,1,0));
        }
        if (Input.mousePosition.x >= Screen.width * rightBarrier)
        {
            transform.Translate(scrollSpeed *  Time.deltaTime * new Vector3(1,0,0));
        }
        if (Input.mousePosition.x <= Screen.width * leftBarrier)
        {
            transform.Translate(scrollSpeed *  Time.deltaTime * new Vector3(-1,0,0));
        }
    }

    /*public void ScrollwheelZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            zoom -= zoomSpeed * Time.deltaTime;
            zoom = Mathf.Clamp(zoom, -10, -30);
            transform.position = new Vector3(transform.position.x,transform.position.y, zoom);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoom += zoomSpeed * Time.deltaTime;
            zoom = Mathf.Clamp(zoom, -10, -30);
            transform.position = new Vector3(transform.position.x,transform.position.y, zoom);
        }
    }*/
}
