using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : Entity, IFloor
{
	public Ice(EntityConstructor ec, GameObject o)
	{
		this.MappedObject = o;
		this.Position = new Coordinates(ec.CoordinateX, ec.CoordinateY);
	}

	public Ice() { }

	public Ice Copy(LevelState original)
	{
		Ice ice = new Ice();
		ice.MappedObject = this.MappedObject;
		ice.Position = this.Position;
		ice.state = original;
		return ice;
	}

	#region UpdateSprite
	private bool generatedSprite = false;
    public override void UpdateSprite()
    {
		if (generatedSprite)
			return;

		MappedObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/ice/ice_128");

		generatedSprite = true;
	}
    #endregion


    #region IFloor
    public void SteppedOn(Entity entity, Direction direction)
	{
        if (!(entity is IMovingEntity))
        {
			throw new System.Exception("entity should have been imovingentity");
        }

		IMovingEntity movingEntity = entity as IMovingEntity;

		movingEntity.Move(entity.Position + direction);
	}
    #endregion

}
