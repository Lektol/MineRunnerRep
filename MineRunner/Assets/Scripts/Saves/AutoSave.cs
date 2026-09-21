using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class AutoSave : MonoBehaviour
{
    [SerializeField] private float SecBetweenSaves;
    private Coroutine coroutine;

    private void OnDisable()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    void Start()
    {
        coroutine = StartCoroutine(Save());
    }
    IEnumerator Save()
    {
        yield return new WaitForSeconds(SecBetweenSaves);
        YG2.SaveProgress();
        YG2.SetLeaderboard("LeaderBoard", PlayerStats.Instance.Crystals);
        coroutine = null;
        coroutine = StartCoroutine(Save());
        Debug.Log("Сохранил прогресс");
    }
}
