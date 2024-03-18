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

    private readonly float xBound = 3.75f;
    private readonly float movingTime = 0.5f;
    private bool isMoving = false;
    private PlayerPosition playerPosition = PlayerPosition.Middle;


    // Start is called before the first frame update
    void Start()
    {
        if (xROrigin == null) xROrigin = FindObjectOfType<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !isMoving) MoveLeft();
        else if (Input.GetKeyDown(KeyCode.RightArrow) && !isMoving) MoveRight();

        // Update the xROrigin position to match the player's position
        xROrigin.transform.position = new Vector3(transform.position.x, xROrigin.transform.position.y, xROrigin.transform.position.z);
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
}
