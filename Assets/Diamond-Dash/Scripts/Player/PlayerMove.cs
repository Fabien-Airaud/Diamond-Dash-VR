using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    public GameObject mirrorLeft;
    public GameObject mirrorRight;
    public GameObject levelControl;
    public float runningSpeed = 5f;
    public float movingTime = 0.5f;

    private bool isRunning = false;
    private bool isMoving = false;
    private bool isJumping = false;
    private bool isSliding = false;
    private RoadPosition playerPosition;
    private Animator playerAnimator;
    private CharacterController characterController;


    void Start()
    {
        if (!mirrorLeft) mirrorLeft = GameObject.Find("Mirror Left");
        if (!mirrorRight) mirrorRight = GameObject.Find("Mirror Right");
        if (!levelControl) levelControl = GameObject.Find("LevelControl");
        playerAnimator = GameObject.Find("PlayerModel").GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        playerPosition = RoadPosition.Middle;
    }

    void Update()
    {
        if (isRunning)
        {
            RunPlayer(runningSpeed);
            Move();
        }
    }


    // Run the player forward and the two mirrors
    private void RunPlayer(float speed)
    {
        transform.Translate(speed * Time.deltaTime * transform.forward, Space.World);
        mirrorLeft.transform.Translate(speed * Time.deltaTime * transform.forward, Space.World);
        mirrorRight.transform.Translate(speed * Time.deltaTime * transform.forward, Space.World);
    }

    private IEnumerator StartMovingPlayer(float startTime)
    {
        float elapsedTime = 0;

        while (elapsedTime < startTime)
        {
            RunPlayer((elapsedTime / startTime) * runningSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isRunning = true;
    }

    private IEnumerator MovePlayerOverTime(float targetXPosition)
    {
        isMoving = true;
        float elapsedTime = 0;
        float startxPosition = transform.position.x;

        while (elapsedTime < movingTime)
        {
            float xPosition = Mathf.Lerp(startxPosition, targetXPosition, elapsedTime / movingTime);
            transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(targetXPosition, transform.position.y, transform.position.z);
        isMoving = false;
    }

    private IEnumerator JumpPlayerOverTime()
    {
        isJumping = true;
        playerAnimator.SetBool("Jump", true);
        yield return null; // Wait for the next frame to get the correct jumping time

        float pYPos = transform.position.y;
        float pJumpHeight = 0.3f;
        float cCYPos = characterController.center.y;
        float ccJumpHeight = 0.7f;

        float jumpingTime = playerAnimator.GetNextAnimatorStateInfo(0).length;
        Debug.Log("Jumping time: " + jumpingTime);
        yield return new WaitForSeconds(jumpingTime * 0.05f);

        // Jumping up
        float elapsedTime = 0;
        float partTime = jumpingTime * 0.3f;
        while (elapsedTime < partTime)
        {
            float pYPosition = Mathf.Lerp(pYPos, pYPos + pJumpHeight, elapsedTime / partTime);
            float cCYPosition = Mathf.Lerp(cCYPos, cCYPos + ccJumpHeight, elapsedTime / partTime);
            transform.position = new Vector3(transform.position.x, pYPosition, transform.position.z);
            characterController.center = new Vector3(characterController.center.x, cCYPosition, characterController.center.z);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        transform.position = new Vector3(transform.position.x, pYPos + pJumpHeight, transform.position.z);
        characterController.center = new Vector3(characterController.center.x, cCYPos + ccJumpHeight, characterController.center.z);
        playerAnimator.SetBool("Jump", false);

        // Jumping down
        elapsedTime = 0;
        partTime = jumpingTime * 0.3f;
        while (elapsedTime < partTime)
        {
            float pYPosition = Mathf.Lerp(pYPos + pJumpHeight, pYPos, elapsedTime / partTime);
            float cCYPosition = Mathf.Lerp(cCYPos + ccJumpHeight, cCYPos, elapsedTime / partTime);
            transform.position = new Vector3(transform.position.x, pYPosition, transform.position.z);
            characterController.center = new Vector3(characterController.center.x, cCYPosition, characterController.center.z);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        transform.position = new Vector3(transform.position.x, pYPos, transform.position.z);
        characterController.center = new Vector3(characterController.center.x, cCYPos, characterController.center.z);

        yield return new WaitForSeconds(jumpingTime * 0.25f);
        isJumping = false;
    }

    private IEnumerator ObstacleCollision(GameObject obstacle)
    {
        isRunning = false;
        playerAnimator.SetBool("GameOver", true);

        CollectableControl collectableControl = levelControl.GetComponent<CollectableControl>();
        collectableControl.AddLives(-1);
        yield return new WaitUntil(() => playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dying_Backwards"));
        playerAnimator.SetBool("GameOver", false);

        if (collectableControl.IsAlive())
        {
            playerAnimator.SetTrigger("StandUp");
            yield return new WaitUntil(() => playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Standing_up"));
            Destroy(obstacle);
            yield return new WaitUntil(() => playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Standing_Idle"));
            StartMove(1f);
        }
        else
        {
            yield return new WaitForSeconds(playerAnimator.GetCurrentAnimatorStateInfo(0).length);
            levelControl.GetComponent<LevelController>().EndLevel();
        }
    }

    public void StartMove(float startTime)
    {
        playerAnimator.SetTrigger("StartRunning");
        StartCoroutine(StartMovingPlayer(startTime));
    }

    public void HitObstacle(GameObject obstacle)
    {
        StartCoroutine(ObstacleCollision(obstacle));
    }

    private void Move()
    {
        if (!isMoving)
        {
            if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal2") > 0)
            {
                if (playerPosition != RoadPosition.Right)
                {
                    // Move the player to the right over movingTime
                    StartCoroutine(MovePlayerOverTime(transform.position.x + LevelBoundary.laneSize));
                    playerPosition++;
                }
            }
            else if (Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Horizontal2") < 0)
            {
                if (playerPosition != RoadPosition.Left)
                {
                    // Move the player to the left over movingTime
                    StartCoroutine(MovePlayerOverTime(transform.position.x - LevelBoundary.laneSize));
                    playerPosition--;
                }
            }
            else if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical2") > 0)
            {
                if (!isJumping)
                {
                    // Jump the player
                    StartCoroutine(JumpPlayerOverTime());
                }
            }
        }
    }
}
