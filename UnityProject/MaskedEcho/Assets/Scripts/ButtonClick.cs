using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    // Das GameObject, das deaktiviert werden soll
    public GameObject objectToDeactivate;

    // Diese Methode wird vom Button aufgerufen
    public void DeactivateObject()
    {
        if (objectToDeactivate != null)
        {
            objectToDeactivate.SetActive(false);
            Debug.Log(objectToDeactivate.name + " wurde deaktiviert!");
        }
        else
        {
            Debug.LogWarning("Kein GameObject zugewiesen!");
        }
    }

}
