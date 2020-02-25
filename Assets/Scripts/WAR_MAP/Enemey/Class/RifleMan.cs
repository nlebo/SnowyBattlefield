using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RifleMan : Enemy_Manager
{
    public override bool Attack()
    {
        if (Items[1].count > 0)
        {
            Items[1].Rand(this);
        }
        return base.Attack();
    }

    public override bool CoverCheck(Unit_Manager _Unit)
    {
        return base.CoverCheck(_Unit);
    }
    
}
