using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickYellow : MonoBehaviour
{
    //Color para identificar los puntos en el GameManager
    private string _color = "Yellow";

    //Los bloques amarillos y verdes aguantan 2 golpes
    private int _vidas = 2;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _vidas--;   //Resto vidas cuando les golpea la pelota
        if (_vidas <= 0) 
        {
            GameManager.AddPoints(this._color); //Le paso al GameManager el color para la suma de puntos   
            this.gameObject.SetActive(false); //Elimino el bloque después de la colisión 
        }
    }
}
