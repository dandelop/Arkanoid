using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int _lifesPlayer = 3;           //Cantidad de bolas
    private static int _pointsPlayer = 0;   //Puntos del jugador
    private static int _pointsMaxPlayer;    //Máxima puntuación del jugador
    private HUDManager _HUDManager;

    public static int Multiplier = 1;      //Multiplicador de puntos

    public HUDManager HUDManager
    {
        get => _HUDManager;
        set => _HUDManager =  value;
    }

    private Ball _ball;
    public Ball Ball
    {
        get => _ball;
        set => _ball = value;
    }
    public void Awake()
    {
        Time.timeScale = 1f;    //Para que el juego se reanude después de recargar la escena

        if (_instance != null)
        {

            if (_instance != this)
            {
                Destroy(this);
            }

            else
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Update()
    {   //Actualiza el texto de los puntos y la vida del jugador
        _HUDManager.setPlayerPoints(_pointsPlayer);
        _HUDManager.setPlayerMaxPoints(_pointsMaxPlayer);
        _HUDManager.setPlayerLifes(_lifesPlayer);

        //Cuando se eliminan todos los ladrillos activo la pantalla de victoria y paro el juego
        if (GameObject.FindGameObjectsWithTag("Brick").Length == 0)
        {  
            _HUDManager.Victory();
            Time.timeScale = 0f;
        }
    }

    public void LostBall()
    {
        //Cuando la bola se cae se resta una vida
        _lifesPlayer--;
        //Si las vidas llegan a cero se activa la pantalla de game over y se para el juego
        if (_lifesPlayer <= 0)
        {
            _HUDManager.GameOver();
            Time.timeScale = 0f;
        }
        else
        {   //Si aún le quedan vidas puede sacar otra vez
            Ball.canLaunch = true;
        }
    }

    public void Again() //Botón para jugar otra vez
    {
        //Recargo la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Establezco los parametros a sus valores iniciales menos el record de puntuación
        _lifesPlayer = 3;                                           
        _pointsPlayer = 0;
    }

    public static void AddPoints(String color)  //Sistema de puntos
    {     
        switch (color)  //Según el color del ladrillo destruido añade más o menos puntos
        {
            case "Red":
                _pointsPlayer += 10 * Multiplier;
                break;
            case "Orange":
                _pointsPlayer += 20 * Multiplier;
                break;
            case "Yellow":
                _pointsPlayer += 30 * Multiplier;
                break;
            case "Green":
                _pointsPlayer += 40 * Multiplier;
                break;
            case "Blue":
                _pointsPlayer += 50 * Multiplier;
                break;
        }
        if (_pointsPlayer > _pointsMaxPlayer)   //Para llevar la cuenta de la máxima puntuación
        {
            _pointsMaxPlayer = _pointsPlayer;
        }
        Multiplier++;  //Por cada ladrillo consecutivo destruido sin rebotar en la pala añade 1 al multiplicador
    }

    //Singleton
    private static GameManager _instance = null;

    public static GameManager Instance
    {
        get => _instance;
    }

    private GameManager()
    {
        if (_instance==null)
        {
            _instance = this;
        }
    }  
}