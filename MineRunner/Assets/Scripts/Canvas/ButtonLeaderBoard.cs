using UnityEngine;

public class ButtonLeaderBoard : MonoBehaviour
{
    [SerializeField] private GameObject _leaderBoard;

    public void SetLeaderBoard()
    {
        _leaderBoard.SetActive(!_leaderBoard.activeSelf);
    }
}
