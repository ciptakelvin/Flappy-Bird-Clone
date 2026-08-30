using UnityEngine;

// This attribute automatically adds a Rigidbody2D component to the GameObject 
// if it doesn't already have one, preventing null reference errors.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Reference to the 2D physics body of the player
    private Rigidbody2D rb;

    private Camera mainCamera;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip flapClip;
    [SerializeField] private AudioClip playClip;
    [SerializeField] private AudioClip dieClip;
    [SerializeField] private AudioClip scoreClip;

    [SerializeField] private float topPadding = 0.05f;

    // The upward velocity applied to the bird when jumping
    [SerializeField] private float flapForce = 7f;

    [Header("Rotation Settings")]
    [SerializeField] private float maxDownAngle = -90f;
    [SerializeField] private float maxFallSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Cache the Rigidbody2D component attached to this GameObject
        rb = GetComponent<Rigidbody2D>();

        mainCamera = Camera.main; // Grabs the main camera in the scene
    }

    // Update is called once per frame and is ideal for checking input timings
    void Update()
    {
        KeepPlayerInBounds();
        UpdateRotation();

        // USED IN LEGACY INPUT SYSTEM
        // Polls the legacy system to check if the Spacebar was pressed down on this frame.
        // You can also use Input.GetButtonDown("Jump") if configured in Project Settings -> Input Manager.
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    OnJump();
        //}
    }

    private void UpdateRotation()
    {
        if (Time.timeScale == 0f || rb == null) return;

        float targetAngle = 0f;
        if (rb.linearVelocity.y < 0)
        {
            float fallFraction = Mathf.Clamp01(-rb.linearVelocity.y / maxFallSpeed);
            targetAngle = Mathf.Lerp(0f, maxDownAngle, fallFraction);
        }

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    // This callback is triggered automatically by the Player Input component 
    // when the "Jump" action is performed (using the 'Send Messages' behavior).
    private void OnJump()
    {
        // If the game is paused or not started yet, don't jump or play audio!
        if (Time.timeScale == 0f)
            return;

        // Safety check to ensure the Rigidbody2D reference exists
        if (rb == null)
            return;

        // RECOMMENDED (Modern Unity): Instantly overrides the vertical velocity to the flapForce.
        // This property handles the underlying Vector2 struct manipulation for you, keeping
        // your horizontal speed (x) intact while ensuring a perfectly consistent jump height.
        rb.linearVelocityY = flapForce;

        // ALTERNATIVE 1: The older struct-assignment method.
        // Necessary in older Unity versions where you couldn't modify 'y' directly.
        // It creates a brand-new Vector2 using the current X velocity and the new Y force.
        // rb.linearVelocity = new Vector2(rb.linearVelocity.x, flapForce);

        // ALTERNATIVE 2: The Force-Based approach.
        // Line 1 resets the vertical speed to 0 so downward gravity momentum is cleared.
        // Line 3 adds an instant physical push upward. Unlike the direct velocity methods above,
        // this approach is directly affected by the Rigidbody2D's 'Mass' setting in the Inspector.
        // rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        // rb.AddForce(Vector2.up * flapForce, ForceMode2D.Impulse);

        transform.rotation = Quaternion.identity; // Back to facing front

        audioSource.PlayOneShot(flapClip);
    }

    private void KeepPlayerInBounds()
    {
        // 1. Calculate the exact world Y-coordinate of the top of the screen
        // Viewport (0.5, 1) represents the top-center of the screen
        Vector3 topScreenWorldPos = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f));

        // Subtract padding so the bird's sprite doesn't halfway vanish off-screen
        float maxAllowedY = topScreenWorldPos.y - topPadding;

        // 2. Check if the bird has crossed the line
        if (transform.position.y > maxAllowedY)
        {
            // Lock the position to the ceiling
            transform.position = new Vector3(transform.position.x, maxAllowedY, transform.position.z);

            // CRUCIAL: Zero out upward velocity so physics doesn't keep fighting the position lock
            if (rb != null && rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Use rb.velocity on older Unity versions
            }
        }
    }

    public void ResetPlayer()
    {
        transform.position = new Vector3(-5.5f, 0f, 0f);

        // Zero out the physics forces
        rb.linearVelocity = Vector2.zero; // Note: Use rb.velocity if using an older Unity version than 2026/2025 LTS
        rb.angularVelocity = 0f;
        transform.rotation = Quaternion.identity;

        audioSource.PlayOneShot(playClip);
    }

    public void Die()
    {
        audioSource.PlayOneShot(dieClip);
    }

    public void Score()
    {
        audioSource.PlayOneShot(scoreClip);
    }
}