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

	public Wall () { }

	public Wall Copy(LevelState original)
	{
		Wall wall = new Wall();
		wall.MappedObject = this.MappedObject;
		wall.Position = this.Position;
		wall.state = original;
		return wall;
	}

	#region UpdateSprite
	private bool generatedSprite = false;
    public override void UpdateSprite()
    {
		if (generatedSprite)
			return;

		MappedObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/wall/wall_128");

		generatedSprite = true;
	}
	#endregion

    #region IObstacle
    public bool Opened { get { return false; } }
	public void Open(bool open)
	{
		return; //wall cannot be opened
	}
	#endregion

}
