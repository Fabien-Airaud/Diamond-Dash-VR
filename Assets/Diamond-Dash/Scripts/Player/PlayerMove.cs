using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameObject MirrorLeft;
    public GameObject MirrorRight;
    public float runningSpeed = 5f;
    public float movingTime = 0.5f;

    private bool isRunning = false;
    private bool isMoving = false;
    private RoadPosition playerPosition;
    private Animator playerAnimator;


    private void Start()
    {
        playerAnimator = GameObject.Find("PlayerModel").GetComponent<Animator>();
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        playerPosition = RoadPosition.Middle;
    }

    void Update()
    {
        if (isRunning)
        {
            // Run the player forward and the two mirrors
            transform.Translate(runningSpeed * Time.deltaTime * transform.forward, Space.World);
            MirrorLeft.transform.Translate(runningSpeed * Time.deltaTime * transform.forward, Space.World);
            MirrorRight.transform.Translate(runningSpeed * Time.deltaTime * transform.forward, Space.World);

            Move();
        }
    }


    private IEnumerator StartMovingPlayer(float startTime)
    {
        float elapsedTime = 0;

        while (elapsedTime < startTime)
        {
            transform.Translate((elapsedTime/ startTime) * runningSpeed * Time.deltaTime * transform.forward, Space.World);
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

    public void StartMove(float startTime)
    {
        playerAnimator.SetTrigger("StartRunning");
        StartCoroutine(StartMovingPlayer(startTime));
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
        }
    }
}
