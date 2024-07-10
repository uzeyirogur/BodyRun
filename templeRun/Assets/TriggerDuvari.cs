using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDuvari : MonoBehaviour
{
    public GameObject platformPrefab; //Platform prefabý
    public GameObject cubePrefab; // Oluþturulacak küp prefabý
    public GameObject playerPrefab; // Oyuncu prefabý
    public GameObject triggerPrefab; //Trigger duvari prefabý

    public float[] kupKoordinatlari = new float[3]; // Küplerin x konumlarýnýn hangisinde bulunacaðý

    private float platformGenisligi = 30; // Üzerinde durulan platformun geniþliði
    int platformUzunlugu = 500;            //Platform Uzunlugu
    public float seritGenisligi;     // Bir þeridin ne kadar geniþ olduðu

    public int olusacakYanalSeritSayisi = 10; // Oluþturulacak yanal þerit sayýsý
    
    public float kuplerArasiMesafe = 30f; // Þeritler arasý mesafe


    public int aSabiti = 100; //Platfrom oluþtururken kullanilcak sabit (Elle ayarlýyoruz)

    public int kupX = 15; // Küp geniþliði
    public int kupY = 10; // Küp yüksekliði
    public int kupZ = 30; // Küp uzunluðu 

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
        if (other.CompareTag("Player")) // Eðer temas eden nesne Player tag'ýna sahipse
        {
            int triggerDuvariZDegeri = (int)triggerPrefab.transform.position.z;


            //Yeni platfrom nesnesi
            GameObject platform = new GameObject("yeniPlatform" + triggerDuvariZDegeri);

            // Mesh Filter bileþeni ekle
            MeshFilter meshFilter = platform.AddComponent<MeshFilter>();
            // Mesh Renderer bileþeni ekle
            MeshRenderer meshRenderer = platform.AddComponent<MeshRenderer>();
            // Box Collider bileþeni ekle
            BoxCollider boxCollider = platform.AddComponent<BoxCollider>();

            // Mesh Filter bileþenine cube þeklinde bir mesh atayýn
            meshFilter.mesh = cubePrefab.GetComponent<MeshFilter>().sharedMesh;

            // Mesh Renderer bileþenine default bir malzeme atayýn
            meshRenderer.material = new Material(Shader.Find("Standard"));

            // Box Collider'ýn boyutunu ayarla
            boxCollider.size = new Vector3(1, 1f, 1f);

            // Opsiyonel olarak, platforma bir default boyut atayabilirsiniz.
            platform.transform.localScale = new Vector3(30f, 0.5f, 500f);
            platform.transform.position = new Vector3(0f, 0f, ((triggerDuvariZDegeri + platformUzunlugu - aSabiti)));

            // TriggerDuvari nesnesini bul
            GameObject triggerDuvari = GameObject.FindGameObjectWithTag("Trigger");

            // Eðer TriggerDuvari bulunduysa ve platform oluþturulduysa
            if (triggerDuvari != null && platform != null)
            {
                // TriggerDuvari'nin kopyasýný al ve alt nesnesi yap
                GameObject triggerDuvariKopya = Instantiate(triggerDuvari, platform.transform.position, platform.transform.rotation, platform.transform);
                triggerDuvariKopya.name = triggerDuvari.name; // Kopyanýn adýný ayarla
                triggerDuvariKopya.transform.localScale = new Vector3(1f, 30f, 0.001f);
                triggerDuvariKopya.transform.position = new Vector3(0f, 7f, triggerDuvariZDegeri + platformUzunlugu);
            }

            CreateCubes(platform, cubePrefab, (triggerDuvariZDegeri + aSabiti + 65));
            platformManager.DestroyPlatformsBelowZ(triggerDuvariZDegeri);
        }
    }

    public void CreateCubes(GameObject yeniPlatform, GameObject kup, float baslangicMesafesi)
    {
        // Oyuncudan belirli bir mesafe kadar uzaklýkta baþlangýç
        float currentPositionZ = baslangicMesafesi;

        int kacKereTekEngelOldu = 0;

        // Her bir sýra için
        for (int i = 0; i < olusacakYanalSeritSayisi; i++)
        {
            // Rastgele sayýda engel oluþtur
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

            // Rastgele engel konumu seç
            int[] engelSeritleri = new int[engelSayisi];

            for (int j = 0; j < engelSayisi; j++)
            {
                int rastgeleKonum;

                do
                {
                    rastgeleKonum = Random.Range(1, 4); // 0, 1 veya 2 konumu
                } while (System.Array.IndexOf(engelSeritleri, rastgeleKonum) != -1); // Eðer rastgele konum daha önce kullanýldýysa tekrar oluþtur

                engelSeritleri[j] = rastgeleKonum - 1;
            }

            // Her bir konum için
            for (int k = 0; k < engelSayisi; k++)
            {
                float positionX = kupKoordinatlari[engelSeritleri[k]];

                // Küpü oluþtur
                GameObject cube = Instantiate(kup, new Vector3(positionX, 0f, currentPositionZ), Quaternion.identity);
                cube.transform.localScale = new Vector3(kupX, kupY, kupZ);
                cube.transform.parent = yeniPlatform.transform; // Küpü yeni platformun alt nesnesi yap

                
            }
            currentPositionZ += (kuplerArasiMesafe + 20); // Sýra arasý mesafe ekle
        }
    }

    public void CreateCubesBaslangic(GameObject yeniPlatform, GameObject kup, float baslangicMesafesi, float[] koordinatListesi, int baslangicYanalSerit)
    {
        // Oyuncudan belirli bir mesafe kadar uzaklýkta baþlangýç
        float currentPositionZ = baslangicMesafesi;

        int kacKereTekEngelOldu = 0;


        // Her bir sýra için
        for (int i = 0; i < baslangicYanalSerit; i++)
        {
            // Rastgele sayýda engel oluþtur
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

            // Rastgele engel konumu seç
            int[] engelSeritleri = new int[engelSayisi];

            for (int j = 0; j < engelSayisi; j++)
            {
                int rastgeleKonum;

                do
                {
                    rastgeleKonum = Random.Range(1, 4); // 0, 1 veya 2 konumu
                } while (System.Array.IndexOf(engelSeritleri, rastgeleKonum) != -1); // Eðer rastgele konum daha önce kullanýldýysa tekrar oluþtur

                engelSeritleri[j] = rastgeleKonum - 1;
            }

            // Her bir konum için
            for (int k = 0; k < engelSayisi; k++)
            {
                float positionX = koordinatListesi[engelSeritleri[k]];

                // Küpü oluþtur
                GameObject cube = Instantiate(kup, new Vector3(positionX, 0f, currentPositionZ), Quaternion.identity);
                cube.transform.localScale = new Vector3(7, 10, 30);
                cube.transform.parent = yeniPlatform.transform; // Küpü yeni platformun alt nesnesi yap


            }
            currentPositionZ += (kuplerArasiMesafe + 20); // Sýra arasý mesafe ekle
        }
    }
}
