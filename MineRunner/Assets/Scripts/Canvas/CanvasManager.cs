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
        PausePanel = 3,
    }

    [Serializable]
    public struct Panel
    {
        public TypePanel TypePanel;
        public GameObject ObjPanel;
    }
    [Header("Главные панели")]
    [SerializeField] private Panel _playablePanel;
    [SerializeField] private Panel _menuPanel; 
    [SerializeField] private Panel _pausePanel;
    [Header("Панели внутри главных")]
    [SerializeField] private GameObject _rebithPanel;
    private Panel[] _allPanels;

    private void Start()
    {
        _allPanels = new[] {_playablePanel, _menuPanel ,_pausePanel};
    }

    public void StartGame()
    {
        GameFlow.Instance.StartGame();
    }

    public void SetActivePlayablePanel()
    {
        SetActivePanel(TypePanel.PlayPanel);
    }

    public void SetActiveMenuPanel()
    {
        SetActivePanel(TypePanel.MenuPanel);
    }

    public void SetActivePausePanel()
    {
        //GameFlow.Instance.Pause();
        SetActivePanel(TypePanel.PausePanel);
    }

    public void Pause()
    {
        GameFlow.Instance.Pause();
    }

    public void ComebackToPlay()
    {
        GameFlow.Instance.Resume();
    }

    public void ExitToMenu()
    {
        GameFlow.Instance.EndRun();
    }

    public void SetActiveRebithPanel()
    {
        _rebithPanel.SetActive(true);
    }

    public void SetActivePanel(TypePanel panel)
    {
        foreach(var Panel in _allPanels)
        {
            if(Panel.ObjPanel == null) continue;
            Panel.ObjPanel.SetActive(Panel.TypePanel==panel);
        }
    }
}
