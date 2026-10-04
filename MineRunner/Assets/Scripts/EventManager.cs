using System;

public class EventManager
{
    public static event Action OnStartGame; 
    public static event Action OnPlayerDied; 
    public static event Action OnResetGame;
    public static event Action OnGetCrystal;
    public static event Action OnRebirth;
    public static event Action OnTotalLose;

    public static void OnStartGameInvoke()
    {
        OnStartGame?.Invoke();
    }

    public static void OnPlayerDiedInvoke()
    {
        OnPlayerDied?.Invoke();
    }

    public static void OnResetGameInvoke()
    {
        OnResetGame?.Invoke();
    }

    public static void OnGetCrystalInvoke()
    {
        OnGetCrystal?.Invoke();
    }

    public static void OnRebirthInvoke()
    {
        OnRebirth?.Invoke();
    }

    public static void OnTotalLoseInvoke()
    {
        OnTotalLose?.Invoke();
    }
}
