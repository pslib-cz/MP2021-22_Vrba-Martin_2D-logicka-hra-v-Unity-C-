using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    const string pathToLevels = "Assets/TimeGameAssets/Levels/";

    const char rowDivider = ';';
    const char colDivider = ',';

    const string textWall = "w";
    const string textFloor = "f";
    const string textPlayer = "p";

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

    public List<GameObject> Environment;
    // Start is called before the first frame update
    void Start()
    {
        LoadLevel(1);
        History = new Stack<LevelState>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.anyKeyDown)
		{
            Direction pressed = Direction.None;
            bool doTick = false;

			if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                pressed = Direction.Up;
                doTick = true;
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                pressed = Direction.Down;
                doTick = true;
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                pressed = Direction.Left;
                doTick = true;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                pressed = Direction.Right;
                doTick = true;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
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
	}

    void LoadLevel(int levelID)
	{
        //delete previous tiles

        Environment = new List<GameObject>();

        //LevelTextAsset = Resources.Load(/*pathToLevels+*/levelName) as TextAsset;
        //LevelTextAsset = Resources.Load(Path.Combine(Application.streamingAssetsPath, "SampleVideo_1280x720_5mb.mp4")) as TextAsset;
        LevelTextAsset = levels[levelID];
        string[] rows = LevelTextAsset.text.Split(rowDivider);
		for (int i = 0; i < rows.Length; i++)
		{
            string row = rows[(rows.Length - 1) - i];
            string[] tiles = row.Split(colDivider);
            
            //Debug.Log("=========row "+i);
            //Debug.Log(row);
			for (int j = 0; j < tiles.Length; j++)
			{
                string tile = tiles[j];
                
                string tileCodeString = tile; //use only first thing from that tho


                int tileCode;
                if (!int.TryParse(tileCodeString, out tileCode))
				{
                    Debug.LogError("couldnt convert " + tileCodeString + " into number");
				}

                Create((TileType)tileCode, i, j);
			}
		}


	}

    void Create(TileType tileCode, int i, int j)
	{
        GameObject tileObject;
        float x = tileSize * j;
        float y = tileSize * i;

        TileLayer layer;
        
        switch (tileCode)
        {
            case TileType.Nothing:
                return;

            case TileType.Wall:
                //Wall
                tileObject = Instantiate(prefabWall);
				layer = TileLayer.Environment;
                break;

            case TileType.Floor:
                //Floor
                tileObject = Instantiate(prefabFloor);
                layer = TileLayer.Environment;
                break;
            
            case TileType.Player:
                //Floor
                Create(TileType.Floor, i, j);

                //Player
                tileObject = Instantiate(prefabPlayer);
                layer = TileLayer.Object;
                break;

            default:
                return;
        }

        tileObject.transform.position = new Vector3(x, y, (int)layer);
        tileObject.transform.parent = gameObject.transform;
		if (layer == TileLayer.Environment)
		{
            Environment.Add(tileObject);
		}
    }
}
