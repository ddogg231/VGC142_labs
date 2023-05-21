using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class Playermovement : MonoBehaviour
{
    
    public float Playerspeed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.0f;
    private Vector3 Playervelocity;
    private bool isGrounded;
    private PlayerInput playerInput;
    private Transform CameraTrasform;

    Rigidbody rb;
    public CharacterController controller;
    public GameObject model;


    Vector3 moveDir;
    Vector3 curMoveInput;
    Plane plane = new Plane(Vector3.up, 0);
    public Transform groundCheck;

    public float groundCheckRadius = 0.4f;
    public LayerMask groundMask;

    public GameObject player;
    public float health;
    
    int errorCounter = 0;
    //anims
    private Animator anim;
    int AnimX;
    int AnimZ;

    Vector2 currentAnimblendVector;
    Vector2 animVelocity;

    private InputAction MoveAction;
    private InputAction JumpAction;
   
    void start()
    {
        controller = GetComponent<CharacterController>();

        playerInput = GetComponent<PlayerInput>();
        MoveAction = playerInput.actions["Move"];
        JumpAction = playerInput.actions["Jump"];
        CameraTrasform = Camera.main.transform;
        
        try
        {
            model = GameObject.FindGameObjectWithTag("PlayerModel");
            rb = GetComponent<Rigidbody>();
            anim = model.GetComponent<Animator>();

            if (!isGrounded) throw new UnassignedReferenceException("moveSpeed not set" + name);
            if (!rb) throw new UnassignedReferenceException("Rigidbody not set on " + name);
            if (!groundCheck) throw new UnassignedReferenceException("Groundcheck not set on " + name);
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

 //private void Update()
 //{
 //    isGrounded = (Physics.OverlapSphere(groundCheck.position, groundCheckRadius, groundMask)).Length > 0;
 //
 //    curMoveInput.y = rb.velocity.y;
 //    rb.velocity = curMoveInput;
 //
 //  if (Mathf.Abs(moveDir.magnitude) > 0)
 //  {
 //      float dotProduct = Vector3.Dot(moveDir, transform.forward);
 //  
 //      if (dotProduct > -lookThreshole && dotProduct < lookThreshole)
 //      {
 //          float horizontalDotProduct = Vector3.Dot(Vector3.left, moveDir);
 //          float hValue;
 //          if (horizontalDotProduct < 0)
 //              hValue = -1;
 //          else
 //              hValue = 1;
 //  
 //          anim.SetFloat("Horzontal", hValue);
 //          anim.SetFloat("vertical", 0);
 //  
 //      }
 //  
 //      if (dotProduct >= lookThreshole)
 //      {
 //          anim.SetFloat("Horzontal", 0);
 //          anim.SetFloat("vertical", 1);
 //      }
 //      if (dotProduct >= -lookThreshole)
 //      {
 //          anim.SetFloat("Horzontal", 0);
 //          anim.SetFloat("vertical", -1);
 //      }
 //  }
 //  else
 //  {
 //      anim.SetFloat("Horzontal", 0);
 //      anim.SetFloat("vertical", 0);
 //  }
 //
 //    float distance;
 //    Ray mouseRAY = GameManager.Instance.MousePos();
 //    Vector3 worldPos;
 //
 //    if(plane.Raycast(mouseRAY, out distance))
 //    {
 //        worldPos = mouseRAY.GetPoint(distance);
 //
 //        Vector3 relativePos = worldPos - transform.position;
 //
 //        relativePos.y = 0;
 //
 //        transform.rotation = Quaternion.LookRotation(relativePos);
 //    }     // float dotProduct = Vector3.Dot(moveDir, transform.forward);
            // if (isGrounded && velocity.y < 0)
            // {
             //     velocity.y = -2f;
            // }

 //}

    // Update is called once per frame
  
    public void TakeDamge(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyPlayer), 0.5f);
        Debug.Log("damage taken");
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
        {
            curMoveInput = Vector3.zero;
            moveDir = Vector3.zero;
            return;
        }

        Vector2 move = ctx.action.ReadValue<Vector2>();
        

        moveDir = new Vector3(move.x, 0, move.y).normalized;
        curMoveInput = moveDir * Playerspeed;
    }
    public void Punch(InputAction.CallbackContext ctx)
    {
        
    }
    public void Kick(InputAction.CallbackContext ctx)
    {

    }
    public void Fire(InputAction.CallbackContext ctx)
    {

    }
    public void Jump(InputAction.CallbackContext ctx)
  {
      if (!isGrounded) return;
  
      rb.AddForce(jumpHeight * Vector3.up);
       
  }
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DestroyPlayer();
        }
    }
    private void DestroyPlayer()
    {
        Destroy(player);
    }
  public void Update()
    {
        isGrounded = controller.isGrounded;
       if(isGrounded && Playervelocity.y < 0)
        {
            Playervelocity.y = 0;
        }

        Vector2 input = MoveAction.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);
        //move the way the camera is facing
        move = move.x * CameraTrasform.right.normalized + move.z * CameraTrasform.forward.normalized;
        controller.Move(move * Time.deltaTime * Playerspeed);
        move.y = 0f;

        if(JumpAction.triggered && isGrounded)
        {
            Playervelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        Playervelocity.y += gravity * Time.deltaTime;
        controller.Move(Playervelocity * Time.deltaTime);


        //isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
    // float x = Input.GetAxis("Horizontal");
    // float z = Input.GetAxis("Vertical");
    //
    // Vector3 move = transform.right * x + transform.forward * z;
    //
    // controller.Move(move * speed * Time.deltaTime);
    //
    //if (Input.GetButtonDown("Jump") && isGrounded)
    //{
    //    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    //
    //
    //}
    //
    // velocity.y += gravity * Time.deltaTime;
    //
    // controller.Move(velocity * Time.deltaTime);
    //
    //
    //
    //   float distance;
    //   Ray mouseRAY = GameManager.Instance.MousePos();
    //   Vector3 worldPos;
    // 
    // if(plane.Raycast(mouseRAY, out distance))
    // {
    //   worldPos = mouseRAY.GetPoint(distance);
    // 
    //   Vector3 relativePos = worldPos - transform.position;
    // 
    //   relativePos.y = 0;
    // 
    //  transform.rotation = Quaternion.LookRotation(relativePos);
    // }
    }
}

    
