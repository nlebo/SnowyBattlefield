using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MANAGER : MonoBehaviour {

	[HideInInspector]
	public static UI_MANAGER m_UI_MANAGER;

	[Header("Text")]
    public Text D_Action_Point;
    public Text Posture;
	public Text _Class;
	public Text _Clip;
	public Text Sequence_Text;
	public Text Notice;
	public Text Set;
	public Text Tactical;
	public Text Name;
	public Text _FPS;
	public Text HitRateText;
	public TextMesh Damage;

	[Header("GameObject")]
    public GameObject Action_Point;
	public GameObject Sequence;
	public GameObject Inventory;
	public GameObject Chest;
	public GameObject Vest;
	public GameObject Bag;

	[Header("Button")]
    public Button Action_Button;
    public Button MOVE_BUTTON;
    public Button Posture_BUTTON;
    public Button Standing_Button;
    public Button Crouching_Button;
    public Button Proneing_Button;
	public Button ATTACK_Button;
	public Button Item_Button;
	public Button Back;
	public Button EndTurnButton;
	public Button ReloadButton;
	public Button DigButton;
	public Button ExpandButton;
	public Button BackPackButton;

	[Header("Image")]
    public Image Delete_AP_Bar;
    public Image HitRate;

    [Header("Array&List")]
	public Text[] Bars_Tex;
	List<Text> Texts;
	public List<Button> Weapon;
	public List<Button> Item;
	public List<Button> Weapon_Button;
	public Sprite[] Cursors;
	public Image[] Bars;
	

	[Header("ToastBar")]
	public GameObject Weapon_Toast;
	public GameObject Item_Toast;
	public GameObject Action_Toast;
	public GameObject Posture_Toast;


	[Header("ETC")]
	public string[] RFirstName,RLastName;
	public string[] EUFirstName,EULastName;
	public Image PostureImage;
	public Sprite[] PosSprite;
	public Unit_Manager _Unit;
	public Camera UICamera;




	float NowFPS;
	float FPS_Time;

    [HideInInspector]
	public bool WeaponToasting;

	[HideInInspector]
	public bool ItemToasting;

	[HideInInspector]
	public bool ActionToasting;

	[HideInInspector]
	public bool PostureToasting;

	void Awake()
	{
		m_UI_MANAGER = this;
		Texts = new List<Text>();

		NowFPS = 0;
		FPS_Time = 0;
		WeaponToasting = false;
		Application.targetFrameRate = 120;
		StartCoroutine(FPS());
	}



	public void Add_Sequence(string Text_)
	{
		Text TEXT = Instantiate(Sequence_Text, Sequence.transform).GetComponent<Text>();
		TEXT.text = Text_;

		Texts.Add(TEXT);

		if (Texts.Count <= 1) return;

	
	}

	private void Update()
	{
		FPS();
		if (_Unit != null)
			BarValue();

		if (Board_Manager.m_Board_Manager.TurnFlag == 0)
			HotKey();
	}

	public void HotKey()
	{

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			ATTACK_Button.onClick.Invoke();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			ReloadButton.onClick.Invoke();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			MOVE_BUTTON.onClick.Invoke();
		}
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			
		}
		if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			Posture_BUTTON.onClick.Invoke();
		}
		if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			Item_Button.onClick.Invoke();
		}
		if (Input.GetKeyDown(KeyCode.Alpha7))
		{

		}
		if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			Action_Button.onClick.Invoke();
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			EndTurnButton.onClick.Invoke();
		}
	}


	public void BarValue()
	{
		Bars[0].fillAmount = _Unit.Health / 100f;
		Bars_Tex[0].text = (_Unit.Health / 100f * 100).ToString();

		Bars[1].fillAmount = _Unit.pressure / 100f;
		Bars_Tex[1].text = _Unit.pressure.ToString();

		Bars[2].fillAmount = _Unit.Now_Action_Point / (float)_Unit.Pull_Action_Point;
		Bars_Tex[2].text = _Unit.Now_Action_Point.ToString();
		
	}

	public void PosImageChange(Unit_Manager.Posture Pos)
	{
		switch (Pos)
		{
			case Unit_Manager.Posture.Standing:
				PostureImage.sprite = PosSprite[0];
				break;
			case Unit_Manager.Posture.Crouching:
				PostureImage.sprite = PosSprite[1];
				break;
			case Unit_Manager.Posture.Prone:
				PostureImage.sprite = PosSprite[2];
				break;
		}
		PostureImage.SetNativeSize();
	}

	public IEnumerator BarUpToast(GameObject Toast,string What)
	{
		if(Toast.activeInHierarchy) 
		{
			yield return null;
		}

        else
        {
            RectTransform _Rect = Toast.GetComponent<RectTransform>();
            Vector2 _Base = _Rect.anchoredPosition;
            _Rect.anchoredPosition = new Vector2(_Rect.anchoredPosition.x,0);

            yield return null;
            Toast.SetActive(true);
            yield return null;

            float TTime = 0;
            while (TTime < 0.5f)
            {
                TTime += Time.deltaTime;
                _Rect.anchoredPosition = new Vector2(_Rect.anchoredPosition.x, Mathf.Lerp(0, _Base.y, TTime * 2));

                yield return null;
            }

            _Rect.anchoredPosition = _Base;
			if(What == "Weapon") WeaponToasting = false;
			else if(What == "Item") ItemToasting = false;
			else if(What == "Action")ActionToasting = false;
			else if(What == "Posture")PostureToasting = false;
            yield return null;
        }
	}

	public IEnumerator BarDownToast(GameObject Toast,string What)
	{
		if(!Toast.activeInHierarchy)
		{
			yield return null;
		}
		else if(What == "Weapon" && WeaponToasting)Toast.SetActive(false);
		else if(What == "Item" && ItemToasting) Toast.SetActive(false);
		else if(What == "Action" && ActionToasting)Toast.SetActive(false);
		else if(What == "Posture"&& PostureToasting)Toast.SetActive(false);
        else
        {
            RectTransform _Rect = Toast.GetComponent<RectTransform>();
            Vector2 _Base = _Rect.anchoredPosition;

            yield return null;
            

            float TTime = 0;
            while (TTime < 0.5f)
            {
                TTime += Time.deltaTime;
                _Rect.anchoredPosition = new Vector2(_Rect.anchoredPosition.x, Mathf.Lerp(_Base.y, 0, TTime * 2));

                yield return null;
            }

			_Rect.anchoredPosition = new Vector2(_Rect.anchoredPosition.x,0);
			yield return null;

			Toast.SetActive(false);
			_Rect.anchoredPosition = _Base;
			if(What == "Weapon") WeaponToasting = false;
			else if(What == "Item") ItemToasting = false;
			else if(What == "Action")ActionToasting = false;
			else if(What == "Posture")PostureToasting = false;
            yield return null;
        }
	}
	IEnumerator FPS()
	{
		while (true)
		{
			FPS_Time += (Time.deltaTime - FPS_Time) * 0.1f;
			NowFPS = (int)(1.0 / FPS_Time);
			_FPS.text = NowFPS.ToString();
			yield return new WaitForSeconds(0.5f);
		}
	}

}
