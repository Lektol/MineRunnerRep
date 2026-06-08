using System;

public class EventManager
{
    public static event Action OnStartGame; 
    public static event Action OnLooseGame; 
    public static event Action OnGetCrystal;

    public static void OnStartGameInvoke()
    {
        OnStartGame?.Invoke();
    }

    public static void OnLooseGameInvoke()
    {
        OnLooseGame?.Invoke();
    }

    public static void OnGetCrystalInvoke()
    {
        OnGetCrystal?.Invoke();
    }
}
