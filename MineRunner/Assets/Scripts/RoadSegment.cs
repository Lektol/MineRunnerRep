using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    [SerializeField] private Transform _entry;
    [SerializeField] private Transform _exit;

    public Transform Entry => _entry;
    public Transform Exit => _exit;

    public void AlignTo(Transform target)
    {
        transform.position += target.position - _entry.position;
    }
}
