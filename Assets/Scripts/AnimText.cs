using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Accessibility;
using Random = UnityEngine.Random;

public class AnimText : MonoBehaviour
{
    
    private float _speed;
    public TMP_Text _text;
    public TMP_Text _text2;
    public Color _textColor;
    
    private Color _anyColor;
    private Color _color;
    private Coroutine _coroutine;
    private float _startScale;
    private Tweener _tweener;

    private void Start()
    {
        _speed = 1f;
        _startScale = transform.localScale.x;
        _anyColor = _textColor;
        _text2.color = new Color(_text2.color.r, _text2.color.g, _text2.color.b, _startScale);
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _startScale);
    }

    public void Init(string text, float speed)
    {
        _speed = speed;
        _text = GetComponent<TMP_Text>();
        _text.text = text;
        transform.localScale = Vector3.one/2f;
        transform.position = new Vector3(Random.Range(410,1640), Random.Range(176, 920), 0);
    }

    public void SetText(string text)
    {
        if (_text.color == _textColor)
        {
            _text.color = new Color(1,1,1,_text.color.a);
            _text2.color = new Color(_textColor.r, _textColor.g, _textColor.b, _text2.color.a);
        }
        else
        {
            _text.color = new Color(_textColor.r, _textColor.g, _textColor.b, _text.color.a);
            _text2.color = new Color(1,1,1,_text2.color.a);
        }

        StartCoroutine(StartChangeText(text));
    }

    public void PlayEffect()
    {
        // if(_coroutine != null)
        //     StopCoroutine(_coroutine);
        // _coroutine = StartCoroutine(StartAnim());
        if (_tweener != null && _tweener.IsActive())
        {
            return;
        }
        _tweener = transform.DOPunchScale(Vector3.one * 0.2f,5f, 0,0f).SetEase(Ease.OutQuart);
    }


    IEnumerator StartChangeText(string text)
    {
       
        string myText = text;
        _text.text = "";

        if (myText.Length > _text2.text.Length)
        {
            int count2 = myText.Length - _text2.text.Length;
            for (int i = 0; i < count2; i++)
            {
                _text2.text += " ";
            }
        }
        else
        {
            int count2 = _text2.text.Length - myText.Length;
            for (int i = 0; i < count2; i++)
            {
                myText += " ";
            }
        }
        
        int count = myText.Length;
        //Debug.Log(count + " "+ myText.Length + " " + _text2.text.Length);

        for (int i = 0; i < count; i++)
        {
            // for (int j = 0; j < 5; j++)
            // {
            //     if (myText[i] != ' ')
            //     {
            //         _text.text = _text.text.Substring(0, i);
            //         _text.text = _text.text + j;
            //     }
            //     yield return new WaitForSeconds(0.02f);
            // }
            yield return new WaitForSeconds(0.02f);
            _text.text = _text.text.Substring(0, i)+myText[i];
            string space = "";
            for (int j = 0; j < i+1; j++)
            {
                space += " ";
            }

            _text2.text = space + _text2.text.Substring(i+1, count-i-1);
            yield return null;
        }

        _text2.text = text;
        _text.text = "";
        _text2.color = _text.color;
    }

    IEnumerator StartAnim()
    {
        while (transform.localScale.x<1f)
        {
            float delta = _speed * Time.deltaTime;
            transform.localScale += Vector3.one * delta;
            _color = _text.color;
            _color.a += delta;
            _text.color = _color;
            _color = _text2.color;
            _color.a += delta;
            _text2.color = _color;
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);
        
        while (transform.localScale.x>_startScale)
        {
            float delta = _speed * Time.deltaTime;
            transform.localScale -= Vector3.one * delta;
            _color = _text.color;
            _color.a -= delta;
            _text.color = _color;
            _color = _text2.color;
            _color.a -= delta;
            _text2.color = _color;
            yield return null;
        }
    }

}
