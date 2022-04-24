using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : Entity, IFloor
{
	public Floor(EntityConstructor ec, GameObject o)
	{
		this.Position = new Coordinates(ec.CoordinateX, ec.CoordinateY);
		this.MappedObject = o;
	}

	private Floor() { }

	public Floor Copy(LevelState original)
    {
		Floor floor = new Floor();
		floor.MappedObject = this.MappedObject;
		floor.Position = this.Position;
		floor.state = original;
		return floor;
    }

	#region UpdateSprite
	private bool generatedSprite = false;
	public override void UpdateSprite()
	{
		if (generatedSprite)
			return;

		MappedObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/floor/floor_128");

		generatedSprite = true;
	}
	#endregion

	public void SteppedOn(Entity entity, Direction direction) { } //basic floor doesn't do anything special
}
