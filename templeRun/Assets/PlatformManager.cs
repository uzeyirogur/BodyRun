using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    // Silinecek platformlarýn baþlangýç adý
    public string platformNamePrefix = "yeniPlatform";

    // Kullanýcýnýn z deðerinden küçük olan platformlarý silen fonksiyon
    public void DestroyPlatformsBelowZ(float zThreshold)
    {
        // Sahnedeki tüm objeleri al
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        // Silinecek platformlarýn listesi
        List<GameObject> platformsToDelete = new List<GameObject>();

        // Tüm objeleri dön
        foreach (GameObject obj in allObjects)
        {
            // Objenin adý platformNamePrefix ile baþlýyorsa ve objenin pozisyonunun z bileþeni zThreshold'den küçükse
            if (obj.name.StartsWith(platformNamePrefix) && (obj.transform.position.z + 500) < zThreshold)
            {
                platformsToDelete.Add(obj);
            }
        }

        // Silinecek platformlarý yok et
        foreach (GameObject platform in platformsToDelete)
        {
            Destroy(platform);
        }
    }
}
