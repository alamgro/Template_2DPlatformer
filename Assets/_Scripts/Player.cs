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
        [Tooltip("When this is checked, the player will stop immediately if it is not receiving a movement input.")]
        [SerializeField] private bool stopMovementImmediately;
        [Range(0.1f, 30f), Tooltip("The amount of speed reduction per second when the player is not moving")]
        [SerializeField] private float decelerationStrength;
        [Header("Jump attributes")]
        [Tooltip("Jump force")]
        [SerializeField] private float jumpForce;
        [SerializeField] AnimationCurve jumpCurve;
        [SerializeField] float jumpCurveTotalTime;
        [Tooltip("Player's gravity scale")]
        [SerializeField] private float gravityScale;
        [Header("Other attributes")]
        [SerializeField] private LayerMask maskIgnoreSelf;

        //NON-SERIALIZED VARIABLES
        private Rigidbody2D rigidB;
        private Collider2D playerCollider;
        private Vector2 movementDirection;
        private float jumpCurveDelta;
        private bool isJumping = false;
        #endregion

        void Start()
        {
            #region GET COMPONENTS
            rigidB = GetComponent<Rigidbody2D>();
            playerCollider = GetComponent<Collider2D>();
            #endregion

        }

        void Update()
        {
            //Update gravity
            if (rigidB.gravityScale != gravityScale)
                rigidB.gravityScale = gravityScale;

            float HorizontalInput = Input.GetAxisRaw("Horizontal");

            if(HorizontalInput != 0f)
            {
                //Assign movement direction multiplied by the speed
                movementDirection.x = HorizontalInput * movementSpeed;
            }
            else
            {
                //Stop movement
                if (stopMovementImmediately || Mathf.Abs(movementDirection.x) <= 0.1f) 
                    movementDirection.x = 0f;
                else //Reduce movement gradually
                    movementDirection.x += -Mathf.Sign(movementDirection.x) * decelerationStrength * Time.deltaTime;
            }

            if (IsGrounded())
            {
                Debug.Log("Is Grounded");
                AvoidEdgeSlide();
                movementDirection.y = 0f;
                isJumping = false;
            }
            else
            {
                rigidB.gravityScale = gravityScale;
                movementDirection.y = rigidB.velocity.y;
            }

            #region JUMP
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                //Check if the player is on the ground
                jumpCurveDelta = 0f;
                isJumping = true;
                //rigidB.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            else if(Input.GetKey(KeyCode.Space) && !isJumping)
            {
                if(jumpCurveDelta <= 1f)
                {
                    movementDirection.y = jumpCurve.Evaluate(jumpCurveDelta) * jumpForce;
                    //Debug.Log(movementDirection.y);
                    jumpCurveDelta += Time.deltaTime / jumpCurveTotalTime;
                }
            }
            #endregion

            //AvoidEdgeSlide();
        }

        private void FixedUpdate()
        {
            rigidB.velocity = movementDirection;
        }

        private bool IsGrounded()
        {
            Debug.DrawRay(playerCollider.bounds.center, Vector2.down * (playerCollider.bounds.extents.y + 0.05f), Color.blue);

            return Physics2D.BoxCast(playerCollider.bounds.center, 
                new Vector2(playerCollider.bounds.size.x - 0.1f, 0.05f), 0f, 
                Vector2.down, playerCollider.bounds.extents.y + 0.05f, maskIgnoreSelf);
        }

        private void AvoidEdgeSlide()
        {
            rigidB.velocity = Vector2.zero;
            rigidB.angularVelocity = 0f;
            rigidB.gravityScale = 0f;
        }

    }
}
