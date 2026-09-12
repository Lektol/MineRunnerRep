using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class RebirthPanel : MonoBehaviour
{
    [SerializeField] private GameObject _rebPanel;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private TextMeshProUGUI _textSec;
    [SerializeField] private int _secToOffer;

    void OnEnable()
    {
        EventManager.OnLoseGame += StartCoroutineOffer;
    }

    void OnDisable()
    {
        EventManager.OnLoseGame -= StartCoroutineOffer;
    }

    void StartCoroutineOffer()
    {
        StartCoroutine(OfferRebirth());
    }
    IEnumerator OfferRebirth()
    {
        _pauseButton.SetActive(false);
        _rebPanel.SetActive(true);
        for(int i = _secToOffer; i > 0; i--)
        {
            _textSec.text = "" + i;
            yield return new WaitForSeconds(1);
        }
        RefuseRebirth();
    }

    public void RefuseRebirth()
    {
        StopAllCoroutines();
        _rebPanel.SetActive(false);
        _pauseButton.SetActive(true);
        EventManager.OnResetGameInvoke();
    }

    public void GetOffer()
    {
        StopAllCoroutines();
        _rebPanel.SetActive(false);
        _pauseButton.SetActive(true);
        YG2.InterstitialAdvShow();
        EventManager.OnRebirthInvoke();
    }
}
