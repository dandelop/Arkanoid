using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    //Para enlazar los textos y los botones desde unity
    [SerializeField]
    private TextMeshProUGUI textPlayerLifes;
    [SerializeField]
    private TextMeshProUGUI textPlayerPoints;
    [SerializeField]
    private TextMeshProUGUI textPlayerMaxPoints;
    [SerializeField]
    private GameObject textGameOver;
    [SerializeField]
    private GameObject textVictory;
    [SerializeField]
    private GameObject buttonAgain;

    private void Awake()
    {
        GameManager.Instance.HUDManager = this;
    }

    internal void setPlayerLifes(int Lifes)
    {   //Setter para el conteo de vidas
        textPlayerLifes.text = "Lifes: " + Lifes.ToString();
    }
    internal void setPlayerPoints(int Points)
    {   //Setter para el conteo de puntos
        textPlayerPoints.text = "Points: " + Points.ToString() + " " + "x" + GameManager.Multiplier;
    }
    internal void setPlayerMaxPoints(int Points)
    {   //Setter para el conteo de puntuación máxima
        textPlayerMaxPoints.text = "Record: " + Points.ToString();
    }
    internal void GameOver() //Pantalla de Game Over
    {   //Activo el texto de Game Over y el botón para jugar otra vez
        textGameOver.SetActive(true);
        buttonAgain.SetActive(true);
    }
    internal void Victory() //Pantalla de Victoria
    {   //Activo el texto de victoria y el botón para jugar otra vez
        textVictory.SetActive(true);
        buttonAgain.SetActive(true);
    }
    public void Again() //Botón para jugar otra partida
    {
        GameManager.Instance.Again();
    }
}
