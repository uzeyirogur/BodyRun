using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Takip edilecek hedef (oyuncu)

    public float smoothSpeed = 0.125f; // Kameran�n karaktere ne kadar yak�n olaca��n� kontrol eder
    public Vector3 offset; // Kameran�n karakterden ne kadar uzakta olaca��n� belirler

    private void LateUpdate()
    {
        if (target != null)
        {
            // Hedefin konumunu al, offset ile ayarlanm�� kamera konumunu ekleyerek kameran�n hedefin �st�nde olmas�n� sa�la
            Vector3 desiredPosition = target.position + offset;

            // Kameran�n yumu�ak bir �ekilde hedefin konumuna gitmesini sa�la
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Kameran�n rotasyonunu hedefin rotasyonuna e�itle
            transform.rotation = target.rotation;

            // Kameran�n yeni pozisyonunu ayarla
            transform.position = smoothedPosition;
        }


    }
}
