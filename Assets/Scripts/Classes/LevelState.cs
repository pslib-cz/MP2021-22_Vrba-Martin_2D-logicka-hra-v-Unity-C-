using System;
using System.Collections.Generic;


public class LevelState
{

	private IDictionary<Coordinates, IList<Entity>> Entities;

	public Player Player { get; private set; }
	public List<Wall> Walls { get; private set; }
	public List<Floor> Floors { get; private set; }
	public List<Box> Boxes { get; private set; }
	public List<Button> Buttons { get; private set; }



	public void Add(Entity entity)
	{
		//add the entity to the main dictionary
		if (Entities.ContainsKey(entity.Position))
		{
			Entities[entity.Position].Add(entity);
		}
		else
		{
			Entities.Add(entity.Position, new List<Entity>() { entity });
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
				break;
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

	public IList<Entity> this[Coordinates coord]
	{
		get
		{
			if (Entities.ContainsKey(coord))
			{
				return Entities[coord];
			}
			return null;
		}
	}


	public bool Has<T>(Coordinates coordinates)
	{
		if (!Entities.ContainsKey(coordinates)) return false;

		IList<Entity> tile = Entities[coordinates];

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
	{
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
	}


	public LevelState(LevelState original)
	{

		this.Player = original.Player.Copy();

		this.Walls = new List<Wall>(original.Walls);
		this.Boxes = new List<Box>(original.Boxes);
		this.Buttons = new List<Button>(original.Buttons);
		this.Floors = new List<Floor>(original.Floors);
		
		this.Entities = new Dictionary<Coordinates, IList<Entity>>(original.Entities);
	}



	public LevelState()
	{
		this.Player = null;
		this.Walls = new List<Wall>();
		this.Boxes = new List<Box>();
		this.Buttons = new List<Button>();
		this.Floors = new List<Floor>();

		this.Entities = new Dictionary<Coordinates, IList<Entity>>();


	}




	/*public LevelState(List<GameObject> entities)
    {
        //Entities = new List<Entity>();

		foreach (var Entity in entities)
		{

		}
    }*/
}