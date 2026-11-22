using TMPro;
using UnityEngine;

/// <summary>
/// Счётчик для забега, т.е. для кристаллов, которые собраны во время игры 
/// </summary>
public class CrystalCountUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _crystalCount;
    private PlayerStats _playerStatsInstance;

    private void Start()
    {
        _playerStatsInstance = PlayerStats.Instance;
        ChangeCrystalCountText(_playerStatsInstance.CollectedCrystals);
        SubscribeToCrystalChanged();
    }

    private void OnEnable()
    {
        SubscribeToCrystalChanged();
    }

    private void OnDisable()
    {
        _playerStatsInstance.OnCrystalsChanged -= ChangeCrystalCountText;
    }

    private void SubscribeToCrystalChanged()
    {
        if (_playerStatsInstance != null)
        {
            _playerStatsInstance.OnCrystalsChanged -= ChangeCrystalCountText;
            _playerStatsInstance.OnCrystalsChanged += ChangeCrystalCountText;
        }
    }

    private void ChangeCrystalCountText(int count) => _crystalCount.text = "" + count;

}
