using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        _rigid.velocity = new Vector2(x, y).normalized * _speed;
    }
}
