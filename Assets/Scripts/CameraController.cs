using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensitivity = 2f; // Hiiren herkkyys
    public float smoothTime = 0.1f; // Tasoitusaika hiiren liikkeelle

    private Vector2 currentMouseLook;
    private Vector2 smoothV;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothTime);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothTime);
        currentMouseLook += smoothV * sensitivity;

        currentMouseLook.y = Mathf.Clamp(currentMouseLook.y, -90f, 90f);

        transform.localRotation = Quaternion.AngleAxis(-currentMouseLook.y, Vector3.right);
        transform.parent.localRotation = Quaternion.AngleAxis(currentMouseLook.x, Vector3.up);
    }
}
