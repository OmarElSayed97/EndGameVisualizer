using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using System;

public class Grid : MonoBehaviour
{



    [SerializeField]
    TMP_Text Action;

   

    [SerializeField]
    GameObject GridObject;



    //A Group of prefabs used in building the Grid
    [SerializeField]
    GameObject P_Tile;

    [SerializeField]
    GameObject P_Ironman;

    [SerializeField]
    GameObject P_Thanos;

    [SerializeField]
    GameObject P_Warrior;

    [SerializeField]
    GameObject[] P_Stones;


    //Variables where the parsed strig values coming from Java are stored
    private static int GridWidth;
    private static int GridHeight;
    private static Vector2 IronmanPos;
    private static Vector2 ThanosPos;
    private static Vector2[] StonesPos;
    private static Vector2[] WarriorsPos;



    private GameObject GObj_ironman; //Ironman's Gameobject 
    private Animator IronAnim; //Ironman's Animator

    [SerializeField]
    Camera IronmanCamera; //Ironman's ThirdPersonCamera

    [SerializeField]
    Camera MainCamera; //Main camera for the Grid

    [SerializeField]
    Transform[] CameraPositions;



    //String variables where the seperated parsed string coming from Java will be stored into
    private string EndGameString;
    private string[] SeperatedEndGameString;
    private string grid;
    private string path;
    private string[] Actions;

    //A counter that is used to stop the InvokedRepeating method
    private int ActionsCount;
    
    //A 2D array that stores all the tiles of the Grid
    private GameObject[,] GridArray;


    //Variabes used in Lerping Ironman
    private bool IsMoving;
    private float TimeStartedMoving;


    // Start is called before the first frame update
    void Start()
    {
        //Receiving the String from the Clipboard
        EndGameString = GUIUtility.systemCopyBuffer;
        Debug.Log("String copied: " + EndGameString);

        //Seperate the string into 2, Grid and Path
        SeperatedEndGameString = EndGameString.Split('_');

        grid = SeperatedEndGameString[0];
        path = SeperatedEndGameString[1];


        StonesPos = new Vector2[6];
        
        GridArray = new GameObject[100,100];

        

        gridParse(grid);
        AdjustCameraPos();
        CreateGrid(GridWidth, GridHeight);
        PopulateGrid();
        
        Actions = path.Split(',');
        ActionsCount = 0;




        InvokeRepeating("SimulatePath", 3f, 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if(ActionsCount == Actions.Length)
        {
            CancelInvoke();
        }
    }


    void CreateGrid(int Width, int Height)
    {
        for (int i = 0; i < Width; i++)
        {
            for(int j = 0; j < Height; j++)
            {
                if(i == 0 && j == 0)
                {
                    GameObject tile = Instantiate(P_Tile, new Vector3(0, 0, 0), Quaternion.identity);
                    tile.transform.SetParent(GridObject.transform);
                    GridArray[i,j] = tile;
                }
                else
                {
                    GameObject tile = Instantiate(P_Tile, new Vector3(i, 0, j), Quaternion.identity);
                    tile.transform.SetParent(GridObject.transform);
                    GridArray[i,j] = tile;
                }
                
            }
        }
    }


    public void gridParse(string grid)
    {
        string[] gridArray = grid.Split(';');

        string[] gridDimensions = gridArray[0].Split(',');
        string[] ironPos = gridArray[1].Split(',');
        string[] ThanosP = gridArray[2].Split(',');
        string[] stonesPos = gridArray[3].Split(',');
        string[] warriorsPos = gridArray[4].Split(',');

        WarriorsPos = new Vector2[warriorsPos.Length/2];


        GridWidth = int.Parse(gridDimensions[1]);
        GridHeight = int.Parse(gridDimensions[0]);
        IronmanPos = new Vector2(int.Parse(ironPos[0]), int.Parse(ironPos[1]));
        ThanosPos = new Vector2(int.Parse(ThanosP[0]), int.Parse(ThanosP[1]));
        int j = 0;
        for (int i = 0; i < stonesPos.Length; i += 2)
        {
            int tmp1 = int.Parse(stonesPos[i]);
            int tmp2 = int.Parse(stonesPos[i+1]);
            Vector2 tmpPoint = new Vector2(tmp1, tmp2);
            StonesPos[j] = tmpPoint;

            j++;
        }
        j = 0;
        for (int i = 0; i < warriorsPos.Length; i += 2)
        {
            int tmp1 = int.Parse(warriorsPos[i]);
            int tmp2 = int.Parse(warriorsPos[i+1]);
            Vector2 tmpPoint = new Vector2(tmp1, tmp2);
            WarriorsPos[j] = tmpPoint;

            j++;
          
        }
        


    }



    public void PopulateGrid()
    {
        
        GObj_ironman = Instantiate(P_Ironman, new Vector3(0,0, 0),Quaternion.identity);
        GObj_ironman.transform.SetParent(GridArray[(int)IronmanPos.x, (int)IronmanPos.y].transform);
        GObj_ironman.transform.position = GObj_ironman.transform.parent.position;
        IronAnim = GObj_ironman.GetComponent<Animator>();
        IronmanCamera.transform.SetParent(GObj_ironman.transform);

        GameObject thanos = Instantiate(P_Thanos, new Vector3(0, 1f, 0), Quaternion.identity);
        thanos.transform.SetParent(GridArray[(int)ThanosPos.x, (int)ThanosPos.y].transform);
        thanos.transform.position = new Vector3(thanos.transform.parent.position.x, 0 , thanos.transform.parent.position.z);


        for(int i = 0; i<StonesPos.Length; i++)
        {
            GameObject stone = Instantiate(P_Stones[i], new Vector3(0, 0, 0), Quaternion.identity);
            stone.transform.SetParent(GridArray[(int)StonesPos[i].x, (int)StonesPos[i].y].transform);
            stone.transform.position = new Vector3(stone.transform.parent.position.x, 0.5f, stone.transform.parent.position.z);
        }

        
        for (int i = 0; i < WarriorsPos.Length; i++)
        {
            
            GameObject warrior = Instantiate(P_Warrior, new Vector3(0, 0, 0), Quaternion.identity);
            warrior.transform.SetParent(GridArray[(int)WarriorsPos[i].x, (int)WarriorsPos[i].y].transform);
            warrior.transform.position = new Vector3(warrior.transform.parent.position.x, 0, warrior.transform.parent.position.z);
        }

    }


    public void AdjustCameraPos()
    {
        switch (GridWidth)
        {
            case 5:
                MainCamera.transform.position = CameraPositions[0].position;
                MainCamera.transform.rotation = CameraPositions[0].rotation;
                break;
            case 6:
                MainCamera.transform.position = CameraPositions[1].position;
                MainCamera.transform.rotation = CameraPositions[1].rotation;
                break;
            case 7:
                MainCamera.transform.position = CameraPositions[2].position;
                MainCamera.transform.rotation = CameraPositions[2].rotation;
                break;
            case 8:
                MainCamera.transform.position = CameraPositions[3].position;
                MainCamera.transform.rotation = CameraPositions[3].rotation;
                break;
            case 9:
                MainCamera.transform.position = CameraPositions[4].position;
                MainCamera.transform.rotation = CameraPositions[4].rotation;
                break;

            case 10:
                MainCamera.transform.position = CameraPositions[5].position;
                MainCamera.transform.rotation = CameraPositions[5].rotation;
                break;

            case 11:

                MainCamera.transform.position = CameraPositions[6].position;
                MainCamera.transform.rotation = CameraPositions[6].rotation;
                break;

            case 12:
                MainCamera.transform.position = CameraPositions[7].position;
                MainCamera.transform.rotation = CameraPositions[7].rotation;
                break;
            case 13:
                MainCamera.transform.position = CameraPositions[8].position;
                MainCamera.transform.rotation = CameraPositions[8].rotation;
                break;
            case 14:
                MainCamera.transform.position = CameraPositions[9].position;
                MainCamera.transform.rotation = CameraPositions[9].rotation;
                break;
            case 15:
                MainCamera.transform.position = CameraPositions[10].position;
                MainCamera.transform.rotation = CameraPositions[10].rotation;
                break;
                
        }
    }


    public void SimulatePath()
    {
        Transform Tile;
        string action = GetAction(Actions[ActionsCount]);

        switch (action)
            {
                case "RIGHT":
                    IronmanPos.y += 1;
                    GObj_ironman.transform.SetParent(GridArray[(int)IronmanPos.x, (int)IronmanPos.y].transform);
                    GObj_ironman.transform.localRotation = Quaternion.Euler(0, 0, 0);
                StartMoving();
                    break;
                case "LEFT":
                    IronmanPos.y -= 1;
                    GObj_ironman.transform.SetParent(GridArray[(int)IronmanPos.x, (int)IronmanPos.y].transform);
                    GObj_ironman.transform.localRotation = Quaternion.Euler(0, 180, 0);
                StartMoving();
                break;
                case "UP":
                    IronmanPos.x -= 1;
                    GObj_ironman.transform.SetParent(GridArray[(int)IronmanPos.x, (int)IronmanPos.y].transform);
                    GObj_ironman.transform.localRotation = Quaternion.Euler(0, -90, 0);
                StartMoving();
                break;
                case "DOWN":
                    IronmanPos.x += 1;
                    GObj_ironman.transform.SetParent(GridArray[(int)IronmanPos.x, (int)IronmanPos.y].transform);
                    GObj_ironman.transform.localRotation = Quaternion.Euler(0, 90, 0);
                StartMoving();
                break;
                case "KILL":
                    Kill();
                   


                break;
                case "COLLECT":
                    Tile = GridArray[(int)IronmanPos.x, (int)IronmanPos.y].transform;
                    foreach(Transform child in Tile)
                    {
                        if (child.tag != "Player")
                        {
                            Destroy(child.gameObject);
                            //DealDmg
                        }
                    }
                    break;
                case "SNAP":
                    Tile = GridArray[(int)IronmanPos.x, (int)IronmanPos.y].transform;
                    foreach (Transform child in Tile)
                    {
                        if (child.tag != "Player")
                        {
                            Destroy(child.gameObject);
                        }
                    }
                break;
            }
        ActionsCount++;
        
    }


    public string GetAction(string action)
    {
        string rightAction = "";
        
        action = action.ToUpper();

        if (action.Contains("RIGHT"))
        {
            rightAction = "RIGHT";
        }
        if (action.Contains("UP"))
        {
            rightAction = "UP";
        }
        if (action.Contains("LEFT"))
        {
            rightAction = "LEFT";
        }
        if (action.Contains("DOWN"))
        {
            rightAction = "DOWN";
        }
        if (action.Contains("KILL"))
        {
            rightAction = "KILL";
        }
        if (action.Contains("COLLECT"))
        {
            rightAction = "COLLECT";
        }
        if (action.Contains("SNAP"))
        {
            rightAction = "SNAP";
        }

        Action.text = "Action:   " + rightAction;
        return rightAction;


    }



    public void GetAdjacentWarriors()
    {
        if(GridArray[(int)IronmanPos.x + 1, (int)IronmanPos.y].transform.childCount == 1)
        {
            Transform Tile = GridArray[(int)IronmanPos.x + 1, (int)IronmanPos.y].transform;
            foreach (Transform child in Tile)
            {
                if (child.tag == "Warrior")
                {
                    
                }
            }
        }

        if (GridArray[(int)IronmanPos.x - 1, (int)IronmanPos.y].transform.childCount == 1)
        {
            Transform Tile = GridArray[(int)IronmanPos.x - 1, (int)IronmanPos.y].transform;
            foreach (Transform child in Tile)
            {
                if (child.tag == "Warrior")
                {

                }
            }
        }

        if (GridArray[(int)IronmanPos.x , (int)IronmanPos.y+1].transform.childCount == 1)
        {
            Transform Tile = GridArray[(int)IronmanPos.x, (int)IronmanPos.y + 1].transform;
            foreach (Transform child in Tile)
            {
                if (child.tag == "Warrior")
                {

                }
            }
        }

        if (GridArray[(int)IronmanPos.x, (int)IronmanPos.y-1].transform.childCount == 1)
        {
            Transform Tile = GridArray[(int)IronmanPos.x, (int)IronmanPos.y - 1].transform;
            foreach (Transform child in Tile)
            {
                if (child.tag == "Warrior")
                {

                }
            }
        }


    }

    public void Kill()
    {
        if((int)IronmanPos.x + 1 < GridHeight)
        {
            if (GridArray[(int)IronmanPos.x + 1, (int)IronmanPos.y].transform.childCount == 1)
            {
                Transform Tile = GridArray[(int)IronmanPos.x + 1, (int)IronmanPos.y].transform;
                foreach (Transform child in Tile)
                {
                    if (child.tag == "Warrior")
                    {
                        Destroy(child.gameObject);
                        //child.gameObject.GetComponent<Warriors>().Die();
                    }
                }
            }
        }

        if ((int)IronmanPos.x - 1 >= 0)
        {
            if (GridArray[(int)IronmanPos.x - 1, (int)IronmanPos.y].transform.childCount == 1)
            {
                Transform Tile = GridArray[(int)IronmanPos.x - 1, (int)IronmanPos.y].transform;
                foreach (Transform child in Tile)
                {
                    if (child.tag == "Warrior")
                    {
                        Destroy(child.gameObject);
                        //child.gameObject.GetComponent<Warriors>().Die();
                    }
                }
            }
        }
        

        if ((int)IronmanPos.y + 1 < GridHeight)
        {
            if (GridArray[(int)IronmanPos.x, (int)IronmanPos.y + 1].transform.childCount == 1)
            {
                Transform Tile = GridArray[(int)IronmanPos.x, (int)IronmanPos.y + 1].transform;
                foreach (Transform child in Tile)
                {
                    if (child.tag == "Warrior")
                    {
                        Destroy(child.gameObject);
                        //child.gameObject.GetComponent<Warriors>().Die();
                    }
                }
            }
        }

        if ((int)IronmanPos.y - 1 >= 0)
        {
            if (GridArray[(int)IronmanPos.x, (int)IronmanPos.y - 1].transform.childCount == 1)
            {
                Transform Tile = GridArray[(int)IronmanPos.x, (int)IronmanPos.y - 1].transform;
                foreach (Transform child in Tile)
                {
                    if (child.tag == "Warrior")
                    {
                        Destroy(child.gameObject);
                        //child.gameObject.GetComponent<Warriors>().Die();
                    }
                }
            }
        }
       
    }

    void StartMoving()
    {
        IsMoving = true;
        TimeStartedMoving = Time.time;
    }

    private void FixedUpdate()
    {
        if (IsMoving)
        {
            float TimeSinceStarted = Time.time - TimeStartedMoving;
            float PercentageCompleted = TimeSinceStarted / 10f;
            Debug.Log(PercentageCompleted);
            GObj_ironman.transform.position = Vector3.Lerp(GObj_ironman.transform.position, GObj_ironman.transform.parent.position, PercentageCompleted);

            if (PercentageCompleted >= 1)
            {
                IsMoving = false;
            }

        }

       
    }

}
