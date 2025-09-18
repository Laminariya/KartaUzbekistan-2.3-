using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrunoMikoski.TextJuicer;
using BrunoMikoski.TextJuicer.Modifiers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RegionClass : MonoBehaviour
{

    public string Number;

    public AnimTextClass Name;
    public GameObject Temi;
    
    public List<string> MenuText1 = new List<string>();
    public List<string> MenuText2 = new List<string>();
    
    private List<Image> Images = new List<Image>();
    private List<AnimTextClass> TextJuicers = new List<AnimTextClass>();
    
    public Button Button;
    private GameManager _manager;
    private Image _image;
    
    public void Init()
    {
        _image = GetComponent<Image>();
        gameObject.SetActive(true);
        _manager = GameManager.instance;
        Button = GetComponentInChildren<Button>();
        Button.onClick.AddListener(OnClick);
        Images = GetComponentsInChildren<Image>(true).ToList();
        TextJuicers = GetComponentsInChildren<AnimTextClass>(true).ToList();
        Images.Remove(Images[Images.Count - 1]);

        bool number = false;
        for (int i = 0; i < Images.Count; i++)
        {
            if (Images[i].name == "Button" || number)
            {
                number = true;
                Images.Remove(Images[i]);
            }
        }

        if (Name != null)
        {
            Name.Init();
            Name.textJuicer.SetProgress(0);
            Name.textJuicer.Update();
        }

        foreach (var textClass in TextJuicers)
        {
            textClass.Init();
        }
        Hide();
    }

    public void Hide()
    {
        _manager.TemiObrasheniy.gameObject.SetActive(false);
        _image.enabled = false;
        
        foreach (var textJuicer in TextJuicers)
        {
            textJuicer.gameObject.SetActive(false);
        }

        foreach (var image in Images)
        {
            image.enabled = false;
        }
        if (Name != null)
        {
            Name.textJuicer.SetProgress(0);
            Name.textJuicer.Update();
        }
    }

    public void ChangeLang()
    {
        foreach (var textJuicer in TextJuicers)
        {
            textJuicer.ChangeLanguage(_manager.CurrentLang);
        }
        if (Name != null)
        {
            Name.ChangeLanguage(_manager.CurrentLang);
        }
        _manager.NameRegion.Text = MenuText1[_manager.CurrentLang];
        _manager.DiscriptionRegion.Text = MenuText2[_manager.CurrentLang];
    }

    private void OnClick()
    {
        _manager.OffRegionButtons();
        ChangeLang();
        
        if(_manager.CurrentRegion!=null)
            _manager.CurrentRegion.Hide();
        _manager.TemiObrasheniy.gameObject.SetActive(true);
        _manager.CurrentRegion = this;
        _image.enabled = true;
        
       
        _manager.NameRegion.SetProgress(0);
        _manager.NameRegion.Update();
        _manager.DiscriptionRegion.SetProgress(0);
        _manager.DiscriptionRegion.Update();
        
        foreach (var image in Images)
        {
            image.enabled = true;
            image.color = new Color(1f, 1f, 1f, 0f);
            image.DOFade(1f, 0.5f);
        }
        
        _image.color = new Color(1f, 1f, 1f, 0f);

        _image.DOFade(1f, 0.5f).OnComplete(StartShowAnim);
        

        GameManager.instance.MySendMessage("23kartastena"+Number);
    }

    public void StartShowAnim()
    {
        foreach (var textJuicer in TextJuicers)
        {
            textJuicer.gameObject.SetActive(true);
            textJuicer.textJuicer.SetProgress(0f);
            textJuicer.textJuicer.Update();
        }

        if (Name != null)
        {
            Name.textJuicer.SetProgress(0);
            Name.textJuicer.Update();
        }
        
        _manager.NameRegion.SetProgress(0);
        _manager.NameRegion.Update();
        _manager.DiscriptionRegion.SetProgress(0);
        _manager.DiscriptionRegion.Update();
        
        StartCoroutine(ShowAnim());
    }

    IEnumerator ShowAnim()
    {
        float progress = 1f;
        
        progress = 0f;
        while (progress<1f)
        {
            progress += Time.deltaTime * _manager.SpeedAnimText;
            foreach (var textJuicer in TextJuicers)
            {
                textJuicer.textJuicer.SetProgress(progress);
                textJuicer.textJuicer.Update();
            }

            if (Name != null)
            {
                Name.textJuicer.SetProgress(progress);
                Name.textJuicer.Update();
            }
            
            _manager.NameRegion.SetProgress(progress);
            _manager.NameRegion.Update();
            _manager.DiscriptionRegion.SetProgress(progress);
            _manager.DiscriptionRegion.Update();
            
            yield return null;
        }
        _manager.OnRegionButtons();
    }

    IEnumerator HideAnim()
    {
        float progress = 1f;
        while (progress>0f)
        {
            progress -= Time.deltaTime * _manager.SpeedAnimText;
            foreach (var textJuicer in TextJuicers)
            {
                textJuicer.textJuicer.SetProgress(progress);
                textJuicer.textJuicer.Update();
            }

            if (Name != null)
            {
                Name.textJuicer.SetProgress(progress);
                Name.textJuicer.Update();
            }

            _manager.NameRegion.SetProgress(progress);
            _manager.NameRegion.Update();
            _manager.DiscriptionRegion.SetProgress(progress);
            _manager.DiscriptionRegion.Update();
            
            yield return null;
        }
    }


}
