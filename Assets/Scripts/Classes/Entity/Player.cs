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
		FindRenderer();
	}
	public Player Copy()
	{
		Player output = new Player();
		output.direction = this.direction;
		output.Position = this.Position;
		output.MappedObject = this.MappedObject;
		output.FindRenderer();

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
		if (input != Direction.None)
		{
			///change direction
			SetDirection(input);

			/*///move if possible
			bool canMove = false;
			if (state[LookingAt()].Opened)
			{
				canMove = true;
				// ...
			}
			// will add boxes and stuff later
			if (canMove)
			{
				Move(LookingAt());
			}*/
			Tile destination = state[LookingAt()];

			if (destination.Opened)
			{
				if (destination.HasBox)
				{
					Box box = destination.Box;
					if (box.Push(state,direction))
					{
						Move(Position + direction);
					}
				}
				else
				{
					Move(Position + direction);
				}
			}



		}
		else
		{
			Debug.Log("input is none");
		}


		//do actual stuff
	}
	#endregion
	#region UpdateSprite
	private SpriteRenderer renderer;
	private Dictionary<string, Sprite> sprites;
	private void FindRenderer()
	{
		renderer = MappedObject.GetComponent<SpriteRenderer>();

		#region sprites
		sprites = new Dictionary<string, Sprite>();
		//https://answers.unity.com/questions/1331550/how-to-load-sprite-dynamically-from-assets-in-unit.html
		sprites.Add("down", Resources.Load<Sprite>("Sprites/player/player_down_128"));
		sprites.Add("up", Resources.Load<Sprite>("Sprites/player/player_up_128"));
		sprites.Add("right", Resources.Load<Sprite>("Sprites/player/player_right_128"));
		sprites.Add("left", Resources.Load<Sprite>("Sprites/player/player_left_128") );

		Debug.Log("sprites length " + sprites.Count);
		#endregion
	}

	private void SetSprite(string name)
	{
		renderer.sprite = sprites[name];
	}

	public override void UpdateSprite()
	{
		switch (direction)
		{
			case Direction.None:
				SetSprite("down");
				break;
			case Direction.Up:
				SetSprite("up");
				break;
			case Direction.Down:
				SetSprite("down");
				break;
			case Direction.Left:
				SetSprite("left");
				break;
			case Direction.Right:
				SetSprite("right");
				break;
			default:
				SetSprite("down");
				break;
		}
	}
	#endregion
}