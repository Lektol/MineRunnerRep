using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager
{
    public static event Action OnStartGame; 
    public static event Action OnLooseGame; 

    public static void OnStartGameInvoke()
    {
        OnStartGame?.Invoke();
    }

    public static void OnLooseGameInvoke()
    {
        OnLooseGame?.Invoke();
    }
}
