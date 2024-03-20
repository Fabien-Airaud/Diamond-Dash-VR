using System.Collections;
using Unity.XR.CoreUtils;
using UnityEngine;

public enum PlayerPosition
{
    Left,
    Middle,
    Right
}

public class PlayerController : MonoBehaviour
{
    public XROrigin xROrigin;
    public float velocity;

    private readonly float xBound = 3.75f;
    private readonly float movingTime = 0.5f;
    private bool isMoving = false;
    private PlayerPosition playerPosition = PlayerPosition.Middle;

    private bool isGrounded = true;
    private bool jump = false;
    private Animator animator;
    private Rigidbody playerRb;


    // Start is called before the first frame update
    void Start()
    {
        if (xROrigin == null) xROrigin = FindObjectOfType<XROrigin>();
        animator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = playerRb.velocity.y;
        if ((Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal2") > 0) && !isMoving && isGrounded) MoveRight();
        else if ((Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Horizontal2") < 0) && !isMoving && isGrounded) MoveLeft();

        // Manage the player's jump and velocity
        JumpAndVelocity();

        // Update the xROrigin position to match the player's position
        xROrigin.transform.position = new Vector3(transform.position.x, xROrigin.transform.position.y, xROrigin.transform.position.z);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsGrounded", true);
        }
    }

    private IEnumerator MovePlayerOverTime(Vector3 targetPosition)
    {
        isMoving = true;
        float elapsedTime = 0;
        Vector3 startPosition = transform.position;

        while (elapsedTime < movingTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / movingTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }

    private void MoveLeft()
    {
        if (playerPosition != PlayerPosition.Left)
        {
            // Move the player to the left over movingTime
            StartCoroutine(MovePlayerOverTime(new Vector3(transform.position.x - xBound, transform.position.y, transform.position.z)));
            playerPosition--;
        }
    }

    private void MoveRight()
    {
        if (playerPosition != PlayerPosition.Right)
        {
            // Move the player to the right over movingTime
            StartCoroutine(MovePlayerOverTime(new Vector3(transform.position.x + xBound, transform.position.y, transform.position.z)));
            playerPosition++;
        }
    }

    private void JumpAndVelocity()
    {
        if (isGrounded)
        {
            if (jump)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Start_Jump") && animator.GetCurrentAnimatorClipInfoCount(0) < 10)
                {
                    animator.SetBool("IsGrounded", false);
                    isGrounded = false;
                    animator.SetBool("Jump", false);
                    jump = false;
                    //playerRb.AddForce(0, playerRb.mass * 10, 0, ForceMode.Impulse);
                }
            }
            else if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical2") > 0)
            {
                animator.SetBool("Jump", true);
                jump = true;
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("In_Air"))
        {
            playerRb.AddForce(0, -playerRb.mass * 30, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
