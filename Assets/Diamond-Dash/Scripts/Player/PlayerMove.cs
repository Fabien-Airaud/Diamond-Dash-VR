using System.Collections;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float runningSpeed = 3f;
    public float movingTime = 0.5f;
    private bool isMoving = false;
    private RoadPosition playerPosition;


    private void Start()
    {
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        playerPosition = RoadPosition.Middle;
    }

    void Update()
    {
        // Run the player forward
        transform.Translate(runningSpeed * Time.deltaTime * transform.forward, Space.World);

        Move();
    }


    private IEnumerator MovePlayerOverTime(Vector3 targetPosition)
    {
        isMoving = true;
        float elapsedTime = 0;
        Vector3 startPosition = transform.position;

        while (elapsedTime < movingTime)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / movingTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
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
                    StartCoroutine(MovePlayerOverTime(new Vector3(transform.position.x + LevelBoundary.laneSize, transform.position.y, transform.position.z)));
                    playerPosition++;
                }
            }
            else if (Input.GetAxis("Horizontal") < 0 || Input.GetAxis("Horizontal2") < 0)
            {
                if (playerPosition != RoadPosition.Left)
                {
                    // Move the player to the left over movingTime
                    StartCoroutine(MovePlayerOverTime(new Vector3(transform.position.x - LevelBoundary.laneSize, transform.position.y, transform.position.z)));
                    playerPosition++;
                }
            }
        }
    }
}
