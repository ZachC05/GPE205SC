using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameControl : MonoBehaviour
{
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

    }

    public void Start()
    {
        roomGenerator = GetComponent<RoomGenerator>();

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




    public void GetAI()
    {
        foreach (AIController ai in AI)
        {
            ai.control = gameObject.GetComponent<GameControl>();
        }
    }

    public void SpawnPlayer()
    {
        spawnPos = GameObject.FindGameObjectWithTag("playerSpawn").gameObject;

        //Spawns the yank and controller prefab
        GameObject playerOj = Instantiate(playerPrefab,CamSpawnPos, Quaternion.LookRotation(CamSpawnRot));
        GameObject tankObj = Instantiate(tankPawnPrefab, spawnPos.transform.position, Quaternion.identity);

        //Gets the controller script from the summoned player copntroller
        Controller playerController = playerOj.GetComponent<Controller>();

        //gets the pawn script from the summoned tank
        Pawn tankpawn = tankObj.GetComponent<Pawn>();

        //assigns the controller to the tank the player can control
        playerController.pawn = tankpawn;

        //assigner the owner player controller
        tankpawn.owner = playerController;
    }


}
