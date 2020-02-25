using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssultSoldier : Enemy_Manager
{

    public override void SelectClass()
    {
        _CLASS = CLASS.AssultSoldier;
    }
    public override bool Attack()
    {
        return base.Attack();
    }

    public override bool CoverCheck(Unit_Manager _Unit)
    {
        if (_Posture == Posture.Prone) {
            for(int i=0;i<View_Manager.Enemys.Count;i++)
            {
                if(View_Manager.Enemys[i] == this.transform && Mathf.Abs(_Unit.x - x) + Mathf.Abs(_Unit.y -y) > 5)
                {
                    return base.CoverCheck(_Unit,1);
                }
            }
            if (Mathf.Abs(_Unit.x - x) + Mathf.Abs(_Unit.y - y) > 5)
                return false;
            else
                return true;
        }

        
        ChangePos(Posture.Prone);
        return false;
    }
}
