using UnityEngine;

namespace CharacterController2D
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        #region PRIVATE VARIABLES
        [Header("Movement attributes")]
        [Tooltip("Movement speed")]
        [SerializeField] private float movementSpeed;
        [Range(0.1f, 20f), Tooltip("The amount of speed reduction per second when the player is not moving")]
        [SerializeField] private float decelerationStrength;
        [Header("Jump attributes")]
        [Tooltip("Jump force")]
        [SerializeField] private float jumpForce;
        [Tooltip("Player's gravity scale")]
        [SerializeField] private float gravityScale;
        [Header("Other attributes")]
        [SerializeField] private LayerMask maskIgnoreSelf;

        //NON-SERIALIZED VARIABLES
        private Rigidbody2D rigidB;
        private Collider2D playerCollider;
        private Vector2 movementDirection;
        #endregion

        void Start()
        {
            rigidB = GetComponent<Rigidbody2D>();
            playerCollider = GetComponent<Collider2D>();
        }

        void Update()
        {
            //Update gravity
            if (rigidB.gravityScale != gravityScale)
                rigidB.gravityScale = gravityScale;

            movementDirection.y = 0f;
            float HorizontalInput = Input.GetAxisRaw("Horizontal");

            if(HorizontalInput != 0f)
            {
                //Assign movement direction multiplied by the speed
                movementDirection.x = HorizontalInput * movementSpeed;
            }
            else
            {
                //Decrease speed over time
                if (Mathf.Abs(movementDirection.x) >= 0.1f)
                    movementDirection.x += -Mathf.Sign(movementDirection.x) * decelerationStrength * Time.deltaTime;
                else
                    movementDirection.x = 0f;
            }


            #region JUMP
            Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.extents.y + 0.05f), Color.green);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Check if the player is on the ground
                if (Physics2D.BoxCast(playerCollider.bounds.center, new Vector2(playerCollider.bounds.size.x - 0.1f, 0.05f), 0f, Vector2.down, playerCollider.bounds.extents.y, maskIgnoreSelf))
                {
                    //print("Jump");
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
}
