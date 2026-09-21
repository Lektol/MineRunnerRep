using UnityEngine;

public class ButtonLeaderBoard : MonoBehaviour
{
    [SerializeField] private GameObject _leaderBoard;

    private void OnEnable()
    {
        EventManager.OnStartGame += OffLeaderBoard;
    }

    private void OnDisable()
    {
        EventManager.OnStartGame -= OffLeaderBoard;
    }

    public void SetLeaderBoard()
    {
        _leaderBoard.SetActive(!_leaderBoard.activeSelf);
    }

    private void OffLeaderBoard()
    {
        _leaderBoard.SetActive(false);
    }
}
