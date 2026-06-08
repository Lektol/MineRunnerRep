using TMPro;
using UnityEngine;

public class CrystalCountUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CrystalCount;

    void Start()
    {
        ChangeCrystalCountText(PlayerStats.Instance.Crystals);
    }

    void OnEnable()
    {
        PlayerStats.Instance.OnCrystalsChanged += ChangeCrystalCountText;
    }

    void OnDisable()
    {
        PlayerStats.Instance.OnCrystalsChanged -= ChangeCrystalCountText;
    }

    void ChangeCrystalCountText(int count) => CrystalCount.text = "" + count;

}
