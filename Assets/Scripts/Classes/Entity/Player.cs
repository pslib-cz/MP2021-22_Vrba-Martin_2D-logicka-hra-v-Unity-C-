using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Player : Entity, IDirectionFacingEntity, IMovingEntity
{
	#region Constructors
	private Player() { }
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
	#endregion
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
	#region IMovingEntity
	public void Move(Coordinates destination)
	{
		//Debug.Log("playe moved");
		//Debug.Log(destination.x+" - "+destination.y);
		this.Position = destination;
	}
	#endregion
	#region PerformTick
	public override void PerformTick(LevelState state, Direction input)
	{
		//Debug.Log("player performing tick");
		if (input != Direction.None)
		{
			//Debug.Log("input isnt none");



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

			if (canMove)
			{
				Move(LookingAt());
			}
		}
		else
		{
			Debug.Log("input is none");
		}


        //do actual stuff
	}
	#endregion
}