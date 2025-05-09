
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameControl : MonoBehaviour
{
    [Header("Menu UI Sound")]
    public AudioSource menuUIAudio;


    [Header("State Scenes")]
    //Game States
    public GameObject titleScreen;
    public GameObject mainMenuScreen;
    public GameObject creditsScreen;
    public GameObject optionsSccreen;
    public GameObject gameplayScreen;
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;

    [Header("Camera")]
    public GameObject tempCamera;

    [Header("Key Inputs DEVELOPER")]
    //Keys To Transfer
    public KeyCode titleScreenKey;
    public KeyCode mainMenuScreenKey;
    public KeyCode creditsScreenKey;
    public KeyCode OptionsSccreenKey;
    public KeyCode gameplayScreenKey;
    public KeyCode gameOverScreenKey;

    //this game instnace
    public static GameControl instance;

    //what the controller and tanks are
    public GameObject playerPrefab;
    public GameObject tankPawnPrefab;

    public GameObject spawnPos;


    [Header("2 player mode")]
    public Toggle TwoPlayerModeToggle;
    public bool twoPlayerMode;
    public GameObject playerPrefab2;
    public GameObject player2TankFab;
    public GameObject player2UIItems;

    //start poit of the tank
    public List<PlayerController> players;
    public List<AIController> AI;

    public Vector3 CamSpawnPos;
    public Vector3 CamSpawnRot;

    RoomGenerator roomGenerator;
    //So scout can reveal the players location
    public bool playerSeenByScout;
    // Start is called before the first frame update

    public bool gameActive = false;


    [SerializeField] int aiAmount = 0;
    
    
    
    public getCamera ui;


    public void Awake()
    {

        //if another instance of this object exist, this will be destroyed
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        players = new List<PlayerController>();
        AI = new List<AIController>();
        roomGenerator = GetComponent<RoomGenerator>();
    }

    private void Update()
    {
        //checks if everyone is dead, if so the game ends
        if (players.Count == 0 && gameActive == true)
        {
            GameOverScreenTransfer();
        }

        if(AI.Count == 0 && gameActive == true)
        {
            GameWinScreenTransfer();
        }

        //adjusts all of the ai to see the player
        foreach (AIController ai in AI)
        {
            if (playerSeenByScout)
            {
                ai.scoutCanSee = true;
            }
            else
            {
                ai.scoutCanSee = false;
            }
        }

        if (Input.GetKeyUp(titleScreenKey))
        {
            TitleScreenTransfer();
        }

    }

    public void Start()
    {
        TitleScreenTransfer();
    }
    


    public void GetAI()
    {
        foreach (AIController ai in AI)
        {
            ai.control = gameObject.GetComponent<GameControl>();
            aiAmount++;
        }
    }

    public void SpawnPlayer()
    {
        spawnPos = GameObject.FindGameObjectWithTag("playerSpawn").gameObject;

        //Spawns the yank and controller prefab
        GameObject playerCon = Instantiate(playerPrefab,CamSpawnPos, Quaternion.LookRotation(CamSpawnRot));
        GameObject player = Instantiate(tankPawnPrefab, spawnPos.transform.position, Quaternion.identity);

        //Gets the controller script from the summoned player copntroller
        Controller playerController = playerCon.GetComponent<Controller>();

        //gets the pawn script from the summoned tank
        Pawn tankpawn = player.GetComponent<Pawn>();

        //assigns the controller to the tank the player can control
        playerController.pawn = tankpawn;

        //assigner the owner player controller
        tankpawn.owner = playerController;

        playerCon.GetComponent<PlayerController>().pointsText = ui.pointsPlayer1;
        playerCon.GetComponent<PlayerController>().livesText = ui.livesPlayer1;
        playerCon.GetComponent<PlayerController>().healthText = ui.healthPlayer1;


    }

    public void SpawnPlayer2()
    {
        spawnPos = GameObject.FindGameObjectWithTag("playerSpawn").gameObject;

        //Spawns the yank and controller prefab
        GameObject playerCon = Instantiate(playerPrefab2, CamSpawnPos, Quaternion.LookRotation(CamSpawnRot));
        GameObject player = Instantiate(player2TankFab, spawnPos.transform.position, Quaternion.identity);

        //Gets the controller script from the summoned player copntroller
        Controller playerController = playerCon.GetComponent<Controller>();

        //gets the pawn script from the summoned tank
        Pawn tankpawn = player.GetComponent<Pawn>();

        //assigns the controller to the tank the player can control
        playerController.pawn = tankpawn;

        //assigner the owner player controller
        tankpawn.owner = playerController;

        playerCon.GetComponent<PlayerController>().pointsText = ui.pointsPlayer2;
        playerCon.GetComponent<PlayerController>().livesText = ui.livesPlayer2;
        playerCon.GetComponent<PlayerController>().healthText = ui.healthPlayer2;

    }


    //Key Code State Transfers

    public void DeactivateAllStates()
    {
        gameActive = false;
        titleScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        optionsSccreen.SetActive(false);
        creditsScreen.SetActive(false);
        gameplayScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        gameWinScreen.SetActive(false);
        
        if (AI.Capacity > 0)
        {
            foreach (AIController ai in AI)
            {
                
                ai.DestroyEverything();
            }
            AI.Clear();
            
        }
        if(players.Count > 0)
        {
            foreach(PlayerController player in players)
            {
                Destroy(player.pawn.gameObject);
                Destroy(player.gameObject);
            }
        }



        roomGenerator.DeleteMap();
    }

    public void TitleScreenTransfer()
    {
        menuUIAudio.Play();
        DeactivateAllStates();
        titleScreen.SetActive(true);
        tempCamera.SetActive(true);
    }
    public void MainMenuSreenTransfer()
    {
        menuUIAudio.Play();
        DeactivateAllStates();
        mainMenuScreen.SetActive(true);
        tempCamera.SetActive(true);
    }
    public void OptionsScreenTransfer()
    {
        menuUIAudio.Play();
        DeactivateAllStates();
        optionsSccreen.SetActive(true);
        tempCamera.SetActive(true);
    }
    public void CreditsScreenTransfer()
    {
        menuUIAudio.Play();
        DeactivateAllStates();
        creditsScreen.SetActive(true);
        tempCamera.SetActive(true);
    }
    public void GameWinScreenTransfer()
    {
        menuUIAudio.Play();
        DeactivateAllStates();
        gameWinScreen.SetActive(true);
        tempCamera.SetActive(true);
    }
    public void GameplayScreenTransfer()
    {
        menuUIAudio.Play();
        DeactivateAllStates();
        gameplayScreen.SetActive(true);
        tempCamera.SetActive(false);


        //Generates Map
        roomGenerator.GenerateMap();

        //Sets The Camera Position
        CamSpawnPos.x = GameObject.FindGameObjectWithTag("playerSpawn").gameObject.transform.position.x;
        CamSpawnPos.z = GameObject.FindGameObjectWithTag("playerSpawn").gameObject.transform.position.z;

        //Spawns Player
        SpawnPlayer();
        if (twoPlayerMode)
        {
            SpawnPlayer2();
            player2UIItems.SetActive(true);
        }
        else
        {
            player2UIItems.SetActive(false);
        }

        //Gets the AI
        GetAI();



    }
    public void GameOverScreenTransfer()
    {
        menuUIAudio.Play();
        DeactivateAllStates();
        gameOverScreen.SetActive(true);
        tempCamera.SetActive(true);
    }
    public void QuitApp()
    {
        menuUIAudio.Play();
        Application.Quit();
        Debug.Log("Player Quit");
    }

    public void Activate2PlayerMode()
    {
        menuUIAudio.Play();
        if(TwoPlayerModeToggle.isOn == true)
        {
            twoPlayerMode = true;
        }
        else
        {
            twoPlayerMode = false;
        }
    }
    private void OnApplicationQuit()
    {
       
    }



}
