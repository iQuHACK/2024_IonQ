using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// public struct QubitID {
//     public QubitID(int id) {
//         ID = id;
//     }

//     public int ID { get; }

//     public override string ToString() => $"{ID}";
// }

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D coll;
    public int playerIndex;
    public float state;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] public int qubitID;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        state = 0f;
        SwitchControl();
    }

    // Update is called once per frames
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchControl();
        }

        float dirX = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(dirX * moveSpeed, body.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    void SwitchControl()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int currentIndex = -1;

        // Find the index of the current player
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PlayerMovement>().playerIndex == playerIndex)
            {
                currentIndex = i;
                break;
            }
        }

        // Disable current player control
        players[currentIndex].GetComponent<PlayerMovement>().enabled = false;

        // Enable control for the next player in the array (cyclically)
        int nextIndex = (currentIndex + 1) % players.Length;
        players[nextIndex].GetComponent<PlayerMovement>().enabled = true;
    }

    void ChangeColor(double x) {

    }
}
