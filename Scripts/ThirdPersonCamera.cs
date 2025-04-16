using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 5f;
    public float height = 2f;
    public float damping = 5f;
    
    void LateUpdate()
    {
        Vector3 targetPosition = target.position - target.forward * distance + Vector3.up * height;
        
        transform.position = Vector3.Lerp(transform.position, targetPosition, damping * Time.deltaTime);
        
        transform.LookAt(target);
    }

    void Start() {
    if (target == null) {
        target = GameObject.Find("single boat").transform;
        Debug.Log(target == null ? "未找到船只" : "已自动赋值船只");
    }
}
}