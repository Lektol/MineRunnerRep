using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class RebirthPanel : MonoBehaviour
{
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private TextMeshProUGUI _textSec;
    [SerializeField] private int _secToOffer;

    private void OnEnable()
    {
        StartCoroutine(OfferRebirth());
    }
    private IEnumerator OfferRebirth()
    {
        _pauseButton.SetActive(false);
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
        _pauseButton.SetActive(true);
        gameObject.SetActive(false);
        GameFlow.Instance.EndRun();
    }

    public void GetOffer()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        _pauseButton.SetActive(true);
        YG2.InterstitialAdvShow();
        GameFlow.Instance.Rebirth();
    }
}
