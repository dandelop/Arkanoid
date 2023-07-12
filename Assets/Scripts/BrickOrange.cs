using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickOrange : MonoBehaviour
{
    //Color para identificar los puntos en el GameManager
    private string _color = "Orange";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.AddPoints(this._color); //Le paso al GameManager el color para la suma de puntos   
        this.gameObject.SetActive(false); //Elimino el bloque después de la colisión   
    }
}
