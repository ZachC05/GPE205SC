using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [Header("State Scenes")]
    //Game States
    public GameObject titleScreen;
    public GameObject mainMenuScreen;
    public GameObject creditsScreen;
    public GameObject optionsSccreen;
    public GameObject gameplayScreen;
    public GameObject gameOverScreen;

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

    //start poit of the tank
    public List<PlayerController> players;
    public List<AIController> AI;

    public Vector3 CamSpawnPos;
    public Vector3 CamSpawnRot;

    RoomGenerator roomGenerator;
    //So scout can reveal the players location
    public bool playerSeenByScout;
    // Start is called before the first frame update


    [SerializeField] int aiAmount = 0;


    [Header("Player")]
    GameObject player;
    GameObject playerCon;
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
        if (Input.GetKeyDown(titleScreenKey))
        {
            TitleScreenTransfer();
        }
        if (Input.GetKeyDown(mainMenuScreenKey))
        {
            MainMenuSreenTransfer();
        }
        if (Input.GetKeyDown(OptionsSccreenKey))
        {
            OptionsScreenTransfer();
        }
        if (Input.GetKeyDown(creditsScreenKey))
        {
            CreditsScreenTransfer();
        }
        if (Input.GetKeyDown(gameplayScreenKey))
        {
            GameplayScreenTransfer();
        }
        if (Input.GetKeyDown(gameOverScreenKey))
        {
            GameOverScreenTransfer();
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
        playerCon = Instantiate(playerPrefab,CamSpawnPos, Quaternion.LookRotation(CamSpawnRot));
        player = Instantiate(tankPawnPrefab, spawnPos.transform.position, Quaternion.identity);

        //Gets the controller script from the summoned player copntroller
        Controller playerController = playerCon.GetComponent<Controller>();

        //gets the pawn script from the summoned tank
        Pawn tankpawn = player.GetComponent<Pawn>();

        //assigns the controller to the tank the player can control
        playerController.pawn = tankpawn;

        //assigner the owner player controller
        tankpawn.owner = playerController;
    }


    //Key Code State Transfers

    public void DeactivateAllStates()
    {
        titleScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        optionsSccreen.SetActive(false);
        creditsScreen.SetActive(false);
        gameplayScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        
        if (AI.Capacity > 0)
        {
            foreach (AIController ai in AI)
            {
                
                ai.DestroyEverything();
            }
            AI.Clear();
            
        }
        if (playerCon != null)
        {
            Destroy(playerCon);
        }
        if(player != null)
        {
            Destroy(player);
        }

        roomGenerator.DeleteMap();
    }

    public void TitleScreenTransfer()
    {
        DeactivateAllStates();
        titleScreen.SetActive(true);
        tempCamera.SetActive(true);
    }
    public void MainMenuSreenTransfer()
    {
        DeactivateAllStates();
        mainMenuScreen.SetActive(true);
        tempCamera.SetActive(true);
    }
    public void OptionsScreenTransfer()
    {
        DeactivateAllStates();
        optionsSccreen.SetActive(true);
        tempCamera.SetActive(true);
    }
    public void CreditsScreenTransfer()
    {
        DeactivateAllStates();
        creditsScreen.SetActive(true);
        tempCamera.SetActive(true);
    }
    public void GameplayScreenTransfer()
    {
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

        //Gets the AI
        GetAI();
    }
    public void GameOverScreenTransfer()
    {
        DeactivateAllStates();
        gameOverScreen.SetActive(true);
        tempCamera.SetActive(true);
    }
    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Player Quit");
    }
    private void OnApplicationQuit()
    {
       
    }

}
