using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    
    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)Vector2.left * (Time.deltaTime * _speed);
    }
}
