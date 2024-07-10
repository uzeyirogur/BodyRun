using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Takip edilecek hedef (oyuncu)

    public float smoothSpeed = 0.125f; // Kameranýn karaktere ne kadar yakýn olacaðýný kontrol eder
    public Vector3 offset; // Kameranýn karakterden ne kadar uzakta olacaðýný belirler

    private void LateUpdate()
    {
        if (target != null)
        {
            // Hedefin konumunu al, offset ile ayarlanmýþ kamera konumunu ekleyerek kameranýn hedefin üstünde olmasýný saðla
            Vector3 desiredPosition = target.position + offset;

            // Kameranýn yumuþak bir þekilde hedefin konumuna gitmesini saðla
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Kameranýn rotasyonunu hedefin rotasyonuna eþitle
            transform.rotation = target.rotation;

            // Kameranýn yeni pozisyonunu ayarla
            transform.position = smoothedPosition;
        }


    }
}
