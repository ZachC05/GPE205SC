using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveCamera : MonoBehaviour
{
    private TankPawn pawn;
    private Camera cam;
    bool goingToNextArea;
    public Vector3 goToY;
    public GameObject camAnchor;

    // Start is called before the first frame update
    void Start()
    {
        //sets the cam position to a fixed position
        goToY.x = gameObject.transform.position.x;
        goToY.z = gameObject.transform.position.z;


    }

    // Update is called once per frame
    void Update()
    {
        //when this bool is true, the camera will move forward
        if (goingToNextArea)
        {
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, goToY, 100 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        //whenever the player enters a room, it will try to get the tankpawn and the camera, then it will start the moving proccess
        Debug.Log("Player Entered Room" + gameObject.name);
        pawn = other.GetComponent<TankPawn>();
        if(pawn != null)
        {
            if(pawn.owner != null)
            {
                
                cam = pawn.owner.GetComponent<Camera>();
                if (cam != null)
                {
                    StartCoroutine(LockCamandMove(pawn, cam));
                }
                else
                {
                    Debug.Log(other.gameObject.name + "Doesn't have a cam");
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player2")
        {
            foreach (PlayerController controller in GameControl.instance.players)
            {
                controller.TPPlayer2();
            }

        }
    }


    IEnumerator LockCamandMove(TankPawn pawn, Camera cam)
    {
        foreach (PlayerController controller in GameControl.instance.players)
        {
            controller.TPPlayer2();
        }
        //sets stuff to true and false for a few seconds and then the player can resume gameplay
        pawn.owner.LockControls = true;
        goingToNextArea = true;
        yield return new WaitForSeconds(2);
        goingToNextArea = false;
        pawn.owner.LockControls = false;

    }
}
