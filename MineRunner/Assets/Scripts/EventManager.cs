using System;

public class EventManager
{
    public static event Action OnStartGame; 
    public static event Action OnLoseGame; 
    public static event Action OnResetGame;
    public static event Action OnGetCrystal;

    public static void OnStartGameInvoke()
    {
        OnStartGame?.Invoke();
    }

    public static void OnLoseGameInvoke()
    {
        OnLoseGame?.Invoke();
    }

    public static void OnResetGameInvoke()
    {
        OnResetGame?.Invoke();
    }

    public static void OnGetCrystalInvoke()
    {
        OnGetCrystal?.Invoke();
    }
}
