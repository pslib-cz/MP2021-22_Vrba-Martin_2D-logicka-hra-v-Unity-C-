using System.Collections.Generic;

public class Tile
{
	public Tile()
	{
		Entities = new List<Entity>();
	}

	public Tile(Entity entity)
	{
		Entities = new List<Entity>() { entity};
	}


	public IList<Entity> Entities { get; private set; }

	public void Add(Entity entity)
	{
		Entities.Add(entity);
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
}
