using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aggresive_Scout : Enemy_Manager
{
    public override bool StartMove()
    {
        if (_Posture != Posture.Prone)
            ChangePos(Posture.Prone);

        Move_now = StartCoroutine(Move());
        return true;
    }
    public override void SelectClass()
    {
        _CLASS = CLASS.AggresiveScout;
    }
    public override bool Attack()
    {
        return base.Attack();
    }

    public override bool CoverCheck(Unit_Manager _Unit)
    {
        return base.CoverCheck(_Unit);
    }
}
