using UnityEngine.UI;
using UnityEngine;
using YG;
using System;

public class CanvasManager : MonoBehaviour
{
    public enum TypePanel
    {
        PlayPanel = 1,
        MenuPanel = 2,
        PausePanel = 3
    }

    [Serializable]
    public struct Panel
    {
        public TypePanel TypePanel;
        public GameObject ObjPanel;
    }
    [SerializeField] private Panel _playablePanel;
    [SerializeField] private Panel _menuPanel; 
    [SerializeField] private Panel _pausePanel;
    private Panel[] _allPanels;

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

    private void Start()
    {
        _allPanels = new[] {_playablePanel, _menuPanel ,_pausePanel};
    }

    public void StartGame()
    {
        EventManager.OnStartGameInvoke();
    }

    private void SetActivePlayablePanel()
    {
        SetActivePanel(TypePanel.PlayPanel);
    }

    private void SetActiveMenuPanel()
    {
        Time.timeScale = 1f; 
        SetActivePanel(TypePanel.MenuPanel);
    }

    public void SetActivePausePanel()
    {
        SetActivePanel(TypePanel.PausePanel);
        Time.timeScale = 0f;
    }

    public void ComebackToPlay()
    {
        Time.timeScale = 1f; 
        SetActivePanel(TypePanel.PlayPanel);
    }

    public void ExitToMenu()
    {
        EventManager.OnResetGameInvoke();
    }

    private void SetActivePanel(TypePanel panel)
    {
        foreach(var Panel in _allPanels)
        {
            if(Panel.ObjPanel == null) continue;
            Panel.ObjPanel.SetActive(Panel.TypePanel==panel);
        }
    }
}
