using UnityEngine;

public class Hole : Entity, IObstacle, IFloor
{
    public Stages Stage { get; private set; }
    public enum Stages
    {
        Crack, //before someone steps on it
        Crumble, //during being pressured
        Hole, //closed obstacle
        Filled //open obstacle
    }
    public Hole(EntityConstructor ec, GameObject o, Stages stage)
    {
        this.MappedObject = o;
        this.Position = new Coordinates(ec.CoordinateX, ec.CoordinateY);
        this.Stage = stage;
    }

    public Hole() { }

    public Hole Copy(LevelState original)
    {
        Hole hole = new Hole();
        hole.MappedObject = this.MappedObject;
        hole.Position = this.Position;
        hole.Stage = this.Stage;
        hole.state = original;
        return hole;
    }

    #region UpdateSprite
    private bool generatedSprite = false;
    public override void UpdateSprite()
    {
        if (generatedSprite)
            return;

        string spritename = "";
        switch (Stage)
        {
            case Stages.Crack:
                spritename = "crack_128";
                break;
            case Stages.Crumble:
                spritename = "crumbling_128";
                break;
            case Stages.Hole:
                spritename = "empty_128";
                break;
            case Stages.Filled:
                spritename = "filled_128";
                break;
        }

        MappedObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/hole/{spritename}");

        generatedSprite = true;
    }
    #endregion


    #region IObstacle
    public bool Opened { get { return Stage != Stages.Hole; } }
    public void Open(bool open)
    {
        return; //hole cannot be opened
    }
    #endregion

    #region IFloor
    public void SteppedOn(Entity entity, Direction direction)
    {
        if (Stage == Stages.Crack && entity is ICrumbler)
        {
            Stage = Stages.Crumble;
        }


    }
    #endregion

    public bool TryFill()
    {
        if (Stage == Stages.Hole)
        {
            Stage = Stages.Filled;
            return true;
        }
        else return false;
    }

    public override void PerformTick(Direction input)
    {
        if (Stage == Stages.Crumble)
        {
            bool hasCrumbler = false;
            foreach (Entity entity in state[Position].Entities)
            {
                if (entity is ICrumbler)
                {
                    hasCrumbler = true;
                    break;
                }
            }

            if (!hasCrumbler)
            {
                Stage = Stages.Hole;
            }
        }
    }
}
