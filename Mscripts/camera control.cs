using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; // Karakterin transformu
    public float distance = 5.0f; // Kameranın karaktere olan uzaklığı
    public float height = 3.0f; // Kameranın yüksekliği
    public float heightDamping = 2.0f; // Yükseklik değişiminin yumuşaklığı
    public float rotationDamping = 3.0f; // Dönüşün yumuşaklığı

    void LateUpdate()
    {
        if (!target)
            return;

        // Hedefin yüksekliğini ve dönüşünü al
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Dönüşü yumuşak bir şekilde uygula
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Yüksekliği yumuşak bir şekilde uygula
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Dönüşü Quaternion'a çevir
        Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Kameranın pozisyonunu ayarla
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Kameranın yüksekliğini ayarla
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Kamerayı hedefe doğru bakacak şekilde ayarla
        transform.LookAt(target);
    }
}