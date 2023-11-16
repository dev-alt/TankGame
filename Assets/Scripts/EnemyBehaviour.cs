using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    GameManager _gameManager;
    GameObject _player;

    private float _enemyHealth = 100f;
    private float _enemyMoveSpeed = 2f;
    private Quaternion _targetRotation;
    private bool _disableEnemy = false;
    private Vector2 _moveDirection;


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameManager._gameOver && !_disableEnemy)
        {
            MoveEnemy();
            RotateEnemy();
        }
    }
    private void MoveEnemy()
    {
        transform.position = Vector2.MoveTowards(transform.position,
            _player.transform.position, _enemyMoveSpeed * Time.deltaTime);
    }
    private void RotateEnemy()
    {
        _moveDirection = (_player.transform.position - transform.position).normalized;
        _targetRotation = Quaternion.LookRotation(Vector3.forward, _moveDirection);

        if (transform.rotation != _targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, 200 * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            StartCoroutine(Damaged());
            _enemyHealth -= 40f;
            if (_enemyHealth <= 0f)
            {
                Destroy(gameObject);
            }
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            _gameManager._gameOver = true;
            collision.gameObject.SetActive(false);
        }
    }

    IEnumerator Damaged()
    {
        _disableEnemy = true;
        yield return new WaitForSeconds(0.5f);
        _disableEnemy = false;
    }
}
