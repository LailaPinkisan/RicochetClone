using UnityEngine;

public class Elevator : MonoBehaviour
{
    public float launchUpForce = 20f;  // Adjust for height
    public float launchForwardForce = 15f; // Adjust for forward movement

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player entered launch pad trigger!");

        Transform rootObject = other.transform.root;
        if (rootObject.CompareTag("Player"))
        {
            Debug.Log("Player detected! Applying force...");

            if (rootObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Debug.Log("Rigidbody found! Current velocity: " + rb.linearVelocity);

                // Use the launch pad's rotation to determine forward
                Vector3 launchDirection = transform.forward * launchForwardForce + transform.up * launchUpForce;

                // Apply force
                rb.linearVelocity = launchDirection;

                Debug.Log("Force applied! New velocity: " + rb.linearVelocity);
            }
            else
            {
                Debug.LogWarning("No Rigidbody found on Player!");
            }
        }
    }
}
