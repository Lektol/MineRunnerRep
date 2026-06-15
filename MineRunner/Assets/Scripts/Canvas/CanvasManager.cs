using UnityEngine.UI;
using UnityEngine;
using YG;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject PlayablePanel;
    [SerializeField] private GameObject MenuPanel; 

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
        MenuPanel.SetActive(false);
        PlayablePanel.SetActive(true);
    }

    void SetActiveMenuPanel()
    {
        MenuPanel.SetActive(true);
        PlayablePanel.SetActive(false);
    }
}
