using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public GameObject Platform;
    private float platformGenisligi;

    public float karakterHizi = 40f; // Karakterin hareket h�z�
    public float ziplamaYuksekligi = 10f;
    public float dususHizi = 520f; // Karakterin d���� h�z�
    public float seritGenisligi; // �eritler aras� mesafe

    public float[] hareketKoordinatlari = new float[3];

    private int mevcutSerit = 1; // Ba�lang��ta orta �eritte ba�lar
    private bool isMoving = false;
    private Rigidbody rb; // Rigidbody de�i�keni

    private bool isJumping = false; // Z�plama i�lemi kontrol�
    public UDPReceive uDPReceive; //Udp transferi i�in gerekli nesne

    private string yollananVeri;
    private float yonBilgisi;

    public bool oyunDurumu = true;
    public bool tuslarlaOyna = true;


    void Start()
    {
        platformGenisligi = Platform.transform.localScale.x;
        seritGenisligi = platformGenisligi / 3;
        hareketKoordinatlari[0] = seritGenisligi * (-1);
        hareketKoordinatlari[1] = 0;
        hareketKoordinatlari[2] = seritGenisligi * (1);

        rb = gameObject.AddComponent<Rigidbody>(); // Rigidbody bile�enini ekleyin  
    }

    void Update()
    {

        if(oyunDurumu)
        {
            //Karekterin s�rekli olarak ileri hareket etmesi

            transform.Translate(Vector3.forward * karakterHizi * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);


            //Yon tuslariyla oynamak istendigi zaman;
            if(tuslarlaOyna)
            {
                // Karakterin hareketi sadece �u an hareket etmiyorsa ger�ekle�ir
                if (!isMoving)
                {
                    //UDP haberle�mesini kapat�yor
                    uDPReceive.startRecieving = false;

                    if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        ChangeLaneTus(1); // Sa�a git
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        ChangeLaneTus(-1); // Sola git
                    }
                    else if (Input.GetKeyDown(KeyCode.Space) && rb.velocity.y == 0f) // Yere de�di�inde ve z�plamaya haz�r oldu�unda z�plama i�lemini yap
                    {
                        Jump();
                    }

                    // Karakter z�pl�yorsa
                    if (isJumping)
                    {
                        // D���� h�z�n� kontrol et ve d���� h�z�n� sabit tut
                        if (rb.velocity.y < 0)
                        {
                            rb.velocity += Vector3.down * dususHizi * Time.deltaTime;
                            isJumping = false;
                        }
                    }
                }
            }

            //Beden hareketleriyle oynanmak istendigi zaman
            else if (!tuslarlaOyna)
            {
                uDPReceive.startRecieving = true;
                //Udp ile yollanan verileri alma
                try
                {
                    yollananVeri = uDPReceive.data;

                    yonBilgisi = float.Parse(yollananVeri);
                }
                catch (Exception err)
                {
                    Debug.Log("Python verisi bulunamadi");
                    Debug.Log(err.ToString());
                    yonBilgisi = -1;
                }

                if(yonBilgisi == 3)
                {
                    ChangeLaneVucut(2);
                }
                else if(yonBilgisi == 2)
                {
                    ChangeLaneVucut(1);
                }
                else if(yonBilgisi == 1)
                {
                    ChangeLaneVucut(0);
                }

            }

        }
        else
        {
            karakterHizi = 0;
            uDPReceive.client.Close();
            uDPReceive.startRecieving = false;
            Durdur();
        }


    }

    void ChangeLaneTus(int direction)
    {
        int targetLane = mevcutSerit + direction;

        // Hedef �erit s�n�rlar�n�n kontrol�
        if (targetLane < 0 || targetLane > 2)
        {
            return; // Hedef �erit s�n�rlar�n d���nda ise i�lemi sonland�r
        }

        // Hedef konumun belirlenmesi
        float targetX = hareketKoordinatlari[targetLane];

        // Karakterin hedef konuma hareket etmesi
        transform.DOMoveX(targetX, 0.2f).SetEase(Ease.OutQuad).OnStart(() =>
        {
            //isMoving = true;
        }).OnComplete(() =>
        {
            //isMoving = false;
            mevcutSerit = targetLane; // �erit g�ncellemesi
        });
    }

    void ChangeLaneVucut(int direction)
    {
        int targetLane = direction;

        // Hedef �erit s�n�rlar�n�n kontrol�
        if (targetLane < 0 || targetLane > 2)
        {
            return; // Hedef �erit s�n�rlar�n d���nda ise i�lemi sonland�r
        }

        // Hedef konumun belirlenmesi
        float targetX = hareketKoordinatlari[targetLane];

        // Karakterin hedef konuma hareket etmesi
        transform.DOMoveX(targetX, 0.2f).SetEase(Ease.OutQuad).OnStart(() =>
        {
            //isMoving = true;
        }).OnComplete(() =>
        {
            //isMoving = false;
            mevcutSerit = targetLane; // �erit g�ncellemesi
        });
    }

    void Jump()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Yalpalanmay� engelle

        //Z�plama y�ksekligi
        rb.AddForce(Vector3.up * ziplamaYuksekligi, ForceMode.Impulse);

        isJumping = true; // Z�plama i�lemi ba�lad�
    }

    public void Durdur()
    {
        // Karakterin t�m y�nlerdeki h�zlar�n� s�f�rla
        rb.velocity = Vector3.zero;

        // Karakterin t�m y�nlerde hareketini engellemek i�in RigidbodyConstraints.FreezeAll flag'i aktif edilir.
        rb.constraints = RigidbodyConstraints.FreezeAll;

    }
}
