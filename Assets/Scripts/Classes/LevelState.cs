using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelState
{
    private IDictionary<Coordinates, IList<Entity>> Entities;

    public Player Player { get; private set; }
    public List<Wall> Walls { get; private set; }
    public List<Box> Boxes { get; private set; }
    public List<Button> Buttons { get; private set; }


	public void Add(Entity entity, Coordinates coordinates)
	{
		//add the entity to the main dictionary
		if (Entities.ContainsKey(coordinates))
		{
			Entities[coordinates].Add(entity);
		}
		else
		{
			Entities.Add(coordinates, new List<Entity>() { entity });
		}


		//magic happens here
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

    public T Get<T>(Coordinates coordinates) where T: class // might change
	{
        if (!Entities.ContainsKey(coordinates)) return default(T);

        IList<Entity> tile = Entities[coordinates];

		foreach (Entity entity in tile)
		{
			if (entity is T)
			{
				return entity as T;
			}
		}
		return default(T);
	}


    public LevelState(LevelState original)
	{
        
        LevelState output = new LevelState();
        output.Player = new Player(original.Player);
        output.Walls = new List<Wall>(original.Walls);
        output.Boxes = new List<Box>(original.Boxes);
        output.Buttons = new List<Button>(original.Buttons);

	}

    public LevelState()
	{



	}


    /*public LevelState(List<GameObject> entities)
    {
        //Entities = new List<Entity>();

		foreach (var Entity in entities)
		{

		}
    }*/
}