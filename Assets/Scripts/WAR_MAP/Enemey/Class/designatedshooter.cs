﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class designatedshooter : Enemy_Manager
{
    public override bool Attack()
    {
        return base.Attack();
    }

    public override bool CoverCheck(Unit_Manager _Unit)
    {
        if (_Posture == Posture.Prone) return true;

        return ChangePos(Posture.Prone);
    }
}
