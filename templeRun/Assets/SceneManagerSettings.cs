using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SceneManagerSettings : MonoBehaviour
{
    public Toggle toogleSecimi;



    void Start()
    {
        toogleSecimi.GetComponent<Toggle>();
    }

    public void geriDonmeFonksiyonu()
    {
        SceneManager.LoadScene(0);
    }
    
    public void kaydetFonksiyonu()
    {

    }

    public void tuslarlaOyna(bool deger)
    {
        bool toogleDegeri = toogleSecimi.isOn;


        if(deger)
        {
            Debug.Log("Tuþlarla oynanýcak");
        }
        else
        {
            Debug.Log("Tuþlarla oynanmicak");
        }
    }

}
