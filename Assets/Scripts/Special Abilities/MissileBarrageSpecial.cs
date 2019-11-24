using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBarrageSpecial : SpecialAbility
{
    private int barrageDamage = 20;

    // Start is called before the first frame update
    void Start()
    {
        barrageDamage = _mech.strengthOfSpecialAbility;
    }
    protected override void Update()
    {
        base.Update();
    }
}
