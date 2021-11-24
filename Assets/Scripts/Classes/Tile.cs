using System.Collections.Generic;

public class Tile
{
	public Tile()
	{
		Entities = new List<Entity>();
	}

	public Tile(Entity entity)
	{
		Entities = new List<Entity>() { entity };
	}


	public IList<Entity> Entities { get; private set; }

	public void Add(Entity entity)
	{
		Entities.Add(entity);
		if(entity is Box)
		{
			this.Box = entity as Box;
		}
	}

	public bool Opened
	{
		get
		{
			bool hasClosedObstacle = false;
			bool hasFloor = false;
			foreach (Entity entity in Entities)
			{
				if (entity is Floor)
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

	public bool HasBox
	{
		get
		{
			foreach (Entity entity in Entities)
			{
				if (entity is Box)
				{
					return true;
				}
			}
			return false;
		}
	}
	public Box Box { get; private set; }

}
