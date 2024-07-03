using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDetection : MonoBehaviour
{
    public Vector3 PlayerPosition => _playerPos;
    public UnityEvent OnPlayerDetected;
    public UnityEvent OnPlayerLeft;
    private Vector3 _playerPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnPlayerDetected?.Invoke();
        _playerPos = collision.transform.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _playerPos = Vector3.zero;
        OnPlayerLeft?.Invoke();
    }
}
