using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    // Silinecek platformlar�n ba�lang�� ad�
    public string platformNamePrefix = "yeniPlatform";

    // Kullan�c�n�n z de�erinden k���k olan platformlar� silen fonksiyon
    public void DestroyPlatformsBelowZ(float zThreshold)
    {
        // Sahnedeki t�m objeleri al
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        // Silinecek platformlar�n listesi
        List<GameObject> platformsToDelete = new List<GameObject>();

        // T�m objeleri d�n
        foreach (GameObject obj in allObjects)
        {
            // Objenin ad� platformNamePrefix ile ba�l�yorsa ve objenin pozisyonunun z bile�eni zThreshold'den k���kse
            if (obj.name.StartsWith(platformNamePrefix) && (obj.transform.position.z + 500) < zThreshold)
            {
                platformsToDelete.Add(obj);
            }
        }

        // Silinecek platformlar� yok et
        foreach (GameObject platform in platformsToDelete)
        {
            Destroy(platform);
        }
    }
}
