using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    bool fullscreen;

    // Start is called before the first frame update
    void Start()
    {
        fullscreen = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            fullscreen = !fullscreen;

            Screen.fullScreen = fullscreen;

        }
    }

    public void CenterCamera(int smallestX, int biggestX, int smallestY, int biggestY)
    {
        Debug.Log(smallestX+" "+biggestX + " " + smallestY + " " + biggestY);

        int diffX = biggestX - smallestX;
        int diffY = biggestY - smallestY;

        transform.position = new Vector3(smallestX + (diffX / 2), smallestY + (diffY / 2),-10);
    }
}
