using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public Transform orientation; // Assign this in the Inspector

    void Update()
    {
        // Make the player rotate to match the orientation
        transform.rotation = Quaternion.Euler(0, orientation.eulerAngles.y, 0);
    }
}
