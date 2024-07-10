using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDuvari : MonoBehaviour
{
    public GameObject platformPrefab; //Platform prefab�
    public GameObject cubePrefab; // Olu�turulacak k�p prefab�
    public GameObject playerPrefab; // Oyuncu prefab�
    public GameObject triggerPrefab; //Trigger duvari prefab�

    public float[] kupKoordinatlari = new float[3]; // K�plerin x konumlar�n�n hangisinde bulunaca��

    private float platformGenisligi = 30; // �zerinde durulan platformun geni�li�i
    int platformUzunlugu = 500;            //Platform Uzunlugu
    public float seritGenisligi;     // Bir �eridin ne kadar geni� oldu�u

    public int olusacakYanalSeritSayisi = 10; // Olu�turulacak yanal �erit say�s�
    
    public float kuplerArasiMesafe = 30f; // �eritler aras� mesafe


    public int aSabiti = 100; //Platfrom olu�tururken kullanilcak sabit (Elle ayarl�yoruz)

    public int kupX = 15; // K�p geni�li�i
    public int kupY = 10; // K�p y�ksekli�i
    public int kupZ = 30; // K�p uzunlu�u 

    PlatformManager platformManager = new PlatformManager();

    public void Start()
    {
        //platformGenisligi = platformPrefab.transform.localScale.x;
        //platformUzunlugu = (int)platformPrefab.transform.localScale.z;

        seritGenisligi = platformGenisligi / 3;

        kupKoordinatlari[0] = seritGenisligi * (-1);
        kupKoordinatlari[1] = 0;
        kupKoordinatlari[2] = seritGenisligi * (1);

        kupX = (int)(seritGenisligi - 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // E�er temas eden nesne Player tag'�na sahipse
        {
            int triggerDuvariZDegeri = (int)triggerPrefab.transform.position.z;


            //Yeni platfrom nesnesi
            GameObject platform = new GameObject("yeniPlatform" + triggerDuvariZDegeri);

            // Mesh Filter bile�eni ekle
            MeshFilter meshFilter = platform.AddComponent<MeshFilter>();
            // Mesh Renderer bile�eni ekle
            MeshRenderer meshRenderer = platform.AddComponent<MeshRenderer>();
            // Box Collider bile�eni ekle
            BoxCollider boxCollider = platform.AddComponent<BoxCollider>();

            // Mesh Filter bile�enine cube �eklinde bir mesh atay�n
            meshFilter.mesh = cubePrefab.GetComponent<MeshFilter>().sharedMesh;

            // Mesh Renderer bile�enine default bir malzeme atay�n
            meshRenderer.material = new Material(Shader.Find("Standard"));

            // Box Collider'�n boyutunu ayarla
            boxCollider.size = new Vector3(1, 1f, 1f);

            // Opsiyonel olarak, platforma bir default boyut atayabilirsiniz.
            platform.transform.localScale = new Vector3(30f, 0.5f, 500f);
            platform.transform.position = new Vector3(0f, 0f, ((triggerDuvariZDegeri + platformUzunlugu - aSabiti)));

            // TriggerDuvari nesnesini bul
            GameObject triggerDuvari = GameObject.FindGameObjectWithTag("Trigger");

            // E�er TriggerDuvari bulunduysa ve platform olu�turulduysa
            if (triggerDuvari != null && platform != null)
            {
                // TriggerDuvari'nin kopyas�n� al ve alt nesnesi yap
                GameObject triggerDuvariKopya = Instantiate(triggerDuvari, platform.transform.position, platform.transform.rotation, platform.transform);
                triggerDuvariKopya.name = triggerDuvari.name; // Kopyan�n ad�n� ayarla
                triggerDuvariKopya.transform.localScale = new Vector3(1f, 30f, 0.001f);
                triggerDuvariKopya.transform.position = new Vector3(0f, 7f, triggerDuvariZDegeri + platformUzunlugu);
            }

            CreateCubes(platform, cubePrefab, (triggerDuvariZDegeri + aSabiti + 65));
            platformManager.DestroyPlatformsBelowZ(triggerDuvariZDegeri);
        }
    }

    public void CreateCubes(GameObject yeniPlatform, GameObject kup, float baslangicMesafesi)
    {
        // Oyuncudan belirli bir mesafe kadar uzakl�kta ba�lang��
        float currentPositionZ = baslangicMesafesi;

        int kacKereTekEngelOldu = 0;

        // Her bir s�ra i�in
        for (int i = 0; i < olusacakYanalSeritSayisi; i++)
        {
            // Rastgele say�da engel olu�tur
            int engelSayisi = Random.Range(1, 3); // 1 veya 2 engel

            if (engelSayisi == 1)
            {
                kacKereTekEngelOldu++;
            }

            if (kacKereTekEngelOldu == 3)
            {
                engelSayisi = 2;
                kacKereTekEngelOldu = 0;
            }

            // Rastgele engel konumu se�
            int[] engelSeritleri = new int[engelSayisi];

            for (int j = 0; j < engelSayisi; j++)
            {
                int rastgeleKonum;

                do
                {
                    rastgeleKonum = Random.Range(1, 4); // 0, 1 veya 2 konumu
                } while (System.Array.IndexOf(engelSeritleri, rastgeleKonum) != -1); // E�er rastgele konum daha �nce kullan�ld�ysa tekrar olu�tur

                engelSeritleri[j] = rastgeleKonum - 1;
            }

            // Her bir konum i�in
            for (int k = 0; k < engelSayisi; k++)
            {
                float positionX = kupKoordinatlari[engelSeritleri[k]];

                // K�p� olu�tur
                GameObject cube = Instantiate(kup, new Vector3(positionX, 0f, currentPositionZ), Quaternion.identity);
                cube.transform.localScale = new Vector3(kupX, kupY, kupZ);
                cube.transform.parent = yeniPlatform.transform; // K�p� yeni platformun alt nesnesi yap

                
            }
            currentPositionZ += (kuplerArasiMesafe + 20); // S�ra aras� mesafe ekle
        }
    }

    public void CreateCubesBaslangic(GameObject yeniPlatform, GameObject kup, float baslangicMesafesi, float[] koordinatListesi, int baslangicYanalSerit)
    {
        // Oyuncudan belirli bir mesafe kadar uzakl�kta ba�lang��
        float currentPositionZ = baslangicMesafesi;

        int kacKereTekEngelOldu = 0;


        // Her bir s�ra i�in
        for (int i = 0; i < baslangicYanalSerit; i++)
        {
            // Rastgele say�da engel olu�tur
            int engelSayisi = Random.Range(1, 3); // 1 veya 2 engel

            if (engelSayisi == 1)
            {
                kacKereTekEngelOldu++;
            }

            if (kacKereTekEngelOldu == 3)
            {
                engelSayisi = 2;
                kacKereTekEngelOldu = 0;
            }

            // Rastgele engel konumu se�
            int[] engelSeritleri = new int[engelSayisi];

            for (int j = 0; j < engelSayisi; j++)
            {
                int rastgeleKonum;

                do
                {
                    rastgeleKonum = Random.Range(1, 4); // 0, 1 veya 2 konumu
                } while (System.Array.IndexOf(engelSeritleri, rastgeleKonum) != -1); // E�er rastgele konum daha �nce kullan�ld�ysa tekrar olu�tur

                engelSeritleri[j] = rastgeleKonum - 1;
            }

            // Her bir konum i�in
            for (int k = 0; k < engelSayisi; k++)
            {
                float positionX = koordinatListesi[engelSeritleri[k]];

                // K�p� olu�tur
                GameObject cube = Instantiate(kup, new Vector3(positionX, 0f, currentPositionZ), Quaternion.identity);
                cube.transform.localScale = new Vector3(7, 10, 30);
                cube.transform.parent = yeniPlatform.transform; // K�p� yeni platformun alt nesnesi yap


            }
            currentPositionZ += (kuplerArasiMesafe + 20); // S�ra aras� mesafe ekle
        }
    }
}
