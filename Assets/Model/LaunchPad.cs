using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public float launchUpForce = 20f;
    public float launchForwardForce = 25f;
    private bool canLaunch = true;
    
    private PlayerMovement playerMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (canLaunch && other.transform.root.CompareTag("Player"))
        {
            if (other.transform.root.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                playerMovement = other.transform.root.GetComponent<PlayerMovement>();

                // Only disable height clamping if it's a SpecialLaunchPad
                if (playerMovement != null && gameObject.CompareTag("SpecialLaunchPad"))
                {
                    playerMovement.DisableClamp(true);
                }

                canLaunch = false;

                // Reset Y velocity before applying force
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

                // Apply launch force
                Vector3 launchVelocity = transform.up * launchUpForce + (-transform.forward) * launchForwardForce;
                rb.linearVelocity = launchVelocity;

                // Allow launching again after 0.5s
                Invoke(nameof(ResetLaunch), 0.5f);
            }
        }
    }

    private void ResetLaunch()
    {
        canLaunch = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Re-enable height clamp when leaving the launch pad
        if (playerMovement != null && gameObject.CompareTag("SpecialLaunchPad"))
        {
            playerMovement.DisableClamp(false);
        }
    }
}
