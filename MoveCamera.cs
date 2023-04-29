using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public int y = 0;
    Vector3 velocity = Vector3.zero;
    public float smoothness;
    public int yLimit = -1;
    public GameObject settingCanvas;
    public static bool[] warned = { false };
    public bool moveUp = false;
    public bool moveDown = false;
    public float cameraMoveCycle = 0.2f;
    //{ settings() openbool }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        Up(); Down();
        if (cameraMoveCycle == 0.05000001f) cameraMoveCycle = 0.05f;
        if (cameraMoveCycle == 0f) cameraMoveCycle = 0.05f;
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(0, y * -1.3f, -10), ref velocity, smoothness);
    }
    public float upTimer = 0f;
    void Up()
    {
        if(upTimer != 0f) 
        {
            upTimer += Time.deltaTime;
            if(upTimer > cameraMoveCycle) { upTimer = 0f; }
            return;
        }
        if (!moveUp) return;
        upTimer += Time.deltaTime;
        y--;
        if (y < yLimit)
        {
            y = yLimit;
        }
    }
    
    public float dTimer = 0f;
    void Down()
    {
        if (dTimer != 0f)
        {
            dTimer += Time.deltaTime;
            if (dTimer > cameraMoveCycle) { dTimer = 0f; }
            return;
        }
        if (!moveDown) return;
        dTimer += Time.deltaTime;
        y++;
    }

    public void Top()
    {
        y = 0;
    }

    public void Settings(string openBool)
    {
        if (openBool.Equals("true"))
        {
            settingCanvas.SetActive(true);
        }
        else if(openBool.Equals("false"))
        {
            settingCanvas.SetActive(false);
        }
        else
        {
            if(!warned[0]) Debug.LogWarning("Wrong openBool in MoveCamera.Settings(string)");
            warned[0] = true;
        }
    }
}
