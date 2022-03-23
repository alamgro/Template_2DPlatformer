using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Basic attributes")]
    [Tooltip("Velocidad de movimiento")]
    [SerializeField] private float movementSpeed;
    [Tooltip("Fuerza de salto")]
    [SerializeField] private float jumpForce;
    [Tooltip("Fuerza de gravedad")]
    [SerializeField] private float gravityScale;
    [Header("Other attributes")]
    [SerializeField] private LayerMask maskIgnoreSelf;

    private Rigidbody2D rigidB;
    private Collider2D playerCollider;
    private Vector2 movementDirection;

    void Start()
    {
        rigidB = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();

    }

    
    void Update()
    {
        //Update gravity
        if(rigidB.gravityScale != gravityScale)
            rigidB.gravityScale = gravityScale;

        movementDirection.x = Input.GetAxisRaw("Horizontal") * movementSpeed;

        #region JUMP
        Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.extents.y + 0.05f), Color.green);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Check if the player is on the ground
            if(Physics2D.BoxCast(playerCollider.bounds.center, new Vector2(playerCollider.bounds.size.x - 0.1f, 0.05f), 0f, Vector2.down, playerCollider.bounds.extents.y, maskIgnoreSelf))
            {
                print("Jump");
                rigidB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        #endregion

        movementDirection.y = rigidB.velocity.y;

    }

    private void FixedUpdate()
    {
        rigidB.velocity = movementDirection;
    }
}
