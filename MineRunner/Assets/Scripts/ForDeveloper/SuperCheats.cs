using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SuperCheats : MonoBehaviour
{
    // Ваш метод, который хотите вызывать
    public void InvokeStartIvent()
    {
        EventManager.OnStartGameInvoke();
    }

    // Этот код добавит кнопку в инспекторе
    #if UNITY_EDITOR
    [CustomEditor(typeof(SuperCheats))]
    public class MyScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // Рисуем стандартные поля
            
            SuperCheats script = (SuperCheats)target;
            
            if (GUILayout.Button("InvokeStart")) // Кнопка
            {
                script.InvokeStartIvent(); // Вызов метода
            }
        }
    }
    #endif
}
