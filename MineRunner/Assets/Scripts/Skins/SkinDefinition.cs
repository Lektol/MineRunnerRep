using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinDefinition : MonoBehaviour
{
    [Header("Основная информация")]
    [SerializeField] private string _id;
    [SerializeField] private string _skinNameRU;
    [SerializeField] private string _skinNameEN;
    [SerializeField] private Sprite _sprite;

    [Header("Получение")]
    [SerializeField] private SkinUnlockType _unlockType;

    [SerializeField] private int _price;

    [Tooltip("ID товара в каталоге Яндекс Игр")]
    [SerializeField] private string _yandexProductId;


    public string Id => _id;
    public string SkinNameRU => _skinNameRU;
    public string SkinNameEN => _skinNameEN;
    public SkinUnlockType UnlockType => _unlockType;
    public int Price => _price;
    public string YandexProductId => _yandexProductId;
    public Sprite Sprite => _sprite;
}
