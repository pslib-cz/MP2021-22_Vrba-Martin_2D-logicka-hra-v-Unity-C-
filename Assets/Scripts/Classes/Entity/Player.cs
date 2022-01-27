using System.Collections.Generic;
using UnityEngine;
public class Player : Entity, IDirectionFacingEntity, IMovingEntity
{

	#region Constructors
	void PrepareThings()
    {
		FindRenderer();
		FindSound();
    }

	private Player() { }
	public Player(EntityConstructor ec, GameObject o, LevelState state)
	{
		this.direction = ec.Direction;
		this.Position = new Coordinates(ec.CoordinateX, ec.CoordinateY);
		this.MappedObject = o;
		this.state = state;
		PrepareThings();
	}
	public Player Copy(LevelState newstate)
	{
		Player output = new Player();
		output.direction = this.direction;
		output.Position = this.Position;
		output.MappedObject = this.MappedObject;
		output.state = newstate;

		output.PrepareThings();
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
		Coordinates previous = Position;
		this.Position = destination;
		state[previous].Update(this);
	}
	#endregion
	#region PerformTick
	public override void PerformTick(Direction input)
	{
		if (input != Direction.None)
		{
			///change direction
			SetDirection(input);

			Tile destination = state[LookingAt()];

			if (destination.Opened)
			{
				if (destination.Box != null)
				{
					Box box = destination.Box;
					if (box.Push(state,direction))
					{
						Move(Position + direction);
						sound.Play("push");
					}
				}
				else
				{
					Move(Position + direction);
					sound.Play("step");
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
	//private SpriteRenderer renderer;
	//private Dictionary<string, Sprite> sprites;
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

		//Debug.Log("sprites length " + sprites.Count);
		#endregion
	}

	/*private void SetSprite(string name)
	{
		renderer.sprite = sprites[name];
	}*/

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
    #region sound

    SoundManager sound;
	void FindSound()
    {
		sound = Object.FindObjectOfType<SoundManager>();
    }
    #endregion

	//for testing
    public override string ToString()
    {
        return "Player "+state.id;
    }
}