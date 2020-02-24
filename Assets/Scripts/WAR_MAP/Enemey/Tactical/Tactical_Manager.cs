using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tactical_Manager : MonoBehaviour
{
    enum Style
    {
        Attack,
        Defence
    }
    enum Attack_Style
    {
        NONE,
        Move_Forward,
        Charge,
        Siege,
        Max
    }
    enum Defense_Style
    {
        NONE,
        Build_Recon,
        Shot_Run,
        Bypass,
        Max
    }


    [SerializeField]
    Style _Style;
    [SerializeField]
    Attack_Style _Attack_Style;
    [SerializeField]
    Defense_Style _Defense_Style;
    public static Tactical_Manager m_Tactical;

    Text _Text;
    Tactical_Head Tactical;
    public void Awake()
    {
        m_Tactical = this;
        MakeTactical();
    }
    public void MakeTactical()
    {
        int r;
        switch (Random.Range(1, 3))
        {
            case 1:
                _Style = Style.Attack;
                r = Random.Range(1, (int)Attack_Style.Max);
                _Attack_Style += r;
                break;
            case 2:
                _Style = Style.Defence;
                r = Random.Range(1, (int)Defense_Style.Max);
                _Defense_Style += r;
                break;
        }
    }
    public void ReciveTactical(Enemy_Manager _Enemy)
    {
        if (_Text == null)
            _Text = UI_MANAGER.m_UI_MANAGER.Tactical;
        switch (_Attack_Style)
        {
            case Attack_Style.NONE:
                break;
            case Attack_Style.Move_Forward:
                Tactical = _Enemy.gameObject.AddComponent<Move_Forward>();
                Tactical._this = _Enemy;
                _Enemy._Tactical = Tactical;
                _Text.text = "전진";
                break;
            case Attack_Style.Charge:
                Tactical = _Enemy.gameObject.AddComponent<Charge>();
                Tactical._this = _Enemy;
                _Enemy._Tactical = Tactical;
                _Text.text = "돌격";
                break;
            case Attack_Style.Siege:
                Tactical = _Enemy.gameObject.AddComponent<Siege>();
                Tactical._this = _Enemy;
                _Enemy._Tactical = Tactical;
                _Text.text = "포위";
                break;
        }

        switch (_Defense_Style)
        {
            case Defense_Style.NONE:
                break;
            case Defense_Style.Build_Recon:
                Tactical = _Enemy.gameObject.AddComponent<By_Pass>();
                Tactical._this = _Enemy;
                _Enemy._Tactical = Tactical;
                _Text.text = "백핸드";
                break;
            case Defense_Style.Shot_Run:
                Tactical = _Enemy.gameObject.AddComponent<Shot_Run>();
                Tactical._this = _Enemy;
                _Enemy._Tactical = Tactical;
                _Text.text = "능동방어";
                break;
            case Defense_Style.Bypass:
                Tactical = _Enemy.gameObject.AddComponent<By_Pass>();
                Tactical._this = _Enemy;
                _Enemy._Tactical = Tactical;
                _Text.text = "백핸드";
                break;
        }
    }

  
}
