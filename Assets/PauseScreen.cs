using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{

    public bool Paused = true;
    private SpriteRenderer renderer;


    // Start is called before the first frame update
    void Start()
    {
        Paused = true;
        renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Paused = !Paused;
            renderer.enabled = Paused;
        }
    }
}
