using System.Collections;
using System.Collections.Generic;
using System.Text;
using BrunoMikoski.TextJuicer;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    [HideInInspector] public ClientUDP ClientUdp;
    public GameObject DefaultScreen;

    public Button b_Uzb;
    public Button b_Rus;
    public AnimTextClass TemiObrasheniy;
    
    public Button BackButton;
    public List<RegionClass> Regions = new List<RegionClass>();


    public Dictionary<int, List<string>> languageList = new Dictionary<int, List<string>>();
    public List<string> RusLang = new List<string>();
    public List<string> UzbLang = new List<string>();
    public List<AnimTextClass> NameRegions = new List<AnimTextClass>();


    public float SpeedAnimText;
    [HideInInspector] public int CurrentLang = 0;
    [HideInInspector] public RegionClass CurrentRegion;
    private Coroutine _coroutine;
    private float _timeout;
    private bool _isDown;
    public int _currentLangAnim;
    private Color _currentColor;

    public TMP_TextJuicer NameRegion;
    public TMP_TextJuicer DiscriptionRegion;
    
    public AnimTextClass BackText;

    private float _timer;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        _currentColor = b_Rus.image.color;
        ClientUdp = GetComponent<ClientUDP>();
        b_Uzb.onClick.AddListener(OnLangUzb);
        b_Rus.onClick.AddListener(OnLangRus);
        
        foreach (var region in Regions)
        {
            region.Init();
        }

        BackButton.onClick.AddListener(OnBack);
        languageList.Add(0, new List<string>(UzbLang));
        languageList.Add(1, new List<string>(RusLang));
        CurrentLang = 1;
        _currentLangAnim = 1;
        TemiObrasheniy.Init();
        TemiObrasheniy.gameObject.SetActive(false);
        foreach (var nameRegion in NameRegions)
        {
            nameRegion.Init();
        }
        BackText.Init();
        ClientUdp.Init();
    }

    private void ChangeLanguage()
    {
        TemiObrasheniy.ChangeLanguage(CurrentLang);
        foreach (var nameRegion in NameRegions)
        {
            nameRegion.ChangeLanguage(CurrentLang);
        }
        BackText.ChangeLanguage(CurrentLang);
        StartCoroutine(ShowAnim());
    }
    
    IEnumerator ShowAnim()
    {
        float progress = 0f;
        
        while (progress<1f)
        {
            progress += Time.deltaTime * SpeedAnimText;
            foreach (var textJuicer in NameRegions)
            {
                textJuicer.textJuicer.SetProgress(progress);
                textJuicer.textJuicer.Update();
            }
            
            BackText.textJuicer.SetProgress(progress);
            BackText.textJuicer.Update();
            
            yield return null;
        }
    }

    // private void Update()
    // {
    //     if (Input.anyKeyDown)
    //     {
    //         _timer = Time.time;
    //     }
    //
    //     if (!DefaultScreen.activeSelf && Time.time - _timer > 100f)
    //     {
    //         OnBack();
    //     }
    // }


    

    private void OnBack()
    {
        BackButton.enabled = false;
        BackButton.image.DOFade(1f, 0.3f);
        BackButton.image.DOFade(0f, 0.3f).SetDelay(0.3f).OnComplete(OnDefault);
    }

    private void OnDefault()
    {
        if(CurrentRegion != null)
            CurrentRegion.Hide();
        TemiObrasheniy.gameObject.SetActive(false);
        DefaultScreen.SetActive(true);
        MySendMessage("23kartastandby01");
        BackButton.enabled = true;
    }

    private void OffDefault()
    {
        //Debug.Log("OffDefault1");
        //HideAllSliders();
        TemiObrasheniy.gameObject.SetActive(false);
        DefaultScreen.SetActive(false);
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        ChangeLanguage();
        //Debug.Log("OffDefault2");
        b_Uzb.enabled = true;
        b_Rus.enabled = true;
        NameRegion.SetProgress(0f);
        NameRegion.Update();
        DiscriptionRegion.SetProgress(0f);
        NameRegion.Update();
    }

    private void OnLangUzb()
    {
        //Debug.Log("OnLangUzb");
        CurrentLang = 0;
        b_Uzb.enabled = false;
        b_Rus.enabled = false;
        b_Uzb.image.DOFade(1f, 0.3f);
        b_Uzb.image.DOFade(0f, 0.3f).SetDelay(0.3f).OnComplete(OffDefault);
    }

    private void OnLangRus()
    {
        CurrentLang = 1;
        b_Uzb.enabled = false;
        b_Rus.enabled = false;
        b_Rus.image.DOFade(1f, 0.3f);
        b_Rus.image.DOFade(0f, 0.3f).SetDelay(0.3f).OnComplete(OffDefault);
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
        string message =
            "{\"jsonrpc\":\"2.0\", \"id\":39, \"method\":\"Pixera.Compound.applyCueOnTimeline\", \"params\":{\"timelineName\":\"area23\", \"cueName\":\"";
        message += str;
        switch (CurrentLang)
        {
            case 0:
            {
                message += "uz";
                break;
            }
            case 1:
            {
                message += "ru";
                break;
            }
        }

        message += "\", \"blendDuration\":1}}";
        //Debug.Log(message);
        ClientUdp.AddMessage(message);
    }

    public string GetRandomString()
    {
        string result = "";
        switch (CurrentLang)
        {
            case 0:
            {
                return UzbLang[Random.Range(0, UzbLang.Count)];
            }
            case 1:
            {
                return RusLang[Random.Range(0, RusLang.Count)];
            }
            default:
            {
                return result;
            }
        }
    }

    public void OffRegionButtons()
    {
        foreach (var region in Regions)
        {
            region.Button.enabled = false;
        }
    }
    
    public void OnRegionButtons()
    {
        foreach (var region in Regions)
        {
            region.Button.enabled = true;
        }
    }
}