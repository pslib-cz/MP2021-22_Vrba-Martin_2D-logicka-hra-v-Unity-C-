using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    const string pathToLevels = "Assets/TimeGameAssets/Levels/";

    Stack<LevelState> History;
    LevelState level;

    const float tileSize = 1.28f;

    public List<TextAsset> levels;

	public int LevelID { get; set; }
    public bool LevelLoaded { get; set; }
    private TextAsset LevelTextAsset { get; set; }
    public string LevelName { get; set; }

    public GameObject prefabFloor;
    public GameObject prefabPlayer; 
    public GameObject prefabWall;
    public GameObject prefabBox;

    public List<GameObject> Environment;

    Reader reader;
    void Start()
    {
        reader = Object.FindObjectOfType<Reader>();
        History = new Stack<LevelState>();
    }

    void LoadLevel(string levelname)
	{
		foreach (GameObject thing in Environment) //clear the level
		{
            Destroy(thing);
		}
        SavedLevel savedLevel = reader.read(levelname);
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
				case TileType.Button:
					break;
				default:
                    throw new System.NotImplementedException("unknown tile type");
			}

            
            Environment.Add(newObject);
            levelState.Add(newEntity);

		}

        level = levelState;

        Render();
	}


    // Update is called once per frame
    void Update()
    {
		if (Input.anyKeyDown)
		{
            Direction pressed = Direction.None;
            bool doTick = false;
            //Debug.Log("pressed something");

            #region DEBUG
            if (Input.GetKeyDown(KeyCode.I))
            {
                LoadLevel("sus");
            }

            if (Input.GetKeyDown(KeyCode.O))
			{
                LoadLevel("boxtest");
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                LoadLevel("cool");
            }
            #endregion


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

			if (doTick)
			{
                Tick(pressed);
			}
        }
    }

    void Tick(Direction pressed)
	{
        History.Push(new LevelState(level));
        level.Player.PerformTick(level, pressed);
		foreach (Button button in level.Buttons)
		{
            button.PerformTick(level, pressed);
		}
        Render();
	}

    void Render()
	{
        int z = 0; //floor level
		foreach (Floor floor in level.Floors)
		{
            floor.MappedObject.transform.position = new Vector3(floor.Position.x, floor.Position.y, z) * tileSize;
		}


        z = -1; //object level
        level.Player.MappedObject.transform.position = new Vector3(level.Player.Position.x, level.Player.Position.y, z) * tileSize;
        level.Player.UpdateSprite();
        
        foreach (Wall wall in level.Walls)
		{
            wall.MappedObject.transform.position = new Vector3(wall.Position.x, wall.Position.y, z) * tileSize;
		}
        
        foreach (Box box in level.Boxes)
		{
            box.MappedObject.transform.position = new Vector3(box.Position.x, box.Position.y, z) * tileSize;
		}

	}

}
