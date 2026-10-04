using System;
using UnityEngine;
using YG;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    private int _crystals = 0;
    public int Crystals => _crystals;
    public event Action<int> OnCrystalsChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        EventManager.OnGetCrystal += AddCrystal;
        //EventManager.OnTotalLose += SaveProgressAndSetNull;
    }

    private void OnDisable()
    {
        EventManager.OnGetCrystal -= AddCrystal;
        //EventManager.OnTotalLose -= SaveProgressAndSetNull;
    }

    private void AddCrystal()
    {
        _crystals += 1;
        OnCrystalsChanged?.Invoke(_crystals);
    }
    public void SaveProgressAndSetNull()
    {
        if(_crystals > YG2.saves.MaxCrystals)
        {
            YG2.SetLeaderboard("LeaderBoardSpike", _crystals);
            YG2.saves.MaxCrystals = _crystals;
            YG2.SaveProgress();
        }
        _crystals = 0;
        OnCrystalsChanged?.Invoke(_crystals);
    }
}
