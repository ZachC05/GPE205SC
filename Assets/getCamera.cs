using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class getCamera : MonoBehaviour
{
    Canvas canvas;
    public TMP_Text pointsPlayer1;
    public TMP_Text livesPlayer1;
    public TMP_Text pointsPlayer2;
    public TMP_Text livesPlayer2;
    public TMP_Text healthPlayer1;
    public TMP_Text healthPlayer2;
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
