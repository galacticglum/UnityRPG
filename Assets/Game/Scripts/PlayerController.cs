using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Phyiscs")]
    [SerializeField]
    private float gravity = 20;
    [Header("Movement")]
    [SerializeField]
    private float jumpSpeed = 8;
    [SerializeField]
    private float runSpeed = 10;
    [SerializeField]
    private float walkSpeed = 4;
    [SerializeField]
    private float rotationSpeed = 200;
    [Range(0, 1)]
    [SerializeField]
    private float speedModifier = 0.75f;

    private float currentSpeedModifier;
    private bool isGrounded;
    private bool isWalking;
    private bool isToggleMove;
    private Vector3 velocity = Vector3.zero;
    private float toggleMoveCooldown = 0.5f;
    private float currentCooldown;
    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isWalking = true;
        if (Input.GetAxis("Run") != 0)
        {
            isWalking = false;
        }

        if (isGrounded)
        {
            velocity = new Vector3(Input.GetMouseButton(1) ? Input.GetAxis("Horizontal") : 0, 0, Input.GetAxis("Vertical"));
            if (currentCooldown > 0)
            {
                currentCooldown -= Time.deltaTime;
            }
            else if (currentCooldown <= 0)
            {
                currentCooldown = 0;
            }

            if (Input.GetAxis("Toggle Move") != 0 && currentCooldown == 0)
            {
                currentCooldown = toggleMoveCooldown;
                isToggleMove = !isToggleMove;
            }

            // We have taken control over our character and therefore toggle move is no longer valid.
            if (isToggleMove && (Input.GetAxis("Vertical") != 0 || Input.GetButton("Jump")) ||
                Input.GetMouseButton(0) && Input.GetMouseButton(1))
            {
                isToggleMove = false;
            }

            if (Input.GetMouseButton(0) && Input.GetMouseButton(1) && Input.GetAxis("Vertical") == 0 || isToggleMove)
            {
                velocity.z += 1;
            }

            if (velocity.z > 1)
            {
                velocity.z = 1;
            }

            velocity.x += Input.GetAxis("Strafing");
            if (Input.GetMouseButton(1) && Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0)
            {
                velocity.x *= speedModifier;
            }

            currentSpeedModifier = Input.GetAxis("Vertical") < 0 || Input.GetMouseButton(1) && Input.GetAxis("Horizontal") != 0 || 
                Input.GetAxis("Strafing") != 0 ? speedModifier : 1;

            velocity *= isWalking ? walkSpeed * currentSpeedModifier : runSpeed * currentSpeedModifier;
            if (Input.GetButton("Jump"))
            {
                velocity.y = jumpSpeed;
            }

            velocity = transform.TransformDirection(velocity);
        }

        if (Input.GetMouseButton(1))
        {
            transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        }
        else
        {
            transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, 0);
        }

        velocity.y -= gravity * Time.deltaTime;
        isGrounded = (characterController.Move(velocity * Time.deltaTime) & CollisionFlags.Below) != 0;
    }
}
