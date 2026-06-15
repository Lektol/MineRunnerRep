using UnityEngine;
using UnityEngine.UI;
public class FPSCounter : MonoBehaviour
{
    public Text Text;
    private float time = 0;

    void Update()
    {
        float fps = 1f / Time.unscaledDeltaTime;
        time += Time.deltaTime;
        if(time >= 0.3)
        {
           Text.text = (fps.ToString("F0") + " FPS"); 
           time = 0;
        }
        
    }
}