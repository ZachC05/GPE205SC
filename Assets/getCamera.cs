using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class getCamera : MonoBehaviour
{
    Canvas canvas;
    public TMP_Text points;
    public TMP_Text lives;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    public void getandset(Camera cam)
    {
        Debug.Log(cam);
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = cam;
    }
}
