using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    Rigidbody rb;
    CapsuleCollider col;

    bool _isGrounded = true;
    bool _isSneaking = false;

    float _defaultCapsuleHeight;
    public float speed = 10f;

    public float jumpForce = 7f;
    public float gravityModifier = 1f;

    public bool IsSneaking()
    {
        return _isSneaking;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        _defaultCapsuleHeight = col.height;
        Physics.gravity *= gravityModifier;
    }



    // Update is called once per frame
    void Update()
    {
        float mH = Input.GetAxis("Horizontal");
        float mV = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(mH, 0, mV);

        rb.velocity = (movement * speed);

        IsCrouching();
        IsJumping(movement);
        
        //build up physics movement
        //rb.AddForce(movement * speed);
    }

    private void IsJumping(Vector3 movement)
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            var jumpMove = movement;
            jumpMove.y = 1f;
            jumpMove.x *= .1f;
            jumpMove.z *= .1f;


            rb.AddForce(jumpMove * jumpForce, ForceMode.Impulse);
            _isGrounded = false;
        }
    }

    private void IsCrouching()
    {
        if (Input.GetButtonDown("Crouch") && _isGrounded)
        {
            col.height = _defaultCapsuleHeight * .5f;
            _isSneaking = true;
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            col.height = _defaultCapsuleHeight;
            _isSneaking = false;
        }
    }

    private void OnCollisionEnter()
    {
        _isGrounded = true;
    }

}
