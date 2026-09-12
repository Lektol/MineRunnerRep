using TMPro;
using UnityEngine;

public class CrystalCountUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _crystalCount;
    private PlayerStats _playerStatsInstance;

    private void Start()
    {
        _playerStatsInstance = PlayerStats.Instance;
        ChangeCrystalCountText(_playerStatsInstance.Crystals);
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
