using System;

public class EventManager
{
    public static event Action OnPlayerDied;
    public static event Action OnGetCrystal;
    public static event Action<float, float> OnCameraShakeRequested;
    public static event Action<SkinDefinition> OnShopChanged; //Скин куплен
    public static event Action<int> OnBalanceChanged; 

    public static void OnPlayerDiedInvoke()
    {
        OnPlayerDied?.Invoke();
    }

    public static void OnGetCrystalInvoke()
    {
        OnGetCrystal?.Invoke();
    }

    public static void RequestCameraShake(float duration, float magnitude)
    {
        OnCameraShakeRequested?.Invoke(duration, magnitude);
    }

    public static void ShopChanged(SkinDefinition skin)
    {
        OnShopChanged?.Invoke(skin);
    }

    public static void BalanceChanged(int count)
    {
        OnBalanceChanged?.Invoke(count);
    }
}
