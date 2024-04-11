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
    public float jumpHeight = 1f;

    private bool isRunning = false;
    private bool isMoving = false;
    public bool isJumping = false;
    private RoadPosition playerPosition;
    private Animator playerAnimator;


    void Start()
    {
        if (!mirrorLeft) mirrorLeft = GameObject.Find("Mirror Left");
        if (!mirrorRight) mirrorRight = GameObject.Find("Mirror Right");
        if (!levelControl) levelControl = GameObject.Find("LevelControl");
        playerAnimator = GameObject.Find("PlayerModel").GetComponent<Animator>();

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
        CharacterController charControl = GetComponent<CharacterController>();
        float pYPos = transform.position.y;
        float pJumpHeight = 0.3f;
        float cCYPos = charControl.center.y;
        float ccJumpHeight = 0.7f;

        float jumpingTime = playerAnimator.GetNextAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(jumpingTime * 0.05f);

        // Jumping up
        float elapsedTime = 0;
        float upPartTime = jumpingTime * 0.3f;
        while (elapsedTime < upPartTime)
        {
            float pYPosition = Mathf.Lerp(pYPos, pYPos + pJumpHeight, elapsedTime / upPartTime);
            float cCYPosition = Mathf.Lerp(cCYPos, cCYPos + ccJumpHeight, elapsedTime / upPartTime);
            transform.position = new Vector3(transform.position.x, pYPosition, transform.position.z);
            charControl.center = new Vector3(charControl.center.x, cCYPosition, charControl.center.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, pYPos + pJumpHeight, transform.position.z);
        charControl.center = new Vector3(charControl.center.x, cCYPos + ccJumpHeight, charControl.center.z);
        playerAnimator.SetBool("Jump", false);

        // Jumping down
        elapsedTime = 0;
        float downPartTime = jumpingTime * 0.30f;
        while (elapsedTime < downPartTime)
        {
            float pYPosition = Mathf.Lerp(pYPos + pJumpHeight, pYPos, elapsedTime / upPartTime);
            float cCYPosition = Mathf.Lerp(cCYPos + ccJumpHeight, cCYPos, elapsedTime / upPartTime);
            transform.position = new Vector3(transform.position.x, pYPosition, transform.position.z);
            charControl.center = new Vector3(charControl.center.x, cCYPosition, charControl.center.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, pYPos, transform.position.z);
        charControl.center = new Vector3(charControl.center.x, cCYPos, charControl.center.z);

        isJumping = false;
    }

    private IEnumerator ObstacleCollision(GameObject obstacle)
    {
        isRunning = false;
        playerAnimator.SetTrigger("GameOver");

        CollectableControl collectableControl = levelControl.GetComponent<CollectableControl>();
        collectableControl.AddLives(-1);
        yield return new WaitUntil(() => playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dying_Backwards"));

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
