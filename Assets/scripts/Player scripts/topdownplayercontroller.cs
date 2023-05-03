using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class topdownplayercontroller : MonoBehaviour
{
    GameObject model;
    public float moveSpeed = 10.0f;
    public float jumpSpeed = 500.0f;

    Rigidbody rb;
    Animator anim;
    Vector3 curMoveInput;
    Vector3 moveDir;

    Plane plane = new Plane(Vector3.up, 0);

    public LayerMask isGroundLayer;
    public float groundCheckRadius;
    public Transform groundCheck;
    public bool isGrounded;
    public float lookThreshole = 0.6f;

    int errorCounter = 0;
    


    // Start is called before the first frame update
    void Start()
    {
        if (moveSpeed <= 0)
        {
            moveSpeed = 10.0f;
            Debug.Log("Speed was set incorrect, defaulting to " + moveSpeed.ToString());
        }

        try
        {
            model = GameObject.FindGameObjectWithTag("PlayerModel");
            rb = GetComponent<Rigidbody>();
            anim = model.GetComponent<Animator>();

            if (!isGrounded) throw new UnassignedReferenceException("moveSpeed not set" + name);
            if (!rb) throw new UnassignedReferenceException("Rigidbody not set on " + name);
            if (!groundCheck) throw new UnassignedReferenceException("Groundcheck not set on " + name);

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

    private void Fire(InputAction.CallbackContext ctx)
    {
    
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (!isGrounded) return;

        rb.AddForce(jumpSpeed * Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = (Physics.OverlapSphere(groundCheck.position, groundCheckRadius, isGroundLayer)).Length > 0;

        curMoveInput.y = rb.velocity.y;
        rb.velocity = curMoveInput;

        if (Mathf.Abs(moveDir.magnitude) > 0)
        {
            float dotProduct = Vector3.Dot(moveDir, transform.forward);

            if (dotProduct > -lookThreshole && dotProduct < lookThreshole)
            {
                float horizontalDotProduct = Vector3.Dot(Vector3.left, moveDir);
                float hValue;
                if (horizontalDotProduct < 0)
                    hValue = -1;
                else
                    hValue = 1;

                anim.SetFloat("horizontal", hValue);
                anim.SetFloat("vertical", 0);

            }

            if (dotProduct >= lookThreshole)
            {
                anim.SetFloat("horizontal", 0);
                anim.SetFloat("vertical", 1);
            }
            if (dotProduct >= lookThreshole)
            {
                anim.SetFloat("horizontal", 0);
                anim.SetFloat("vertical", 1);
            }
        }
        else
        {
            anim.SetFloat("horizontal", 0);
            anim.SetFloat("vertical", 1);
        }

     
        
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
        move.Normalize();

        moveDir = new Vector3(move.x, 0, move.y).normalized;
        curMoveInput = moveDir * moveSpeed;
    }
    
}
