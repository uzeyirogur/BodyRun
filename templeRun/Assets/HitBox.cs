using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    UIManager UIManagerScript;
    PlayerController playerController;

    private void OnTriggerEnter(Collider other)
    {
        UIManagerScript = GameObject.Find("UI Manager").GetComponent<UIManager>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();


        if (other.CompareTag("Engel"))
        {
            Debug.Log("Engele Carpti");
            UIManagerScript.GetComponent<Canvas>().enabled = true;
            playerController.oyunDurumu = false;
        }
    }
}
