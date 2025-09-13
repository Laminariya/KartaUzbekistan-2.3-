using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimStartScreen : MonoBehaviour
{
    public float Frequency;
    public float SpeedMove;
    public GameObject MoveTextPrefab;
    public Transform MoveTextParent;
    
    public List<RectArea> RectAreas = new List<RectArea>();

    private Vector3 _scale;
    private float _timer;
    private int _number;


    private void Start()
    {
        _number = 0;
        _timer = Time.time;
    }

    private void Update()
    {
        if (Time.time - _timer > Frequency)
        {
            CreateAnimText();
            _timer = Time.time;
        }
    }

    private void CreateAnimText()
    {
        MoveText animTextObject =
            Instantiate(MoveTextPrefab, MoveTextParent).GetComponent<MoveText>();
        animTextObject.transform.SetAsFirstSibling();
        animTextObject.transform.localPosition = GetRandomVector();
        animTextObject.Init(SpeedMove,GameManager.instance.GetRandomString());
    }

    private Vector2 GetRandomVector()
    {
        _number++;
        if(_number >= RectAreas.Count)
            _number = 0;
        Vector2 randomVector = new Vector2(
            Random.Range(RectAreas[_number].LeftUp.localPosition.x, RectAreas[_number].RightDown.localPosition.x),
            Random.Range(RectAreas[_number].RightDown.localPosition.y, RectAreas[_number].LeftUp.localPosition.y));
        return randomVector;
    }

    IEnumerator StartAnimation_PlayEffects()
    {
        float timer = Time.time;
        while (true)
        {
            _scale = Vector3.one;

            yield return new WaitForSeconds(0.3f);

            //AnimTextObjects[Random.Range(0, AnimTextObjects.Count)].PlayEffect();

            yield return null;

            if (Time.time - timer > 5f)
            {
                timer = Time.time;
                if (GameManager.instance._currentLangAnim == 1)
                    GameManager.instance._currentLangAnim = 4;
                else
                {
                    GameManager.instance._currentLangAnim = 1;
                }
                
            }
        }
    }
}

[Serializable]
public class RectArea
{
    public Transform LeftUp;
    public Transform RightDown;
}