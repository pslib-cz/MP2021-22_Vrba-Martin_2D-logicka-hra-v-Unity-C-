using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Entity, IMovingEntity, IPushable
{
	private Box(){}

	public Box(EntityConstructor ec, GameObject o, LevelState state)
	{
		this.state = state;
		this.Position = new Coordinates(ec.CoordinateX, ec.CoordinateY);
		this.MappedObject = o;
	}

	public bool Push(Direction direction)
	{
		Tile destination = state[Position + direction];

		if ( (destination.Box != null) || (!destination.Opened) /*add more if needed*/)
		{
			return false;
		}

		Move(Position + direction);
		return true;
	}

	public void Move(Coordinates destination)
	{
		Coordinates previous = Position;
		this.Position = destination;
		state[previous].Update(this);
		state[Position].Update(this);
	}
	
	/*public Box Copy()
    {
		return Copy(state);
    }*/

	public Box Copy(LevelState newState)
    {
		Box box = new Box();
		box.MappedObject = MappedObject;
		box.Position = Position;

		box.state = newState;

		return box;


	/*	Box newbox = new Box();
		newbox.MappedObject = this.MappedObject;
	*/
    }
	#region UpdateSprite
	private bool generatedSprite = false;
	public override void UpdateSprite()
	{
		if (generatedSprite)
			return;

		MappedObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/box/box_128");

		generatedSprite = true;
	}
    #endregion
}
