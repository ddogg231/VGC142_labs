using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;


[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class Playercontroller : MonoBehaviour
{
    [SerializeField]
    private float Playerspeed = 8f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float animSmoothtime = 0.1f;
    [SerializeField]
    private float roatationSpeed = 9;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public Transform groundCheck;
    public LayerMask groundMask;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction PunchAction;
    private InputAction KickAction;
    private InputAction KillAction;
    private InputAction FireAction;
   
    public Transform CameraTrasform;

    private Animator anim;
    int AnimX;
    int AnimZ;
    public GameObject model;
    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;
    public GameObject player;
    public float health;
  
    public bool isJumping;
    public bool isPunching;
    public bool isDead;
    public bool Kick;
    public bool fire;
    int errorCounter = 0;

    Coroutine jumpForceChange;
    Coroutine SpeedChange;

    public float fallDamageHeight = 10f;
    public float fallDamageMultiplier = 1f;
    public float maxFallDamage = 100f;

    private float startingHeight;
    // Start is called before the first frame update
    void Start()
    {
        startingHeight = transform.position.y;
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        anim = GetComponent<Animator>();
        AnimX = Animator.StringToHash("Horizontal");
        AnimZ = Animator.StringToHash("Vertical");
        
        CameraTrasform = Camera.main.transform;
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        PunchAction = playerInput.actions["Punch"];
        KickAction = playerInput.actions["Kick"];
        KillAction = playerInput.actions["Kill"];
        FireAction = playerInput.actions["Fire"];
        try
        {
            
            if (!groundedPlayer) throw new UnassignedReferenceException("moveSpeed not set" + name);
            if (!anim) throw new UnassignedReferenceException("Model not set on" + name);
        }
        catch (UnassignedReferenceException e)
        {
            Debug.Log(e.Message);
            errorCounter++;
        }
        finally
        {
            Debug.Log("The script ran with " + errorCounter.ToString() + " errors");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //groundedPlayer = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animSmoothtime);
        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);
        move = CameraTrasform.forward * move.z + CameraTrasform.right * move.x;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * Playerspeed);
        

        anim.SetFloat(AnimX, currentAnimationBlendVector.x);
        anim.SetFloat(AnimZ, currentAnimationBlendVector.y);

        if (jumpAction.triggered && groundedPlayer)
        {
            //anim.SetBool("Onground", true);
            anim.SetBool("isJumping", true);
            isJumping = true;
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
           
            
        }
        else
        {
           // anim.SetBool("Onground", false);
            anim.SetBool("isJumping", false);
            isJumping = false;
        }
          

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        //move the way the camera is facing
       Quaternion targetRotation = Quaternion.Euler(0, CameraTrasform.eulerAngles.y, 0);
       transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, roatationSpeed * Time.deltaTime);

        if (KickAction.triggered)
        {
            anim.SetBool("Kick", true);
            Kick = true;
        }
        else
        {
            anim.SetBool("Kick", false);
            Kick = false;
        }

        if (PunchAction.triggered)
        {
            anim.SetBool("isPunching", true);
            isPunching = true;
        }
        else
        {
            anim.SetBool("isPunching", false);
            isPunching = false;
        }
        if (KillAction.triggered)
        {
            TakeDamage(5);
        }
       
    }
  
   public void OnFire(InputValue value)
    {
        fire = value.isPressed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision object is the ground or any other surface that should trigger fall damage
        if (collision.gameObject.CompareTag("ground"))
        {
            
            // Calculate the fall height
            float fallHeight = startingHeight - transform.position.y;

            // Check if fall height exceeds the specified threshold
            if (fallHeight > fallDamageHeight)
            {
                // Calculate the fall damage based on the fall height
                float damage = Mathf.Clamp(fallHeight * fallDamageMultiplier, 0f, maxFallDamage);

                // Apply fall damage to the player's health
                TakeDamage(damage);
                Debug.Log("collisoion with ground");
            }
        }
    }


    public virtual void TakeDamage(float _damage)
    {
        health -= _damage;
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        anim.SetBool("isDead", true);
        isDead = true;
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
    public void StartJumpForceChange()
    {
        if (jumpForceChange == null)
        {
            jumpForceChange = StartCoroutine(JumpForceChange());
        }
        else
        {
            StopCoroutine(jumpForceChange);
            jumpForceChange = null;
            jumpHeight /= 2;
            jumpForceChange = StartCoroutine(JumpForceChange());

        }

    }
    IEnumerator JumpForceChange()
    {
        jumpHeight *= 2;
        yield return new WaitForSeconds(5.0f);

        jumpHeight /= 2;
        jumpForceChange = null;
    }
    public void StartSpeedChange()
    {
        if (SpeedChange == null)
        {
            SpeedChange = StartCoroutine(speedChange());

        }
        else
        {
            StopCoroutine(SpeedChange);
            SpeedChange = null;
            Playerspeed /= 2;
            SpeedChange = StartCoroutine(speedChange());
        }

    }
    IEnumerator speedChange()
    {
        Playerspeed *= 2;
        yield return new WaitForSeconds(15.0f);

        Playerspeed /= 2;
        SpeedChange = null;
    }
}
