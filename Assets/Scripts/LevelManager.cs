using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    const string pathToLevels = "Assets/TimeGameAssets/Levels/";

    Stack<LevelState> History;
    LevelState level;

    const float tileSize = 1.28f;

    public int LevelID { get; set; }
    public bool LevelLoaded { get; set; }
    private TextAsset LevelTextAsset { get; set; }
    public string LevelName { get; set; }

    public GameObject prefabFloor;
    public GameObject prefabPlayer;
    public GameObject prefabWall;
    public GameObject prefabBox;
    public GameObject prefabStorage;

    public List<GameObject> Environment;

    private PauseScreen pauseScreen;

    TextAsset[] levelTexts;
    //Reader reader;
    void Start()
    {
        //reader = Object.FindObjectOfType<Reader>();
        pauseScreen = FindObjectOfType<PauseScreen>();
        History = new Stack<LevelState>();
        ReadAllLevels();
        LoadLevel(0);
    }

    void ReadAllLevels()
    {
        levelTexts = Resources.LoadAll<TextAsset>("Levels/");

    }

    //void LoadLevel(string levelname)
    void LoadLevel(int id)
    {
        History = new Stack<LevelState>();

        foreach (GameObject thing in Environment) //clear the level
        {
            Destroy(thing);
        }
        SavedLevel savedLevel;
        try
        {
            savedLevel = JsonUtility.FromJson<SavedLevel>(levelTexts[id].text);
        }
        catch (System.Exception)
        {
            throw;
        }
        LevelState levelState = new LevelState();

        for (int i = 0; i < savedLevel.Entities.Length; i++)
        {
            EntityConstructor constructor = savedLevel.Entities[i];

            Entity newEntity = null;
            GameObject newObject = null;
            switch (constructor.T)
            {
                case TileType.Floor:
                    newObject = Instantiate(prefabFloor);
                    newEntity = new Floor(constructor, newObject);
                    break;

                case TileType.Wall:
                    newObject = Instantiate(prefabWall);
                    newEntity = new Wall(constructor, newObject);
                    break;

                case TileType.Player:
                    newObject = Instantiate(prefabPlayer);
                    newEntity = new Player(constructor, newObject, levelState);
                    break;

                case TileType.Box:
                    newObject = Instantiate(prefabBox);
                    newEntity = new Box(constructor, newObject, levelState);
                    break;
                case TileType.Storage:
                    newObject = Instantiate(prefabStorage);
                    newEntity = new Storage(constructor, newObject, levelState);
                    break;
                default:
                    throw new System.NotImplementedException("unknown tile type");
            }


            Environment.Add(newObject);
            levelState.Add(newEntity);

        }

        level = levelState;

        LevelID = id;

        Render();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                return;

            if (level.Solved)//checking here so that there is one frame before next level
            {
                //Debug.Log("THE LEVEL IS SOLVED");
                LoadLevel(LevelID + 1);
                return;
            }

            Direction pressed = Direction.None;
            bool doTick = false;
            //Debug.Log("pressed something");

            /*#region DEBUG
            if (Input.GetKeyDown(KeyCode.I))
            {
                LoadLevel("94_11");
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                LoadLevel("81_25");
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                LoadLevel("minicosmos_01");
            }
            #endregion*/


            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //Debug.Log("pressed up");
                pressed = Direction.Up;
                doTick = true;
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //Debug.Log("pressed down");
                pressed = Direction.Down;
                doTick = true;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //Debug.Log("pressed left");
                pressed = Direction.Left;
                doTick = true;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                //Debug.Log("pressed right");
                pressed = Direction.Right;
                doTick = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Debug.Log("pressed space");
                pressed = Direction.None;
                doTick = true;
            }

            //undo
            if (Input.GetKeyDown(KeyCode.Z))
            {
                doTick = false;
                Undo();
            }

            //restart
            if (Input.GetKeyDown(KeyCode.R))
            {
                doTick = false;
                Restart();
            }

            if (doTick)
            {
                Tick(pressed);
            }
        }
    }

    void Undo()
    {
        if (History.Count>0)
        {
            level = History.Pop();
            Render();
        }
    }

    void Restart()
    {
        LoadLevel(LevelID);//loads current level again
    }

    void Tick(Direction pressed)
    {


        History.Push(level);
        level = level.Copy();

        level.Player.PerformTick(pressed);
        foreach (Button button in level.Buttons)
        {
            button.PerformTick(pressed);
        }

        Render();

    }

    void Render()
    {
        int z = 0; //floor level
        foreach (Floor floor in level.Floors)
        {
            RenderTile(floor, z);
        }


        z = -1; //object level
        RenderTile(level.Player, z);

        foreach (Wall wall in level.Walls)
        {
            RenderTile(wall, z);
        }

        foreach (Box box in level.Boxes)
        {
            RenderTile(box, z);
        }

        foreach (Storage storage in level.Storages)
        {
            RenderTile(storage, z);
        }

    }

    void RenderTile(Entity entity, int z)
    {
        entity.MappedObject.transform.position = new Vector3(entity.Position.x, entity.Position.y, z) * tileSize;

        try
        {
            entity.UpdateSprite();
        }
        catch (System.NotImplementedException)
        {
            Debug.Log(entity + " " +
                "doesnt have sprite update method");
        }


    }

}
