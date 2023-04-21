using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public int y = 0;
    Vector3 velocity = Vector3.zero;
    public float smoothness;
    public int yLimit = -1;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(0, y * -1.3f, -10), ref velocity, smoothness);
    }

    public void Up()
    {
        y--;
        if (y < yLimit)
        {
            y = yLimit;
        }
    }

    public void Down() 
    {
        y++;
    }

    public void Top()
    {
        y = 0;
    }
}
