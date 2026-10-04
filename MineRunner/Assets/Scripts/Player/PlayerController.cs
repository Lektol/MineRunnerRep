using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerLifeCycle))]
[RequireComponent(typeof(PlayerView))]
[RequireComponent(typeof(PlayerCollision))]
public class PlayerController : MonoBehaviour
{
    // private PlayerMovement _movement;
    // private PlayerLifeCycle _lifeCycle;

    // private void Awake()
    // {
    //     _movement = GetComponent<PlayerMovement>();
    //     _lifeCycle = GetComponent<PlayerLifeCycle>();
    // }

    // private void OnEnable()
    // {
    //     EventManager.OnStartGame += StartGame;
    //     EventManager.OnLoseGame += LoseGame;
    //     EventManager.OnResetGame += ResetGame;
    //     EventManager.OnRebirth += Rebirth;
    // }

    // private void OnDisable()
    // {
    //     EventManager.OnStartGame -= StartGame;
    //     EventManager.OnLoseGame -= LoseGame;
    //     EventManager.OnResetGame -= ResetGame;
    //     EventManager.OnRebirth -= Rebirth;
    // }

    // private void StartGame()
    // {
    //     _lifeCycle.StartGame();
    // }

    // private void LoseGame()
    // {
    //     _lifeCycle.Die();
    // }

    // private void ResetGame()
    // {
    //     _lifeCycle.ResetToMenu();
    // }

    // private void Rebirth()
    // {
    //     _lifeCycle.Rebirth();
    // }

    // Оставляем публичный API для других систем,
    // если они захотят напрямую заставить игрока прыгнуть.
    // public void Jump(float multiplier = 1f)
    // {
    //     _movement.Jump(multiplier);
    // }

    // public void MoveDown()
    // {
    //     _movement.MoveDown();
    // }
}