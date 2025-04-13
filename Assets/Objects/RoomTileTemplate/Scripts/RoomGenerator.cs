using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class RoomGenerator : MonoBehaviour
{
    [Header("Menu UI Sound")]
    public AudioSource menuUIAudio;

    [Header("UI Settings")]
    public Toggle SetmapOfTheDay;
    public Toggle SetRandomSeed;
    public Toggle SetSeed;
    public TextMeshPro inputFeild;


    [Header("Seed Settings")]
    public bool isMapOfTheDay;
    public bool RandomSeed;
    public bool setSeed;
    public int mapSeed;


    [Header("Grid Settings")]
    public GameObject[] gridPrefabs;
    public int rows;
    public int cols;
    public float roomWidth = 50.0f;
    public float roomHeight = 50.0f;
    private Room[,] grid;

    [Header("Player Room")]
    //player spawnRoom
    public GameObject playerSpawnRoom;

    //random stuff for testing and securing bugs
    public bool firstMapGenerated;

    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.Random.InitState(DateToInt(DateTime.Now));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Returns a random room
    public GameObject RandomRoomPrefab()
    {
        return gridPrefabs[UnityEngine.Random.Range(0, gridPrefabs.Length)];
    }

    public void ReplaceRoomWithPlayerSpawn()
    {
        //gets the grid number
        int colNum = UnityEngine.Random.Range(0, cols);
        int rowNum = UnityEngine.Random.Range(0, rows);

        //deletes the old grid placement
        GameObject oldRoom = grid[colNum, rowNum].gameObject;
        Destroy(oldRoom);

        // Figure out the location. 
        float xPosition = roomWidth * colNum;
        float zPosition = roomHeight * rowNum;
        Vector3 newPosition = new Vector3(xPosition, 0.0f, zPosition);

        //generates the new room
        GameObject playerSpawn = Instantiate(playerSpawnRoom, newPosition, Quaternion.identity) as GameObject;
        playerSpawn.transform.parent = this.transform;

        //give the room a name
        playerSpawn.name = "Player Spawn Room__ " + colNum + "," + rowNum;

        Room playerRoom = playerSpawn.GetComponent<Room>();

        // Open the doors
        // If we are on the bottom row, open the north door
        if (rowNum == 0)
        {
            playerRoom.doorNorth.SetActive(false);
        }
        else if (rowNum == rows - 1)
        {
            // Otherwise, if we are on the top row, open the south door
            Destroy(playerRoom.doorSouth);
        }
        else
        {
            // Otherwise, we are in the middle, so open both doors
            Destroy(playerRoom.doorNorth);
            Destroy(playerRoom.doorSouth);
        }

        // If we are on the bottom row, open the north door
        if (colNum == 0)
        {
            playerRoom.doorEast.SetActive(false);
        }
        else if (colNum == cols - 1)
        {
            // Otherwise, if we are on the top row, open the south door
            Destroy(playerRoom.doorWest);
        }
        else
        {
            // Otherwise, we are in the middle, so open both doors
            Destroy(playerRoom.doorEast);
            Destroy(playerRoom.doorWest);
        }

        // Save it to the grid array
        grid[colNum, rowNum] = playerRoom;
    }

    public void GenerateMap()
    {
       firstMapGenerated = true;
        if (isMapOfTheDay)
        {
            mapSeed = DateToInt(DateTime.Now.Date);
        }

        if(SetSeed)
        {
            //nothihng needed
        }

        if(RandomSeed)
        {
            int randomSeed = UnityEngine.Random.Range(1, 99999);
            Debug.Log(randomSeed);
            mapSeed = randomSeed;
        }

        Debug.Log("map seed is " + mapSeed);
        UnityEngine.Random.InitState(mapSeed);

        // Clear out the grid - "column" is our X, "row" is our Y
        grid = new Room[cols, rows];

        // For each grid row...
        for (int currentRow = 0; currentRow < rows; currentRow++)
        {
            // for each column in that row
            for (int currentCol = 0; currentCol < cols; currentCol++)
            {
                // Figure out the location. 
                float xPosition = roomWidth * currentCol;
                float zPosition = roomHeight * currentRow;
                Vector3 newPosition = new Vector3(xPosition, 0.0f, zPosition);

                // Create a new grid at the appropriate location
                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;

                // Set its parent
                tempRoomObj.transform.parent = this.transform;

                // Give it a meaningful name
                tempRoomObj.name = "Room_" + currentCol + "," + currentRow;

                // Get the room object
                Room tempRoom = tempRoomObj.GetComponent<Room>();

                // Open the doors
                // If we are on the bottom row, open the north door
                if (currentRow == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (currentRow == rows - 1)
                {
                    // Otherwise, if we are on the top row, open the south door
                    Destroy(tempRoom.doorSouth);
                }
                else
                {
                    // Otherwise, we are in the middle, so open both doors
                    Destroy(tempRoom.doorNorth);
                    Destroy(tempRoom.doorSouth);
                }

                // If we are on the bottom row, open the north door
                if (currentCol == 0)
                {
                    tempRoom.doorEast.SetActive(false);
                }
                else if (currentCol == cols - 1)
                {
                    // Otherwise, if we are on the top row, open the south door
                    Destroy(tempRoom.doorWest);
                }
                else
                {
                    // Otherwise, we are in the middle, so open both doors
                    Destroy(tempRoom.doorEast);
                    Destroy(tempRoom.doorWest);
                }

                // Save it to the grid array
                grid[currentCol, currentRow] = tempRoom;
            }
        }

        //at the end of generating a map, intert a player spawn
        ReplaceRoomWithPlayerSpawn();
    }

    public int DateToInt(DateTime dateToUse)
    {
        // Add our date up and return it
        return dateToUse.Year + dateToUse.Month + dateToUse.Day + dateToUse.Hour + dateToUse.Minute + dateToUse.Second + dateToUse.Millisecond;
    }

    public void DeleteMap()
    {
        if (firstMapGenerated)
        {
            // For each grid row...
            for (int currentRow = 0; currentRow < rows; currentRow++)
            {
                // for each column in that row
                for (int currentCol = 0; currentCol < cols; currentCol++)
                {
                    GameObject tempRoom = grid[currentCol, currentRow].gameObject;

                    Debug.Log(tempRoom + " Is Being Deleted");
                    Destroy(tempRoom.gameObject);
                }
            }
            firstMapGenerated = false;
        }

    }

    public void ToggleMapOfTheDay()
    {
        menuUIAudio.Play();
        if (SetmapOfTheDay.isOn == true)
        {
            Debug.Log("Set it to a daily map");
            isMapOfTheDay = true;

            //sets others to false
            RandomSeed = false;
            setSeed = false;

            //resets the values so that play must have a form of map generation
            SetRandomSeed.isOn = false;
            SetSeed.isOn = false;


            SetRandomSeed.interactable = true;
            SetSeed.interactable = true;

            
        }
        SetmapOfTheDay.interactable = false;
    }

    public void ToggleRandomMap()
    {
        menuUIAudio.Play();
        if (SetRandomSeed.isOn == true)
        {
            Debug.Log("Set it to a random map");
            RandomSeed = true;

            //sets others to false
            isMapOfTheDay = false;
            setSeed = false;

            //resets the values so that play must have a form of map generation
            SetmapOfTheDay.isOn = false;
            SetSeed.isOn = false;


            SetmapOfTheDay.interactable = true;
            SetSeed.interactable = true;


        }
        SetRandomSeed.interactable = false;
    }

    public void ToggleSetSeed()
    {
        menuUIAudio.Play();
        if (SetSeed.isOn == true)
        {
            setSeed = true;

            //sets others to false
            RandomSeed = false;
            isMapOfTheDay = false;

            //resets the values so that play must have a form of map generation
            SetRandomSeed.isOn = false;
            SetmapOfTheDay.isOn = false;


            SetRandomSeed.interactable = true;
            SetmapOfTheDay.interactable = true;


        }
        SetSeed.interactable = false;

        //for now return a random number until figure out
        //COME BACK BEFORE FINAL
    }

    public void getInput(string input)
    {
        mapSeed = int.Parse(input);
        Debug.Log(input);
    }
}
