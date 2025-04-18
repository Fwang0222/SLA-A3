using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float height = 2f;
    public float damping = 5f;
    
    public float rotationSpeed = 3f;
    public float minVerticalAngle = -20f;
    public float maxVerticalAngle = 80f;
    
    public float scrollSensitivity = 5f;
    public float minDistance = 2f;
    public float maxDistance = 10f;
    
    private float currentHorizontalAngle = 0f;
    private float currentVerticalAngle = 30f;
    private Vector3 offset;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.Find("single boat").transform;
            Debug.Log(target == null ? "Boat not found" : "Boat is automatically assigned");
        }
        
        currentHorizontalAngle = target.eulerAngles.y;
        UpdateCameraOffset();
    }

    void UpdateCameraOffset()
    {
        offset = new Vector3(0, height, -distance);
    }

    void LateUpdate()
    {
        HandleZoomInput();
      
        currentHorizontalAngle += Input.GetAxis("Mouse X") * rotationSpeed;
        currentVerticalAngle -= Input.GetAxis("Mouse Y") * rotationSpeed;
        
        currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minVerticalAngle, maxVerticalAngle);
        
        Quaternion rotation = Quaternion.Euler(currentVerticalAngle, currentHorizontalAngle, 0);
      
        Vector3 targetPosition = target.position + rotation * offset;
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, damping * Time.deltaTime);
       
        transform.LookAt(target);
    }

    void HandleZoomInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            distance = Mathf.Clamp(distance - scroll * scrollSensitivity, minDistance, maxDistance);
            UpdateCameraOffset();
        }
    }
}