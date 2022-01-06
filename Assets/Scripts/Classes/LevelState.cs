using System;
using System.Collections.Generic;


public class LevelState
{

	private IDictionary<Coordinates, Tile> Tiles;

	public Player Player { get; private set; }
	public List<Wall> Walls { get; private set; }
	public List<Floor> Floors { get; private set; }
	public List<Box> Boxes { get; private set; }
	public List<Button> Buttons { get; private set; }



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


			case nameof(Button):
				Buttons.Add(entity as Button);
				break;






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
			Tile newTile = new Tile(this);
			Tiles.Add(coord, newTile);
			return newTile; 
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
		///create new instance of every changing entity
		LevelState newState = new LevelState(); //newstate is actually a static copy that will not change (will be stored in History)

		newState.Add(Player.Copy(this));


		 
		foreach (Box box in Boxes)
		{
			newState.Boxes.Add(box.Copy(this));
		}

		///more changing entities


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
		this.Player = null;
		this.Walls = new List<Wall>();
		this.Boxes = new List<Box>();
		this.Buttons = new List<Button>();
		this.Floors = new List<Floor>();

		this.Tiles = new Dictionary<Coordinates, Tile>();


	}




	/*public LevelState(List<GameObject> entities)
    {
        //Entities = new List<Entity>();

		foreach (var Entity in entities)
		{

		}
    }*/
}