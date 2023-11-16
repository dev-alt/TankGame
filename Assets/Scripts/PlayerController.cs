using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;

    private Rigidbody2D _rb;
    private Camera _mainCamera;

    private float _moveVertical;
    private float _moveHorizontal;
    private float _moveSpeed = 5f;
    private float _speedLimiter = 0.7f;
    private Vector2 _moveVelocity;

    private Vector2 _mousePos;
    private Vector2 _offset;

    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _bulletSpawn;

    private bool _isShooting = false;
    private float _bulletSpeed = 15f;


    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
    }

    void Update()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveVertical = Input.GetAxisRaw("Vertical");

        _moveVelocity = new Vector2(_moveHorizontal, _moveVertical) * _moveSpeed;

        if (Input.GetMouseButtonDown(0))
        {
            _isShooting = true;
        }

    }

    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
        if (_isShooting)
        {
            StartCoroutine(Fire());
        }
    }

    IEnumerator Fire()
    {
        _isShooting = false;
        GameObject bullet = Instantiate(_bullet, _bulletSpawn.transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = _offset * _bulletSpeed;

        yield return new WaitForSeconds(3);
        Destroy(bullet);
    }

    private void RotatePlayer()
    {
        _mousePos = Input.mousePosition;
        Vector3 screenPoint = _mainCamera.WorldToScreenPoint(transform.localPosition);
        _offset = new Vector2(_mousePos.x - screenPoint.x, _mousePos.y - screenPoint.y).normalized;

        float angle = Mathf.Atan2(_offset.y, _offset.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void MovePlayer()
    {
        if (_moveHorizontal != 0 || _moveVertical != 0)
        {
            if (_moveHorizontal != 0 && _moveVertical != 0)
            {
                _moveVelocity *= _speedLimiter;
            }
            _rb.velocity = _moveVelocity;
        }
        else
        {
            _moveVelocity = new Vector2(0f, 0f);
            _rb.velocity = _moveVelocity;
        }
    }
}
