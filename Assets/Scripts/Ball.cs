using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.WSA;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Ball : MonoBehaviour
{
    [SerializeField] private float velocity;
    [SerializeField] private float _incrementVelocity;
    [SerializeField] private float maxVelocity;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _velocityPrev;      

    //Ángulos de giro según donde golpee la pala (basado en la información de StrategyWiki)
    private const float _effectMiddleAngle = 10;
    private const float _effectSideAngle = 33;
    private const float _effectCornerAngle = 65;

    //Control de saque
    public bool canLaunch = true;
    private void Awake()
    {
        GameManager.Instance.Ball = this;
    }

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>(); 
    }

    public void Launch()
    {   //Lanza la bola hacia arriba desde la pala
        canLaunch = false;
        _rigidbody2D.velocity = velocity * (new Vector2(0, 1)); 
    }

    private void Update()
    {   //Saque de bola
        if (canLaunch == true)
        {   //Alineación de la bola con la pala
            transform.position = new Vector2(GameObject.FindGameObjectWithTag("Player").transform.position.x, -4);
            if (Input.GetKey(KeyCode.Space))
            {
                Launch();
            }
        }
    }

    private void FixedUpdate()
    {   //Limitador de velocidad de la bola
        if (_rigidbody2D.velocity.magnitude >= maxVelocity)
        {
            _rigidbody2D.velocity = _rigidbody2D.velocity.normalized * maxVelocity;
        }
        //Velocidad previa al choque para los cálculos de rebotes
        _velocityPrev = _rigidbody2D.velocity;
    }

    private Vector2 Accelerate(Vector2 velocity)  //Aceleración (aumenta la velocidad al chocar con la pala)
    {
        return _incrementVelocity * velocity.normalized;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //Cojo la última velocidad previa al choque y le sumo la aceleración (solo acelera al chocar con la pala)
            _rigidbody2D.velocity = _velocityPrev + Accelerate(_velocityPrev);
            //Cambio de dirección al rebotar
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -_rigidbody2D.velocity.y);
            //Cada vez que choca contra la pala, el multiplicador vuelve a x1
            GameManager.Multiplier = 1;

            //Gestor de colisiones
            switch (col.collider.sharedMaterial.name)
            {   //Golpe en medio de la pala (aplica un cambio de 10º)
                case "PaddleM":
                    if (_rigidbody2D.velocity.x > 0)
                    {   
                        _rigidbody2D.velocity = Quaternion.AngleAxis(-_effectMiddleAngle, Vector3.forward) * _rigidbody2D.velocity;
                    }
                    else
                    {
                        _rigidbody2D.velocity = Quaternion.AngleAxis(_effectMiddleAngle, Vector3.forward) * _rigidbody2D.velocity;
                    }
                    break;
                //Golpe a la derecha de la pala (aplica un cambio de 33º)
                case "PaddleR":
                    _rigidbody2D.velocity = Quaternion.AngleAxis(-_effectSideAngle, Vector3.forward) * _rigidbody2D.velocity;
                    break;
                //Golpe a la izquierda de la pala (aplica un cambio de 33º)
                case "PaddleL":
                    _rigidbody2D.velocity = Quaternion.AngleAxis(_effectSideAngle, Vector3.forward) * _rigidbody2D.velocity;
                    break;
                //Golpe en el lateral derecho de la pala (aplica un cambio de 65º)
                case "PaddleCornerR":
                    _rigidbody2D.velocity = Quaternion.AngleAxis(-_effectCornerAngle, Vector3.forward) * _rigidbody2D.velocity;
                    break;
                //Golpe en el lateral izquierdo de la pala (aplica un cambio de 65º)
                case "PaddleCornerL":
                    _rigidbody2D.velocity = Quaternion.AngleAxis(_effectCornerAngle, Vector3.forward) * _rigidbody2D.velocity;
                    break;
            }
        }       
        if (col.gameObject.CompareTag("Wall"))  //Rebote contra el muro
        {
            _rigidbody2D.velocity = _velocityPrev;
            _rigidbody2D.velocity = new Vector2(-_rigidbody2D.velocity.x, _rigidbody2D.velocity.y);
        }
        if (col.gameObject.CompareTag("Roof"))  //Rebote contra el techo
        {
            _rigidbody2D.velocity = _velocityPrev;
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -_rigidbody2D.velocity.y);
        }
        if (col.gameObject.CompareTag("Brick")) //Rebote contra los ladrillos
        {
            Vector3 _direcciónInicial = _velocityPrev;  //Vector original de la pelota
            Vector3 _normal = col.GetContact(0).normal;  //Obtengo la normal del punto de contacto con el ladrillo
            _rigidbody2D.velocity = Vector3.Reflect(_direcciónInicial, _normal);  //El nuevo vector es el reflejo definido por la normal
        }
    }
}
