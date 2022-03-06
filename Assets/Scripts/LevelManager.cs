using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    const string path = "Levels/";

    Stack<LevelState> History;
    LevelState level;

    const float tileSize = 1.28f;

    public int LevelID { get; set; }
    public bool LevelLoaded { get; set; }
    public string LevelName { get; set; }

    public GameObject prefabEntity;

    public List<GameObject> Environment;
    public SpriteRenderer EndScreen;

    private PauseScreen pauseScreen;
    private SoundManager sound;


    TextAsset[] levelTexts;
    void Start()
    {
        pauseScreen = FindObjectOfType<PauseScreen>();
        EndScreen.enabled = false;
        History = new Stack<LevelState>();
        ReadAllLevels();
        LoadLevel(0);



    }

    void ReadAllLevels()
    {
        levelTexts = Resources.LoadAll<TextAsset>(path);

    }

    bool LoadLevel(int id)
    {
        if (id < 0 || levelTexts.Length <= id)
        {
            return false;
        }

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

            GameObject newObject = Instantiate(prefabEntity);

            Entity newEntity = null;
            switch (constructor.T)
            {
                case TileType.Floor:
                    newEntity = new Floor(constructor, newObject);
                    break;

                case TileType.Wall:
                    newEntity = new Wall(constructor, newObject);
                    break;

                case TileType.Player:
                    newEntity = new Player(constructor, newObject, levelState);
                    break;

                case TileType.Box:
                    newEntity = new Box(constructor, newObject, levelState);
                    break;
                case TileType.Storage:
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
        return true;
    }


    void Update()
    {
        if (sound == null)
        {
            if (SoundManager.DoneLoading)
            {
                sound = FindObjectOfType<SoundManager>();
                sound.PlayMusic("grid");
            }
        }


        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                return;

            if (level.Solved)//checking here so that there is one frame before next level
            {
                if (!LoadLevel(LevelID + 1))
                    EndScreen.enabled = true;
                
                return;
            }

            Direction pressed = Direction.None;
            bool doTick = false;
           


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
        if (History.Count > 0)
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

        if (level.Solved)
        {
            sound.Play("solved");
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
