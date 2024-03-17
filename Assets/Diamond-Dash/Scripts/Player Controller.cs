using UnityEngine;

public enum PlayerPosition
{
    Left,
    Middle,
    Right
}

public class PlayerController : MonoBehaviour
{
    private readonly float xBound = 3.75f;
    private PlayerPosition playerPosition = PlayerPosition.Middle;


    // Start is called before the first frame update
    void Start()
    {
        
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
