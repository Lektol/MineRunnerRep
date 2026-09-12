using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class RebirthPanel : MonoBehaviour
{
    [SerializeField] private GameObject _rebPanel;
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
        _rebPanel.SetActive(true);
        for(int i = _secToOffer; i > 0; i--)
        {
            _textSec.text = "" + i;
            yield return new WaitForSeconds(1);
        }
        ExitPanel();
    }

    public void ExitPanel()
    {
        StopAllCoroutines();
        _rebPanel.SetActive(false);
        EventManager.OnResetGameInvoke();
    }

    public void GetOffer()
    {
        StopAllCoroutines();
        _rebPanel.SetActive(false);
        YG2.InterstitialAdvShow();
        EventManager.OnRebirthInvoke();
    }
}
