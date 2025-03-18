using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Powerup
{
    public float duration;
    public bool isPerminent;
    public abstract void Apply(PowerupManager target);

    public abstract void Remove(PowerupManager target);
}
