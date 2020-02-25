using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defensive_Scout : Enemy_Manager
{
    public override bool Attack()
    {
        return base.Attack();
    }

    public override bool CoverCheck(Unit_Manager _Unit)
    {
        base.CoverCheck(_Unit);
    }
}
