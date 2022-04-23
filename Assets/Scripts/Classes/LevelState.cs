using System.Collections.Generic;
using UnityEngine;

public class LevelState
{
    public int id; //for testing

    private IDictionary<Coordinates, Tile> Tiles;

    private int _smallestX;
    private int _biggestX;

    private int _smallestY;
    private int _biggestY;

    private float _diffX { get { return _biggestX - _smallestX; } }
    private float _diffY { get { return _biggestY - _smallestY; } }

    public float TileZoom 
    { 
        get
        {
            const float zoom = 12f;
            //gets either diffx or diffy based on which one is bigger
            float biggerDiff = (_diffX > _diffY) ? _diffX : _diffY;

            return zoom / biggerDiff;
        } 
    }

    public Vector3 PositionOffset {
        get
        {
            return new Vector3(_diffX / 2, _diffY / 2) ;
        }
    }



    public Player Player { get; private set; }
    public List<Wall> Walls { get; private set; }
    public List<Floor> Floors { get; private set; }
    public List<Box> Boxes { get; private set; }
    public List<Storage> Storages { get; private set; }
    public List<Button> Buttons { get; private set; }
    public List<Button> Doors { get; private set; }

    public bool Solved
    {
        get
        {
            bool solved = true;
            foreach (Storage storage in Storages)
            {
                if (this[storage.Position].Box == null)
                {
                    solved = false;
                    break;
                }
            }
            return solved;
        }
    }

    public void Add(Entity entity)
    {
        //add the entity to the main dictionary
        if (Tiles.ContainsKey(entity.Position))
        {
            Tiles[entity.Position].Add(entity);
        }
        else
        {
            Tile newTile = new Tile(this);
            newTile.Add(entity);
            Tiles.Add(entity.Position, newTile);
        }

        //update biggest/smallest coordinates
        if (entity.Position.x > _biggestX)
            _biggestX = entity.Position.x;
        if (entity.Position.x < _smallestX)
            _biggestX = entity.Position.x;
        if (entity.Position.y > _biggestY)
            _biggestY = entity.Position.y;
        if (entity.Position.y < _smallestY)
            _smallestY = entity.Position.y;


        /*
		 https://www.thomasclaudiushuber.com/2021/02/25/c-9-0-pattern-matching-in-switch-expressions/
		 
		switch (obj.GetType().Name)
		{
			case nameof(Developer):
				favoriteTask = "Write code";
				break;
			case nameof(Manager):
				favoriteTask = "Create meetings";
				break;
			default:
				favoriteTask = "Listen to music";
				break;
		}
		*/

        switch (entity.GetType().Name)
        {
            case nameof(Player):
                Player = entity as Player;
                break;

            case nameof(Wall):
                Walls.Add(entity as Wall);
                break;

            case nameof(Floor):
                Floors.Add(entity as Floor);
                break;


            case nameof(Box):
                Boxes.Add(entity as Box);
                break;

            case nameof(Storage):
                Storages.Add(entity as Storage);
                break;


            case nameof(Button):
                Buttons.Add(entity as Button);
                break;


            /* case nameof(Door):
                 Doors.Add(entity as Door);
                 break;*/





            default:
                throw new System.Exception("unknown type");
        }


        /*
		if (entity is Player)
			Player = entity as Player;

		if (entity is Wall)
			Walls.Add(entity as Wall);

		if (entity is Floor)
			Floors.Add(entity as Floor);

		if (entity is Box)
			Boxes.Add(entity as Box);

		if (entity is Button)
			Buttons.Add(entity as Button);

		if (entity is Floor)
			Walls.Add(entity as Wall);
		*/
        //magic happens here, TODO adding to each list
    }

    public Tile this[Coordinates coord]
    {
        get
        {
            if (Tiles.ContainsKey(coord))
            {
                return Tiles[coord];
            }
            else
            {
                Tile newTile = new Tile(this);
                Tiles.Add(coord, newTile);
                return newTile;
            }
        }
    }

    /*
	public bool Has<T>(Coordinates coordinates)
	{
		if (!Tiles.ContainsKey(coordinates)) return false;

		IList<Entity> tile = Tiles[coordinates];

		foreach (Entity entity in tile)
		{
			if (entity is T)
			{
				return true;
			}
		}

		return false;
	}

	public Get<T>(Coordinates coordinates)/* where T : class // might change*/
    /*{
		if (!Entities.ContainsKey(coordinates)) return default(T);

		IList<Entity> tile = Entities[coordinates];

		foreach (Entity entity in tile)
		{
			if (entity is T)
			{
				return (T)Convert.ChangeType(entity, typeof(T));
			}
		}
		return default(T);
	}*/


    public LevelState Copy(/*LevelState original*/)
    {
        ///create new instance of every entity
        LevelState newState = new LevelState(); //newstate is actually a static copy that will not change (will be stored in History)

        //testing
        newState.id = this.id + 1;


        newState.Add(Player.Copy(newState));



        foreach (Box box in Boxes)
        {
            newState.Add(box.Copy(newState));
        }

        foreach (Floor floor in Floors)
        {
            newState.Add(floor.Copy(newState));
        }

        foreach (Wall wall in Walls)
        {
            newState.Add(wall.Copy(newState));
        }

        foreach (Storage storage in Storages)
        {
            newState.Add(storage.Copy(newState));
        }


        ///more entities



        return newState;




        /*	this.Player = original.Player.Copy();

			this.Boxes = new List<Box>();
			foreach (Box box in original.Boxes)
			{
				this.Boxes.Add(box.Copy(original));
			}

			this.Walls = new List<Wall>(original.Walls);
			this.Buttons = new List<Button>(original.Buttons);
			this.Floors = new List<Floor>(original.Floors);

			this.Tiles = new Dictionary<Coordinates, Tile>(original.Tiles);
		*/
    }



    public LevelState()
    {
        this.id = 0;
        this.Player = null;
        this.Walls = new List<Wall>();
        this.Boxes = new List<Box>();
        this.Buttons = new List<Button>();
        this.Floors = new List<Floor>();
        this.Storages = new List<Storage>();


        this.Tiles = new Dictionary<Coordinates, Tile>();


    }




    /*public LevelState(List<GameObject> entities)
    {
        //Entities = new List<Entity>();

		foreach (var Entity in entities)
		{

		}
    }*/

    public override string ToString()
    {
        return "LevelState " + id;
    }
}