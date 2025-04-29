using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : Controller
{
    //players own canvas and carp
    [Header("Players Canvas Settings")]
    public GameObject PlayerOwnedCanvas;
    public TMP_Text pointsText;
    public TMP_Text livesText;
    public TMP_Text healthText;
    GameObject permCanvas;


    //Keys are use to get input from players
    [Header("Keys for Moving")]
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;

    [Header("Keys for Rotations")]
    public KeyCode rotateRightKey;
    public KeyCode rotateLeftKey;

    [Header("Keys for Shooting || Can left click to shoot")]
    public KeyCode shootKey;


    public bool player2;


    // Start is called before the first frame update
    public override void Start()
    {

        //If we have a GameManager
        if(GameControl.instance != null)
        {
            //track the players
            if(GameControl.instance.players != null)
            {
                //Add to manager
                GameControl.instance.players.Add(this);
                GameControl.instance.gameActive = true;
            }
        }



        //makes sure the UI exists
        if (pointsText != null && livesText != null)
        {
            UpdatePersonalUI();
        }
        else
        {
            Debug.Log("Please enter a UI for the player");
        }
        base.Start();

        

    }

    // Update is called once per frame
    public override void Update()
    {
        //locks controls if true
        if (!LockControls)
        {
            GetInputs();
        }
        if (pawn == null)
        {
            GameControl.instance.players.Remove(this);
            Destroy(gameObject);
        }
        base.Update();
    }

    public override void GetInputs()
    {
        if(pawn != null)
        {
            //gets inputs from player and communicates it to the pawn scripts and makes noise
            if (Input.GetKey(moveForwardKey))
            {
                pawn.MoveForward();
                pawn.noiseMaker.noiseDistance = 5;
            }
            if (Input.GetKey(moveBackwardKey))
            {
                pawn.MoveBackward();
                pawn.noiseMaker.noiseDistance = 5;
            }
            if (Input.GetKey(rotateRightKey))
            {
                pawn.RotateRight();
                pawn.noiseMaker.noiseDistance = 5;
            }
            if (Input.GetKey(rotateLeftKey))
            {
                pawn.RotateLeft();
                pawn.noiseMaker.noiseDistance = 5;
            }
            //Gets rid of the noise right when the players stops
            if (Input.GetKeyUp(moveForwardKey))
            {
                pawn.noiseMaker.noiseDistance = 0;
            }
            if (Input.GetKeyUp(moveBackwardKey))
            {
                pawn.noiseMaker.noiseDistance = 0;
            }
            if (Input.GetKeyUp(rotateRightKey))
            {
                pawn.noiseMaker.noiseDistance = 0;
            }
            if (Input.GetKeyUp(rotateLeftKey))
            {
                pawn.noiseMaker.noiseDistance = 0;
            }

            //when the player shoots start the noise made function
            if (Input.GetKey(shootKey))
            {
                pawn.Shoot();
                StartCoroutine(noiseMade());
            }
        }

    }

    //a function that makes the noise delay for a few before going away
    IEnumerator noiseMade()
    {
        pawn.noiseMaker.noiseDistance = 10;
        yield return new WaitForSeconds(0.5f);
        pawn.noiseMaker.noiseDistance = 0;
    }

    public void OnDestroy()
    {
        //check if gamemanager
        if (GameControl.instance != null)
        {
            //and is tracking players
            if (GameControl.instance.players != null)
            {
                //remove from list
                GameControl.instance.players.Remove(this);
                if(pawn != null)
                {
                    Destroy(pawn);
                }

            }
        }
    }

    public override void AddPoints(int pointsAmount)
    {
        Debug.Log(gameObject.name + " got " + pointsAmount);
        points += pointsAmount;
        UpdatePersonalUI();
    }

    public override void RemovePoints(int pointsAmount)
    {
        points -= pointsAmount;
        UpdatePersonalUI();
    }

    public override void UpdatePersonalUI()
    {
        if(livesText != null && pointsText != null)
        {
            pointsText.text = "Points: " + points;
            livesText.text = "Lives: " + lives;
            healthText.text = pawn.GetComponent<Health>().currentHealth.ToString() + "/" + pawn.GetComponent<Health>().maxHealth.ToString();
        }

    }

    public override void RemvoeLives()
    {
        lives--;
        UpdatePersonalUI();
    }

    public void TPPlayer2()
    {
        if (player2)
        {
            pawn.transform.position = GameControl.instance.players[0].pawn.transform.position;
        }
    }

    

    public override void ResapawnPlayer()
    {
        
    }
}
