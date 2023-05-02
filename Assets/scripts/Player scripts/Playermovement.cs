using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Playermovement : MonoBehaviour
{

    public CharacterController controller;
    GameObject model;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    Vector3 velocity;
    Vector3 moveDir;

    public float health;

    Rigidbody rb;
    Animator anim;

    public float lookThreshole = 0.6f;

    Plane plane = new Plane(Vector3.up, 0);
    public Transform groundCheck;
    public float groundCheckRadius = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;
    int errorCounter = 0;
  

    private void Start()
    {
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


    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

       


    }
    public void TakeDamge(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyPlayer), 0.5f);
    }

    private void DestroyPlayer()
    {
        Destroy(gameObject);
    }
}
