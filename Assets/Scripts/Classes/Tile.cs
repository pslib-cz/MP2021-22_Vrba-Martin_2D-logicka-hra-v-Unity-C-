using System.Collections.Generic;

public class Tile
{
	private LevelState state;

	public Tile(LevelState state)
	{
		this.state = state;
		Entities = new List<Entity>();
	}


	public IList<Entity> Entities { get; private set; }

	public void Add(Entity entity)
	{
		if (entity is IFloor && Floor != null)
			throw new System.Exception("there cannot be more than 1 floor: "+entity.Position.x+"-"+entity.Position.y);

		Entities.Add(entity);
	}

	public void Update(Entity entity)
	{
		state[entity.Position].Add(entity);
		Entities.Remove(entity);
	}

	public bool Opened
	{
		get
		{
			bool hasClosedObstacle = false;
			bool hasFloor = false;
			foreach (Entity entity in Entities)
			{
				if (entity is IFloor)
					hasFloor = true;
				if (entity is IObstacle)
				{

					if (!(entity as IObstacle).Opened)
					{
						hasClosedObstacle = true;
					}
				}
			}

			//returns true if contains floor but doesnt contain closed obstacle
			return hasFloor && !hasClosedObstacle;
		}
	}


	public Box Box 
	{
		get 
		{
			foreach (Entity entity in Entities)
			{
				if (entity is Box)
				{
					return entity as Box;
				}
			}
			return null;
		} 
	}
	public IFloor Floor
    {
		get
		{
			foreach (Entity entity in Entities)
			{
				if (entity is IFloor)
				{
					return entity as IFloor;
				}
			}
			return null;
		}
	}

}
