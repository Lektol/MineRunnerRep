using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private GameObject _sphereInvincible;
    [SerializeField] private Transform[] _wheels;

    private Animator _animator;

    private bool _isWheelsRotating;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        RotateWheels();
    }

    public void SetInvincible(bool value)
    {
        if (_sphereInvincible != null)
            _sphereInvincible.SetActive(value);
    }

    public void SetDown(bool value)
    {
        if (_animator != null)
            _animator.SetBool("IsDown", value);
    }

    public void PlayStartGame()
    {
        if (_animator != null)
            _animator.SetTrigger("StartGame");
    }

    public void PlayDeath()
    {
        if (_animator != null)
            _animator.SetTrigger("Dead");
    }

    public void PlayRestartGame()
    {
        if (_animator != null)
            _animator.SetTrigger("RestartGame");
    }

    public void SetWheelsRotating(bool value)
    {
        _isWheelsRotating = value;
    }

    private void RotateWheels()
    {
        if (!_isWheelsRotating)
            return;

        if (_wheels == null || _wheels.Length == 0)
            return;

        if (RoadGenerator.Instance == null)
            return;

        float rotationSpeed = RoadGenerator.Instance.MaxSpeed * -10f;

        foreach (Transform wheel in _wheels)
        {
            if (wheel == null) continue;

            wheel.Rotate(
                0f,
                0f,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}