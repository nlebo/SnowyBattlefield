using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssultSoldier : Enemy_Manager
{
    public override bool Attack()
    {
        return base.Attack();
    }

    public override bool CoverCheck(Unit_Manager _Unit)
    {
        if (_Posture == Posture.Prone) {
            for(int i=0;i<View_Manager.Enemys.Count;i++)
            {
                if(View_Manager.Enemys[i] == this.transform)
                {
                    return base.CoverCheck(_Unit,1);
                }
            }

            return true;
        }

        ChangePos(Posture.Prone);
        return false;
    }
}
