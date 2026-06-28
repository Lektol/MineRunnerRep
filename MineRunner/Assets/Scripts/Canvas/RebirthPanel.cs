using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using YG;

public class RebirthPanel : MonoBehaviour
{
    [SerializeField] private GameObject RebPanel;
    [SerializeField] private TextMeshProUGUI TextSec;
    [SerializeField] private int SecToOffer;

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
        RebPanel.SetActive(true);
        for(int i = SecToOffer; i > 0; i--)
        {
            TextSec.text = "" + i;
            yield return new WaitForSeconds(1);
        }
        ExitPanel();
    }

    public void ExitPanel()
    {
        StopAllCoroutines();
        RebPanel.SetActive(false);
        EventManager.OnResetGameInvoke();
    }

    public void GetOffer()
    {
        StopAllCoroutines();
        RebPanel.SetActive(false);
        YG2.InterstitialAdvShow();
        EventManager.OnRebirthInvoke();
    }
}
