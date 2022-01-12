using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Entity
{
    private Storage() { }
    public Storage(EntityConstructor ec, GameObject o, LevelState state)
    {
        this.Position = new Coordinates(ec.CoordinateX, ec.CoordinateY);
        this.MappedObject = o;
        this.state = state;
        FindRenderer();
    }

    public Storage Copy(LevelState state)
    {
        Storage newstorage = new Storage();
        newstorage.Position = this.Position;
        newstorage.MappedObject = this.MappedObject;
        newstorage.state = state;

        newstorage.FindRenderer();
        return newstorage;
    }

	private void FindRenderer()
	{
		renderer = MappedObject.GetComponent<SpriteRenderer>();

		#region sprites
		sprites = new Dictionary<string, Sprite>();
		//https://answers.unity.com/questions/1331550/how-to-load-sprite-dynamically-from-assets-in-unit.html
		sprites.Add("orange", Resources.Load<Sprite>("Sprites/storage/storage_orange_128"));
		sprites.Add("green", Resources.Load<Sprite>("Sprites/storage/storage_green_128"));

		//Debug.Log("sprites length " + sprites.Count);
		#endregion
	}



	protected override void SetSprite(string name)
	{
		renderer.sprite = sprites[name];
	}

	public override void UpdateSprite()
	{
         if (state[Position].Box == null)
        {
            SetSprite("orange");
        }
        else
        {
            SetSprite("green");

        }
	}
}
