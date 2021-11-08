using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : Entity, IDirectionFacingEntity, IMovingEntity
{

	/*public Player()
	{
		this.direction = Direction.None;
	}

    public Player(Direction direction)
    {
        this.direction = direction;
	}

	public Player(Player original)
	{
		this.direction = original.direction;
		this.Position = original.Position;
	}*/

	public Player()
	{

	}

	public Player(EntityConstructor ec, GameObject o)
	{
		this.direction = ec.Direction;
		this.Position = new Coordinates(ec.CoordinateX, ec.CoordinateY);
		this.MappedObject = o;
	}

	public Player Copy()
	{
		Player output = new Player();
		output.direction = this.direction;
		output.Position = this.Position;

		return output;
	}

	#region IDirectionFacingEntity
	private Direction direction;
	public Direction GetDirection()
	{
        return direction;
	}
    public void SetDirection(Direction direction)
	{
        this.direction = direction;
	}
	public Coordinates LookingAt()
	{
		return Position + direction;
	}
	#endregion

	#region PerformTick
	public override void PerformTick(LevelState state, Direction input)
	{
		if (input != Direction.None)
		{
			///change direction
			SetDirection(input);

			///move if possible
			bool canMove = true;
			if (( state[LookingAt()] is IObstacle)) 
			{

				IObstacle obstacle = state.Get<IObstacle>(LookingAt()); //might change
				if (!obstacle.Opened)
				{
					canMove = false;
				}
			}
		}


        //do actual stuff
	}
	#endregion
}