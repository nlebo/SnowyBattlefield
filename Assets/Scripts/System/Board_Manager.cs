using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Board_Manager : MonoBehaviour
{
    public static Board_Manager m_Board_Manager;

    public UnityAction Recive_Tactical;
    public List<Unit_Manager> Unit_List;
    public List<int> Sequence;
    public Image[] images;
    List<int> Pick;
    public int Loading = 0;
    public bool MapHack = false;
    public int TurnFlag = 0; // 0 = Player , 1 = Enemy;
    int set = 0;

    Color[] col;


    private void Awake()
    {
        m_Board_Manager = this;
        Unit_List = new List<Unit_Manager>();
        Sequence = new List<int>();
        col = new Color[] { Color.blue, Color.red };
        StartCoroutine(Loading_());
    }

    public void MakeTurn()
    {
        if (set == 0)
        {
            Sequence.Reverse(0, Sequence.Count);
            StartTurn();
            return;
        }
        Pick = new List<int>();

        for (int i = 0; i < Unit_List.Count; i++)
        {
            int pick = Random.Range(0, Sequence.Count);

            if (Unit_List[Sequence[pick] - 1]._Class == Unit_Manager.Class.Second_Gunner)
            {
                Sequence.Remove(Sequence[pick]);
                continue;
            }
            Pick.Add(Sequence[pick]);
            Sequence.Remove(Sequence[pick]);
            
        }
        Sequence.Clear();
        Sequence = new List<int>();

        for (int i = 0; i < Unit_List.Count; i++)
        {
            if (Unit_List[i] == null)
            {
                Unit_List.RemoveAt(i);
                i--;
                continue;
            }
            Sequence.Add(i + 1);
        }

        for (int i = 0; i < Unit_List.Count; i++)
        {
            int pick = Random.Range(0, Sequence.Count);

            if (Unit_List[Sequence[pick] - 1]._Class == Unit_Manager.Class.Second_Gunner)
            {
                Sequence.Remove(Sequence[pick]);
                continue;
            }
            Pick.Add(Sequence[pick]);
            Sequence.Remove(Sequence[pick]);

        }

        Sequence.Clear();
        Sequence = new List<int>();
        Sequence = Pick;
        
        StartTurn();
        
    }
    public void StartTurn()
    {
        ImageChange();
        if (Unit_List[Sequence[0] - 1] == null)
        {
            Unit_List.RemoveAt(Sequence[0] - 1);
            EndTurn();
            return;
        }

        if (Unit_List[Sequence[0] - 1]._Class == Unit_Manager.Class.Second_Gunner)
        {
            EndTurn();
            return;
        }

        Unit_List[Sequence[0] - 1].TurnOn();
        if (Unit_List[Sequence[0] - 1].Set != set)
        {
            set++;
            UI_MANAGER.m_UI_MANAGER.Set.text = set.ToString();
        }
    }

    public void EndTurn()
    {
        Sequence.RemoveAt(0);

        if (Sequence.Count < 1)
        {
            Debug.Log("Clear");

            for (int i = 0; i < Unit_List.Count; i++)
            {
                if (Unit_List[i] == null)
                {
                    Unit_List.RemoveAt(i);
                    i--;
                    continue;
                }
                Sequence.Add(i+1);
            }
            MakeTurn();
        }
        else
            StartTurn();
    }

    public void AddUnit(Unit_Manager _Unit)
    {
        Unit_List.Add(_Unit);
        Sequence.Add(Unit_List.Count);
    }

    public IEnumerator Loading_()
    {
        float QuitTime = 3;
        float nowTime = 0;
        int Loaded = Loading;

        while (true)
        {
            nowTime += Time.deltaTime;
            if (Loaded != Loading)
            {
                Loaded = Loading;
                nowTime = 0;
            }

            if (nowTime > QuitTime)
                break;

            yield return null;
        }
        Recive_Tactical();
        MakeTurn();
        yield return null;
    }

    public void Death(Unit_Manager _Unit)
    {
        Sequence.Remove((Unit_List.IndexOf(_Unit) + 1));
        Unit_List[Unit_List.IndexOf(_Unit)] = null;
    }

    public void ImageChange()
    {
        for (int i = 0; i < images.Length; i++)
        {
            images[i].gameObject.SetActive(true);
            if (i >= Sequence.Count)
            {
                images[i].gameObject.SetActive(false);
                continue;
            }
            if (Unit_List[Sequence[i] - 1].CompareTag("Player"))
            {
                images[i].color = col[0];
            }
            else
                images[i].color = col[1];
        }
    }
}
