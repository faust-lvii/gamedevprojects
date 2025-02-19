using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0)) // Fare basılı tutulduğunda
        {
            // Fare pozisyonunu dünya koordinatlarına çevir
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Hedef pozisyonu al
                Vector3 targetPosition = hit.point;

                // Hareket yönünü hesapla
                Vector3 direction = (targetPosition - transform.position).normalized;
                direction.y = 0; // Y eksenindeki hareketi engelle

                // Karakteri hareket ettir
                transform.position += direction * moveSpeed * Time.deltaTime;

                // Karakteri hareket yönüne döndür
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            // Fare basılı değilken sadece fareye doğru bak
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                pointToLook.y = transform.position.y;

                Vector3 lookDirection = (pointToLook - transform.position).normalized;
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.LookRotation(lookDirection),
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}