using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    public GameObject pickUpText;
    public GameObject useMaskText;
    public GameObject Shrine;
    public GameObject Mask;

    bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            triggered = true;
            pickUpText.SetActive(true);
            StartCoroutine(pickUpMessage());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        triggered = false;
    }

    IEnumerator pickUpMessage()
    {
        yield return new WaitForSeconds(3);
        pickUpText.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (triggered == true)
            {
                pickUpText.SetActive(false);
                Mask.SetActive(false);
                useMaskText.SetActive(true);
                StartCoroutine(destroySelf());
            }
        }
    }

    IEnumerator destroySelf()
    {
        yield return new WaitForSeconds(3);
        Shrine.SetActive(true);
        useMaskText.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
