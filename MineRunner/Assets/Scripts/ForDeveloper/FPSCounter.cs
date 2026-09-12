using UnityEngine;
using UnityEngine.UI;
public class FPSCounter : MonoBehaviour
{
    public Text Text;
    private float _time = 0;

    void Update()
    {
        float fps = 1f / Time.unscaledDeltaTime;
        _time += Time.deltaTime;
        if(_time >= 0.3)
        {
           Text.text = (fps.ToString("F0") + " FPS"); 
           _time = 0;
        }
        
    }
}