using TMPro;
using UnityEngine;

public class CrystalCountUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CrystalCount;
    private PlayerStats playerStatsInstance;

    void Start()
    {
        playerStatsInstance = PlayerStats.Instance;
        ChangeCrystalCountText(playerStatsInstance.Crystals);
        OnEnable(); //так как не факт, что Awake в PlayerStats вызовется раньше нашего OnEnable
    }

    void OnEnable()
    {
        if (playerStatsInstance != null)
        {
            playerStatsInstance.OnCrystalsChanged -= ChangeCrystalCountText;
            playerStatsInstance.OnCrystalsChanged += ChangeCrystalCountText;
        }
    }

    void OnDisable()
    {
        playerStatsInstance.OnCrystalsChanged -= ChangeCrystalCountText;
    }

    void ChangeCrystalCountText(int count) => CrystalCount.text = "" + count;

}
