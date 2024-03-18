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
    private PlayerPosition playerPosition = PlayerPosition.Middle;


    // Start is called before the first frame update
    void Start()
    {
        if (xROrigin == null) xROrigin = FindObjectOfType<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }

        // Update the xROrigin position to match the player's position
        xROrigin.transform.position = new Vector3(transform.position.x, xROrigin.transform.position.y, xROrigin.transform.position.z);
    }


    private void MoveLeft()
    {
        if (playerPosition == PlayerPosition.Middle)
        {
            transform.position = new Vector3(-xBound, transform.position.y, transform.position.z);
            playerPosition = PlayerPosition.Left;
        }
        else if (playerPosition == PlayerPosition.Right)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            playerPosition = PlayerPosition.Middle;
        }
    }

    private void MoveRight()
    {
        if (playerPosition == PlayerPosition.Middle)
        {
            transform.position = new Vector3(xBound, transform.position.y, transform.position.z);
            playerPosition = PlayerPosition.Right;
        }
        else if (playerPosition == PlayerPosition.Left)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            playerPosition = PlayerPosition.Middle;
        }
    }
}
