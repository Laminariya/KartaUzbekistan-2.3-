using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MoveText : MonoBehaviour
{
    
    private float _speed;
    
    public TMP_Text Text;
    public Image Image;
    
    private Color _originalImageColor;
    private Color _originalTextColor;
    
    public void Init(float speed, string text)
    {
        Text.text = text;
        _speed = speed;
        Text.color = new Color(Text.color.r, Text.color.g, Text.color.b, 0f);
        Image.color = new Color(Image.color.r, Image.color.g, Image.color.b, 0f);
        _originalImageColor = Image.color;
        _originalTextColor = Text.color;
    }

    private void Update()
    {
        transform.Translate(Vector3.back * (_speed * Time.deltaTime));

        if (transform.localPosition.z > -300f && _originalImageColor.a < 0.99f)
        {
            _originalImageColor.a += _speed * Time.deltaTime/200f;
            _originalTextColor.a += _speed * Time.deltaTime/200f;
            Text.color = _originalTextColor;
            Image.color = _originalImageColor;
        }

        if (transform.localPosition.z < -600f)
        {
            _originalImageColor.a -= _speed * Time.deltaTime/200f;
            _originalTextColor.a -= _speed * Time.deltaTime/200f;
            Text.color = _originalTextColor;
            Image.color = _originalImageColor;
        }

        if (transform.localPosition.z < -1200f)
        {
            Destroy(gameObject);
        }
    }

   
}
