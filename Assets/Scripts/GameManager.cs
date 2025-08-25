using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    
    public enum Language
    {
        None = 0,
        Uzb = 1,
        Arab = 2,
        Eng = 3,
        Rus = 4
    }
    
    public static GameManager instance;

    public ClientUDP ClientUdp;
    public AnimText MainText;
    public List<string> DefaultTextes = new List<string>();
    public GameObject DefaultScreen;
    
    public TMP_Text PushScreenText;
    public List<string> PushLangs = new List<string>();
    
    public Button b_Uzb;
    [HideInInspector] public Button b_Arab;
    public Button b_Rus;
    [HideInInspector] public Button b_Eng;
    
    public Image MainImage;
    public Button BackButton;
    public Sprite MainSprite_Rus;
    public Sprite MainSprite_Uzb;
    [HideInInspector] public Sprite MainSprite_Eng;
    [HideInInspector] public Sprite MainSprite_Arab;
    public List<RegionClass> Regions = new List<RegionClass>();
    
    
    public Dictionary<int,List<string>> languageList = new Dictionary<int,List<string>>();
    public List<string> RusLang = new List<string>();
    public List<string> EngLang = new List<string>();
    public List<string> ArabLang = new List<string>();
    public List<string> UzbLang = new List<string>();
    
    public List<TMP_Text> TextObjects = new List<TMP_Text>();
    public List<AnimText> AnimTextObjects = new List<AnimText>();

    public Transform ParentText;
    public GameObject TextPrefab;
    public int CountTextPerSecond;
    public float SpeedSpawnText;
    public float SpeedAnimText;
    
    [HideInInspector] public int CurrentLang = 0;
    private Coroutine _coroutine;
    private float _timeout;
    private Vector3 _scale;
    private bool _isDown;
    private int _currentLangAnim;

    private float _timer;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Start()
    {
        ClientUdp = GetComponent<ClientUDP>();
        b_Uzb.onClick.AddListener(OnLangUzb);
        //b_Arab.onClick.AddListener(OnLangArab);
        b_Rus.onClick.AddListener(OnLangRus);
        //b_Eng.onClick.AddListener(OnLangEng);
        foreach (var region in Regions)
        {
            region.Init();
        }

        BackButton.onClick.AddListener(OnBack);
        languageList.Add(1, new List<string>(UzbLang));
        languageList.Add(3, new List<string>(EngLang));
        languageList.Add(4, new List<string>(RusLang));
        languageList.Add(2, new List<string>(ArabLang));
        CurrentLang = 4;
        _currentLangAnim = 4;
        ChangeLanguageAnim();
        _coroutine = StartCoroutine(StartAnimation());
        //_coroutine = StartCoroutine(StartAnimation());
        ClientUdp.Init();
        //MySendMessage("01");
    }

    private void ChangeLanguageAnim()
    {
        int k = 0;
        for (int i = 0; i < AnimTextObjects.Count; i++)
        {
            AnimTextObjects[i].SetText(languageList[_currentLangAnim][k]);
            k++;
            if (k == languageList[_currentLangAnim].Count)
                k = 0;
        }
        MainText.SetText(DefaultTextes[_currentLangAnim-1]);
        PushScreenText.text = PushLangs[_currentLangAnim-1];
    }

    private void ChangeLanguage()
    {
        switch (CurrentLang)
        {
            case 1:
            {
                MainImage.sprite = MainSprite_Uzb;
                break;
            }
            case 2:
            {
                MainImage.sprite = MainSprite_Arab;
                break;
            }
            case 3:
            {
                MainImage.sprite = MainSprite_Eng;
                break;
            }
            case 4:
            {
                MainImage.sprite = MainSprite_Rus;
                break;
            }
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            _timer = Time.time;
        }

        if (!DefaultScreen.activeSelf && Time.time - _timer > 100f)
        {
            OnBack();
        }
    }
    

    IEnumerator StartAnimation()
    {
        float timer = Time.time;
        while (true)
        {
            
            _scale = Vector3.one;
            
            yield return new WaitForSeconds(0.3f);

            AnimTextObjects[Random.Range(0, AnimTextObjects.Count)].PlayEffect();

            yield return null;
            
            if (Time.time - timer > 5f)
            {
                timer = Time.time;
                if (_currentLangAnim == 1)
                    _currentLangAnim = 4;
                else
                {
                    _currentLangAnim = 1;
                }

                ChangeLanguageAnim();
            }
        }
    }

    private void OnBack()
    {
        switch (CurrentLang)
        {
            case 1:
            {
                MainImage.sprite = MainSprite_Uzb;
                break;
            }
            case 2:
            {
                MainImage.sprite = MainSprite_Arab;
                break;
            }
            case 3:
            {
                MainImage.sprite = MainSprite_Eng;
                break;
            }
            case 4:
            {
                MainImage.sprite = MainSprite_Rus;
                break;
            }
        }
        OnDefault();
    }

    private void OnDefault()
    {
        DefaultScreen.SetActive(true);
        if(_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(StartAnimation());
        MySendMessage("23kartastandby");
    }

    private void OffDefault()
    {
        HideAllSliders();
        DefaultScreen.SetActive(false);
        if(_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private void OnLangUzb()
    {
        OffDefault();
        CurrentLang = 1;
        ChangeLanguage();
    }

    private void OnLangArab()
    {
        OffDefault();
        CurrentLang = 2;
        ChangeLanguage();
    }

    private void OnLangEng()
    {
        OffDefault();
        CurrentLang = 3;
        ChangeLanguage();
    }

    private void OnLangRus()
    {
        OffDefault();
        CurrentLang = 4;
        ChangeLanguage();
    }

    public void HideAllSliders()
    {
        foreach (var region in Regions)
        {
            region.Hide();
        }
    }

    public void MySendMessage(string str)
    {
        string message = "{\"jsonrpc\":\"2.0\", \"id\":39, \"method\":\"Pixera.Compound.applyCueOnTimeline\", \"params\":{\"timelineName\":\"alphaarea1\", \"cueName\":\"";
        message+=str;
        switch (CurrentLang)
        {
            case 1:
            {
                message+="uzb";
                break;
            }
            case 2:
            {
                message+="arab";
                break;
            }
            case 3:
            {
                message+="en";
                break;
            }
            case 4:
            {
                message+="ru";
                break;
            }
        }

        message += "\", \"blendDuration\":1}}";
        //Debug.Log(message);
        ClientUdp.AddMessage(message);
    }

}
