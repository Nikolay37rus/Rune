using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusShield : MonoBehaviour
{
    [SerializeField]
    private Transform transformObject;

    [SerializeField]
    private float _speed = 1;

    private void Update()
    {
        transform.Rotate(Vector3.back * _speed * Time.deltaTime);
    }

}
