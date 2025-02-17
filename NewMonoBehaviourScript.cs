using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float stoppingDistance = 0.1f;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private CharacterController characterController;
    private Camera mainCamera;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        targetPosition = transform.position;
    }

    private void Update()
    {
        // Fare ile tıklama kontrolü
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
                isMoving = true;
            }
        }

        // Karakterin hedef noktaya hareketi
        if (isMoving)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0; // Y eksenindeki hareketi engelle

            // Hedef noktaya olan mesafeyi hesapla
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

            // Karakter hedefe yeterince yakınsa hareketi durdur
            if (distanceToTarget < stoppingDistance)
            {
                isMoving = false;
            }
            else
            {
                // Karakteri hareket ettir
                transform.position += direction * moveSpeed * Time.deltaTime;

                // Karakteri hareket yönüne döndür
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        // Fare yönüne bakma
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