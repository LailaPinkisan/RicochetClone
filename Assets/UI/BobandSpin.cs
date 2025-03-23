using UnityEngine;

public class BobandSpin : MonoBehaviour
{
    [Header("Spin Settings")]
    public float rotationSpeed = 50f;

    [Header("Bob Settings")]
    public float bobHeight = 0.2f; // height of the bobbing motion
    public float bobSpeed = 2f; // Speed of bobbing

    private Vector3 startPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // speed the object
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // bobbing motino
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
