using UnityEngine.UI;
using UnityEngine;
using YG;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject _playablePanel;
    [SerializeField] private GameObject _menuPanel; 

    void OnEnable()
    {
        EventManager.OnStartGame += SetActivePlayablePanel;
        EventManager.OnResetGame += SetActiveMenuPanel;
    }

    void OnDisable()
    {
        EventManager.OnStartGame -= SetActivePlayablePanel;
        EventManager.OnResetGame -= SetActiveMenuPanel;
    }

    public void StartGame()
    {
        EventManager.OnStartGameInvoke();
    }

    void SetActivePlayablePanel()
    {
        _menuPanel.SetActive(false);
        _playablePanel.SetActive(true);
    }

    void SetActiveMenuPanel()
    {
        _menuPanel.SetActive(true);
        _playablePanel.SetActive(false);
    }
}
