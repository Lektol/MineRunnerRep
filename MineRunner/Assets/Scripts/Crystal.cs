using UnityEngine;

public class Crystal : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            EventManager.OnGetCrystalInvoke();
            Destroy(gameObject);
        }
    }
}
