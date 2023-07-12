using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLine : MonoBehaviour
{
    public GameObject ball;

    private void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
    }
    private void OnTriggerEnter2D()
    {
        GameManager.Instance.LostBall();    //Avisa al GameManager para gestionar la pérdida de la pelota
    }
}
