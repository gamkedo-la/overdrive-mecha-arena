using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : Shooting
{
    // Must find out how the mecha selection will work in code
    // Could create a parent class Mecha and have subclasses of SpeedMecha, DamageMecha, and AllPurposeMecha

    protected override void Start()
    {
        base.Start();
    }
}
