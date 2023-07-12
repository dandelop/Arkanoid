using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        //Obtengo el componente para usarlo después
        _rigidbody2D = GetComponent<Rigidbody2D>(); 
    }

    private void FixedUpdate()
    {
        //Input de los controles y limitación de movimiento
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) && transform.position.x >= -7)
        {   
                _rigidbody2D.velocity = (speed * Vector3.left);       
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) && transform.position.x <= 7)
        {
                _rigidbody2D.velocity = (speed * Vector3.right);       
        }
        else
        {
            _rigidbody2D.velocity = new Vector3(0, _rigidbody2D.velocity.y, 0);
        }
    }
}
