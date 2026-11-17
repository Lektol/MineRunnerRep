using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinCard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name; 
    [SerializeField] private Image _image;

    public void Init(SkinDefinition skin)
    {
        _name.text = skin.SkinNameRU;
        if(skin.Sprite != null) _image.sprite = skin.Sprite;
    }
}
