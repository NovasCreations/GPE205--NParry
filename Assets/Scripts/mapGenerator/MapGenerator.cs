using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapGenerator : MonoBehaviour
{
    public GameObject[] gridPrefabs;
    public int rows;
    public int cols;
    public float roomWidth = 50;
    public float roomHeight = 50;
    private Room[,] grid;
    // Start is called before the first frame update
    void Start()
    {
        generateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject RandomRoomPrefab() 
    {
        return gridPrefabs[Random.Range(0, gridPrefabs.Length)];
    }
    public void generateMap()
    {
        grid = new Room[rows, cols];

        for (int currentRow = 0;currentRow < rows;currentRow++)
        {
            for (int currentCol = 0;currentCol < cols;currentCol++)
            {
                float xPosition = currentCol * roomWidth;
                float zPosition = currentRow * roomHeight;
                Vector3 newPosition = new Vector3(xPosition, 0.0f, zPosition);

                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject;
                
                //tempRoomObj.transform.parent = this.transform;

                tempRoomObj.name = "Room_" + currentCol + "," + currentRow;

                Room tempRoom = tempRoomObj.GetComponent<Room>();


                if (currentRow == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (currentRow == rows - 1)
                {
                    tempRoom.doorSouth.SetActive(false);
                }
                else
                {
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }

                if (currentCol == 0)
                {
                    tempRoom.doorEast.SetActive(false);
                }
                else if (currentCol == cols - 1)
                {
                    tempRoom.doorWest.SetActive(false);
                }
                else
                {
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }
                grid[currentCol, currentRow] = tempRoom;
            }
        }
    }
}
