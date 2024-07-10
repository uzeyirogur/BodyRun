using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject cubePrefab; // Oluþturulacak küp prefabý

    public float[] kupKoordinatlari = new float[3];
    public int baslangicMesafesi = -150;
    public float seritGenisligi;
    private float platformGenisligi;


    void Start()
    {
        platformGenisligi = platformPrefab.transform.localScale.x;
        seritGenisligi = platformGenisligi / 3;

        kupKoordinatlari[0] = seritGenisligi * (-1);
        kupKoordinatlari[1] = 0;
        kupKoordinatlari[2] = seritGenisligi * (1);

        TriggerDuvari triggerDuvariInstance = new TriggerDuvari();
        triggerDuvariInstance.CreateCubesBaslangic(platformPrefab, cubePrefab, baslangicMesafesi, kupKoordinatlari, 8);
    }

  
}