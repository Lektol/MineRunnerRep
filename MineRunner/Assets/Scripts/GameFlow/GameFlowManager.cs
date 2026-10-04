using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class GameFlow : MonoBehaviour
{
    public static GameFlow Instance {get; private set;}
    [SerializeField] private PlayerLifeCycle _playerLifeCycle;
    [SerializeField] private RoadGenerator _road;
    [SerializeField] private CavesGenerator _caves;
    [SerializeField] private CanvasManager _canvas;
    [SerializeField] private CameraController _camera;
    [SerializeField] private PlayerStats _playerStats;

    public GameState State { get; private set; }

    private void OnEnable()
    {
        EventManager.OnPlayerDied += PlayerDied;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerDied -= PlayerDied;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetState(GameState.Menu);
    }

    public void StartGame()
    {
        if (State != GameState.Menu)
            return;

        Time.timeScale = 1f;

        _road.StartLevel();
        _caves.SetGameSpeed();
        _playerLifeCycle.StartGame();

        _canvas.SetActivePlayablePanel();
        _camera.SetMainPos();

        SetState(GameState.Playing);
    }

    public void Pause()
    {
        if (State != GameState.Playing)
            return;

        _canvas.SetActivePausePanel();

        SetState(GameState.Paused);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (State != GameState.Paused)
            return;

        Time.timeScale = 1f;

        _canvas.SetActivePlayablePanel();

        SetState(GameState.Playing);
    }

    private void PlayerDied()
    {
        if (State != GameState.Playing)
            return;

        _road.StopLevel();
        _caves.StopLevel();
        _playerLifeCycle.Die();

        _canvas.SetActiveRebithPanel();

        SetState(GameState.Dead);
    }

    public void Rebirth()
    {
        if (State != GameState.Dead)
            return;

        Time.timeScale = 1f;

        _playerLifeCycle.Rebirth();
        _road.StartLevel();
        _caves.SetGameSpeed();

        _canvas.SetActivePlayablePanel();

        SetState(GameState.Playing);
    }

    public void EndRun()
    {
        // if (State != GameState.Dead &&
        //     State != GameState.Playing)
        //     return;

        Time.timeScale = 1f;

        _playerStats.SaveProgressAndSetNull();

        _playerLifeCycle.ResetToMenu();
        _road.ResetLevel();
        _caves.ResetLevel();

        _canvas.SetActiveMenuPanel();
        _camera.SetMenuPos();

        YG2.InterstitialAdvShow();

        SetState(GameState.Menu);
    }

    private void SetState(GameState state)
    {
        State = state;
    }
}
