using YG;
using TMPro;
using UnityEngine;
using System.Collections;

public class ShowAdd : MonoBehaviour
{
    [SerializeField] private GameObject _addPanel;
    [SerializeField] private TextMeshProUGUI _textSec;
    [SerializeField] private TextMeshProUGUI _text;
    private Coroutine coroutine;

    private void OnEnable()
    {
        EventManager.OnTotalLose += ShowAd;
    }

    private void OnDisable()
    {
        EventManager.OnTotalLose -= ShowAd;
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    private void ShowAd()
    {
        coroutine = StartCoroutine(ShowSuccessPanel());
    }

    private IEnumerator ShowSuccessPanel()
    {
        _addPanel.SetActive(true);
        int sec_adv = 2;
        for(int i = sec_adv; i > 0; i--)
        {
            if(YG2.envir.language == "ru") _text.text = "Реклама через: " + i;
            else _text.text = "Advertising in: " + i;
            yield return new WaitForSeconds(1);
        }
        if (!YG2.nowAdsShow)
        {
            YG2.InterstitialAdvShow();
        } 
        _addPanel.SetActive(false);
        coroutine = null;
    }
}
