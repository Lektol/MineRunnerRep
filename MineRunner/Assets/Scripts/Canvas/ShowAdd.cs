using YG;
using TMPro;
using UnityEngine;
using System.Collections;

public class ShowAdd : MonoBehaviour
{
    [SerializeField] private GameObject _addPanel;
    [SerializeField] private TextMeshProUGUI _text;
    private Coroutine _coroutine;

    private void OnDisable()
    {
        if(_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }

    private void ShowAd()
    {
        _coroutine = StartCoroutine(ShowSuccessPanel());
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
        _coroutine = null;
    }
}
