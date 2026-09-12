using TMPro;
using UnityEngine;

public class CrystalCountUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _crystalCount;
    private PlayerStats _playerStatsInstance;

    void Start()
    {
        _playerStatsInstance = PlayerStats.Instance;
        ChangeCrystalCountText(_playerStatsInstance.Crystals);
        OnEnable(); //так как не факт, что Awake в PlayerStats вызовется раньше нашего OnEnable
    }

    void OnEnable()
    {
        if (_playerStatsInstance != null)
        {
            _playerStatsInstance.OnCrystalsChanged -= ChangeCrystalCountText;
            _playerStatsInstance.OnCrystalsChanged += ChangeCrystalCountText;
        }
    }

    void OnDisable()
    {
        _playerStatsInstance.OnCrystalsChanged -= ChangeCrystalCountText;
    }

    void ChangeCrystalCountText(int count) => _crystalCount.text = "" + count;

}
