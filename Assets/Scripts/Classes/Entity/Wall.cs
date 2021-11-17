using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Entity, IObstacle
{
	public Wall(EntityConstructor ec, GameObject o)
	{
		this.MappedObject = o;
		this.Position = new Coordinates(ec.CoordinateX, ec.CoordinateY);
	}

	#region IObstacle
	public bool Opened { get { return false; } }
	public void Open(bool open)
	{
		return; //wall cannot be opened
	}
	#endregion

}
