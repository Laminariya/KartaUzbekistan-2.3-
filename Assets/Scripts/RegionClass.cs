using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionClass : MonoBehaviour
{

    public List<Sprite> MainSprites = new List<Sprite>();
    public List<Sprite> SliderSprites = new List<Sprite>();

    public GameObject SliderPanel;
    public Image SliderImage;
    
    private Button _button;
    
    public void Init()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    public void Hide()
    {
        if (SliderPanel != null)
            SliderPanel.SetActive(false);
    }

    private void OnClick()
    {
        GameManager.instance.HideAllSliders();
        switch (GameManager.instance.CurrentLang)
        {
            case 1:
            {
                GameManager.instance.MainImage.sprite = MainSprites[0];
                if(SliderPanel != null)
                    SliderImage.sprite = SliderSprites[0];
                break;
            }
            case 2:
            {
                GameManager.instance.MainImage.sprite = MainSprites[1];
                if(SliderPanel != null)
                    SliderImage.sprite = SliderSprites[1];
                break;
            }
            case 3:
            {
                GameManager.instance.MainImage.sprite = MainSprites[2];
                if(SliderPanel != null)
                    SliderImage.sprite = SliderSprites[2];
                break;
            }
            case 4:
            {
                GameManager.instance.MainImage.sprite = MainSprites[3];
                if(SliderPanel != null)
                    SliderImage.sprite = SliderSprites[3];
                break;
            }
        }

        if (SliderPanel != null)
        {
            SliderPanel.SetActive(true);
        }

    }


}
