using UnityEngine;

public class CanvasRotation : MonoBehaviour
{
    [SerializeField] Transform _cameraTransform;
    private void Start()
    {
        _cameraTransform = FindAnyObjectByType<Camera>().gameObject.transform;
    }
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(_cameraTransform.position - transform.position);
    }
}