using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public Animator animator;
    private Rigidbody2D characterBody;
    private Vector2 inputMovement;
    private float baseScale = 1.5f; // Keep the scale at 1.5x

    void Start()
    {
        characterBody = GetComponent<Rigidbody2D>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // Get movement input
        inputMovement = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        // Set Speed parameter in Animator (for idle/walking animation)
        animator.SetFloat("Speed", inputMovement.magnitude);

        // Flip the character left/right while keeping 1.5x scale
        if (inputMovement.x > 0) // Moving right
        {
            transform.localScale = new Vector3(baseScale, baseScale, 1);
        }
        else if (inputMovement.x < 0) // Moving left
        {
            transform.localScale = new Vector3(-baseScale, baseScale, 1);
        }
    }

    private void FixedUpdate()
    {
        // Move the player
        Vector2 delta = inputMovement * speed * Time.deltaTime;
        Vector2 newPosition = characterBody.position + delta;
        characterBody.MovePosition(newPosition);
    }
}
