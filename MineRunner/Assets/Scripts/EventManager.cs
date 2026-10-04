using System;

public class EventManager
{
    public static event Action OnPlayerDied; 
    public static event Action OnGetCrystal;


    public static void OnPlayerDiedInvoke()
    {
        OnPlayerDied?.Invoke();
    }

    public static void OnGetCrystalInvoke()
    {
        OnGetCrystal?.Invoke();
    }
}
