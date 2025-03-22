using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 100f;
    public float sensY = 100f;
    public Transform orientation;
    public Transform cameraTransform;  // Assign the Camera (inside CameraHolder)

    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate the camera (for looking up/down)
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Rotate the player only left/right (prevents sideways movement)
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
