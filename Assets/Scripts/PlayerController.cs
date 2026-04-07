using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public float jumpForce = 5f;
    private int jumpCount;
    private int maxJumps = 2;
    private bool isGrounded;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI winTextObject;
    
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        winTextObject.gameObject.SetActive(false); // hide first
        SetCountText();                             // then update text
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementvector = movementValue.Get<Vector2>();

        movementX = movementvector.x;
        movementY = movementvector.y;
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && (isGrounded || jumpCount < maxJumps))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;
            isGrounded = false;
        }
    }

private void OnCollisionStay(Collision collision)
{
    isGrounded = true;
}

private void OnCollisionExit(Collision collision)
{
    isGrounded = false;
}

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 12)
        {
            winTextObject.gameObject.SetActive(true);
            winTextObject.text = "You win!";
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Destroy the current object
            Destroy(gameObject); 
            // Update the winText to display "You Lose!"
            winTextObject.gameObject.SetActive(true);
            winTextObject.text = "You Lose!";
        } else
        {
            isGrounded = true;
            jumpCount = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
    }
}
