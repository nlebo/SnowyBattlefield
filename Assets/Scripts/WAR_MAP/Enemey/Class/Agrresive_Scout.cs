using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agrresive_Scout : Enemy_Manager
{
    public override bool Attack()
    {
        return base.Attack();
    }

    public override bool CoverCheck(Unit_Manager _Unit)
    {
        return base.CoverCheck(_Unit);
    }
}
