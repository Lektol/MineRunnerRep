using System.Collections;
using UnityEngine;

public class PlayerLifeCycle : MonoBehaviour
{
    [Header("Death")]
    [SerializeField] private float _jumpDeadPower = 25f;

    [Header("Invincibility")]
    [SerializeField] private float _secToInvincible = 5f;

    [Header("Start position")]
    [SerializeField] private Vector3 _startPosition = Vector3.zero;
    [SerializeField] private Vector3 _startRotation = Vector3.zero;

    private Rigidbody _rb;

    private PlayerMovement _movement;
    private PlayerView _view;
    private PlayerCollision _collision;

    private Coroutine _invincibleCoroutine;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _movement = GetComponent<PlayerMovement>();
        _view = GetComponent<PlayerView>();
        _collision = GetComponent<PlayerCollision>();
    }

    private void Start()
    {
        ResetToMenu();
    }

    public void StartGame()
    {
        if (_movement.IsDead())
            ResetToMenu();

        _movement.StartMovement();
        _view.SetWheelsRotating(true);
        _view.PlayStartGame();
    }

    public void Die()
    {
        if (_movement.IsDead())
            return;

        _movement.SetDead();
        _view.SetWheelsRotating(false);
        _view.PlayDeath();

        _rb.isKinematic = false;
        _rb.useGravity = true;
        _rb.constraints = RigidbodyConstraints.None;

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        float directionZ =
            GetCurrentDirectionZ();

        Vector3 force =
            new Vector3(-0.35f, 1f, directionZ) *
            _jumpDeadPower;

        _rb.AddForce(force, ForceMode.Impulse);
    }

    public void Rebirth()
    {
        ResetToMenu();
        StartGame();

        StartInvincibility();
    }

    public void ResetToMenu()
    {
        StopInvincibility();

        _collision.IsInvincible = false;

        _movement.ResetMovement(_startPosition);

        _rb.isKinematic = true;
        _rb.useGravity = false;

        _rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY |
            RigidbodyConstraints.FreezeRotationZ;

        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        transform.position = _startPosition;
        transform.rotation = Quaternion.Euler(_startRotation);

        _view.SetWheelsRotating(false);
        _view.PlayRestartGame();
    }

    private void StartInvincibility()
    {
        StopInvincibility();

        _invincibleCoroutine =
            StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        _collision.IsInvincible = true;

        yield return new WaitForSeconds(_secToInvincible);

        _collision.IsInvincible = false;
        _invincibleCoroutine = null;
    }

    private void StopInvincibility()
    {
        if (_invincibleCoroutine == null)
            return;

        StopCoroutine(_invincibleCoroutine);
        _invincibleCoroutine = null;
    }

    private float GetCurrentDirectionZ()
    {
        // Средняя полоса и нижняя/верхняя полосы.
        // Оставляем ту же логику, что была в старом PlayerController.
        return transform.position.z <= 0f ? 1f : -1f;
    }

    private void OnDisable()
    {
        StopInvincibility();
    }
}