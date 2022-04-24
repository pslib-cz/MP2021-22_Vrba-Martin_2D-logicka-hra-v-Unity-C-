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

    CameraControl cc;

    TextAsset[] levelTexts;

    KeyCode skip;


    void Start()
    {
        pauseScreen = FindObjectOfType<PauseScreen>();
        cc = FindObjectOfType<CameraControl>();
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
        skip = KeyCode.None;

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


        /*int smallestX = 0;
        int biggestX = 0;
        int smallestY = 0;
        int biggestY = 0;*/

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
                case TileType.Ice:
                    newEntity = new Ice(constructor, newObject);
                    break;
                default:
                    throw new System.NotImplementedException("unknown tile type");

            }

            /*
            if (newEntity.Position.x < smallestX)
            {
                smallestX = newEntity.Position.x;
            }
            if (newEntity.Position.x > biggestX)
            {
                biggestX = newEntity.Position.x;
            }
            if (newEntity.Position.y < smallestY)
            {
                smallestY = newEntity.Position.y;
            }
            if (newEntity.Position.y > biggestY)
            {
                biggestY = newEntity.Position.y;
            }*/

            Environment.Add(newObject);
            levelState.Add(newEntity);

        }

        //cc.CenterCamera(smallestX, biggestX, smallestY, biggestY);


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
            bool skipped = false;

            if (!(Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.K)|| Input.GetKeyDown(KeyCode.I)|| Input.GetKeyDown(KeyCode.P)))
            {

            }

            if (Input.GetKeyDown(KeyCode.S))
                skip = KeyCode.S;
            if (Input.GetKeyDown(KeyCode.K) && skip == KeyCode.S)
                skip = KeyCode.K;
            if (Input.GetKeyDown(KeyCode.I) && skip == KeyCode.K)
                skip = KeyCode.I;
            if (Input.GetKeyDown(KeyCode.P) && skip == KeyCode.I)
                skipped = true; 


            if (Input.GetKeyDown(KeyCode.Escape))
                return;

            if (level.Solved || skipped)//checking here so that there is one frame before next level
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
        foreach (Ice ice in level.Ices)
        {
            RenderTile(ice, z);
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

        void RenderTile(Entity entity, int z)
        {
            entity.MappedObject.transform.position = (new Vector3(entity.Position.x, entity.Position.y, z)-level.PositionOffset) * tileSize * level.TileZoom;
            entity.MappedObject.transform.localScale = new Vector3(level.TileZoom, level.TileZoom);

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

    

}
