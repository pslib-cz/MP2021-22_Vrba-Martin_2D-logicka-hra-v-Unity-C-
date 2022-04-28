using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFloor 
{
    void SteppedOn(Entity entity, Direction direction);

}
