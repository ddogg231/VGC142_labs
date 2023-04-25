using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class topdownplayercontroller : MonoBehaviour
{

    public float moveSpeed = 10.0f;
    public float jumpSpeed = 500.0f;

    Rigidbody rb;

    Vector3 curMoveInput;
    Vector3 moveDir;

    Plane plane = new Plane(Vector3.up, 0);

    public LayerMask isGroundLayer;
    public float groundCheckRadius;
    public Transform groundCheck;
    public bool isGrounded;

    int errorCounter = 0;
    public float range;


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
             
            rb = GetComponent<Rigidbody>();
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


        GameManager.Instance.playerActions.Player.Move.performed += ctx => Move(ctx);
        GameManager.Instance.playerActions.Player.Move.canceled += ctx => Move(ctx);

        GameManager.Instance.playerActions.Player.Jump.performed += ctx => Jump(ctx);
        GameManager.Instance.playerActions.Player.Fire.performed += ctx => Fire(ctx);
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

        float distance;
        Ray mouseRay = GameManager.Instance.MousePos();
        Vector3 worldPos;

        if (plane.Raycast(mouseRay, out distance))
        {
            worldPos = mouseRay.GetPoint(distance);

            Vector3 relativePos = worldPos - transform.position;

            relativePos.y = 0;

            transform.rotation = Quaternion.LookRotation(relativePos);
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
