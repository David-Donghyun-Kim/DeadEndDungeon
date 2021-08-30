using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region <Variables>
    CharacterController controller;
    Animator anim;

    Vector2 movementInput;
    Vector3 movementDirection;
    Vector3 cameraForward;
    public float rotateSpeed;
    public float moveSpeed;
    public float gravity;
    public float jumpSpeed;

    private float speed = 0f; // for animation change idle->walk->run - not using now
    private float velocity;
    private float desiredRotationAngle = 0;
    private float _movementDirectionY = -5f; // for jump function

    private bool isRolling = false;
    private bool isJumping = false;
    private bool isAttacking = false;
    private bool isChangingWeapon = false;

    public bool cursorLocked;
    #endregion

    #region - Start -
    void Start()
    {
        // Lock user's cursor to the center of the screen.
        if (cursorLocked)
            Cursor.lockState = CursorLockMode.Locked;

        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

    }
    #endregion

    #region - Update -
    void Update()
    {
        GetInput();
        //GetSpeed();
        GetMovementDirection();
        //RotateCharacter_Test();
        //RotateCharacter();
        Roll();
        Attack();
        Interact();
        MoveCharacter();
    }
    #endregion

    #region - Roll -
    private void Roll()
    {
        if (Input.GetButtonDown("Roll") && movementInput.magnitude > 0.1 && !isRolling && controller.isGrounded)
        {
            // Roll animation should not care about the exact player movement input.
            // The only thing I need is just the direction of rolling.
            isRolling = true;
            int rollX = 0, rollY = 0;
            if (movementInput.x < 0)
                rollX = Mathf.FloorToInt(movementInput.x);
            else
                rollX = Mathf.CeilToInt(movementInput.x);
            if (movementInput.y < 0)
                rollY = Mathf.FloorToInt(movementInput.y);
            else
                rollY = Mathf.CeilToInt(movementInput.y);

            anim.SetFloat("RollX", (float)rollX);
            anim.SetFloat("RollY", (float)rollY);
            anim.SetTrigger("doRoll");

            // Rolling delay - the number is rolling cool-down time
            Invoke("RollOut", 0.5f);
        }
    }
    private void RollOut()
    {
        isRolling = false;
    }
    #endregion

    #region - Jump -
    private void Jump()
    {
        if(Input.GetButtonDown("Jump") && controller.isGrounded && !isRolling && !isJumping)
        {
            isJumping = true;
            movementDirection.y = jumpSpeed;
            anim.SetBool("isJumping", true);
            anim.SetTrigger("doJump");
        }
        else if(controller.isGrounded)
        {
            anim.SetBool("isJumping", false);
            Invoke("JumpOut", 0.1f);
        }
    }

    private void JumpOut()
    {
        isJumping = false;
    }
    #endregion

    #region - Interact -
    private void Interact()
    {
        if(Input.GetButtonDown("Interact")) // I'll add more contrain. For just, when user press 'e' button
        {
            Debug.Log("User pressed e");

            // Call your pickup function here!
        }
    }
    #endregion

    #region - Attack -
    private void Attack()
    {
        if (Input.GetButtonDown("Attack") && !isAttacking && !isRolling &&controller.isGrounded)
        {
            isAttacking = true;
            anim.SetTrigger("doAttack");

            Invoke("AttackOut", 0.5f);
        }
    }

    private void AttackOut()
    {
        isAttacking = false;
    }

    public bool GetAttackingState()
    {
        return isAttacking;
    }
    #endregion

    #region - Movement - 
    private void MoveCharacter()
    {
        if (movementInput.magnitude > 0.1) //When player move
        {
            anim.SetBool("isMoving", true);
            anim.SetFloat("x", movementInput.x);
            anim.SetFloat("y", movementInput.y);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
        movementDirection.x *= moveSpeed;
        movementDirection.z *= moveSpeed;

        controller.Move(movementDirection * Time.deltaTime);
    }
    #endregion

    #region - Movement Direction -
    private void GetMovementDirection()
    {
        cameraForward = Camera.main.transform.TransformDirection(Vector3.forward);
        cameraForward.y = 0;
        Debug.DrawRay(Camera.main.transform.position, cameraForward * 10, Color.red);
        var right = Camera.main.transform.TransformDirection(Vector3.right);
        if (controller.isGrounded)
        {
            movementDirection = (movementInput.x * right + movementInput.y * cameraForward).normalized;
            movementDirection.y = 0;

            Jump(); // Call Jump Function
        }
        else if(!controller.isGrounded)
        {
            movementDirection = (movementInput.x * right + movementInput.y * cameraForward).normalized;
            movementDirection.y = _movementDirectionY;
        }
        movementDirection.y -= gravity * Time.deltaTime;
        _movementDirectionY = movementDirection.y;

        // Player looks toward the camera when user press move button(w,s,a,d)
        if (movementInput.magnitude != 0 || isJumping)
            transform.forward = cameraForward.normalized;
        Debug.DrawRay(Camera.main.transform.position, movementDirection * 10, Color.blue);
    }
    #endregion

    #region - Input -
    private void GetInput()
    {
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    #endregion

    #region *** Not Using Now ***
    #region - Animation Blender Tree Speed -
    private void GetSpeed()
    {
        speed = Mathf.Abs(movementInput.x) + Mathf.Abs(movementInput.y);
        speed = Mathf.Clamp(speed, 0f, 1f);
        speed = Mathf.SmoothDamp(anim.GetFloat("Speed"), speed, ref velocity, 0.1f);
        anim.SetFloat("Speed", speed);
    }
    #endregion

    #region - Character Rotation -
    private void RotateCharacter_Test()
    {

        desiredRotationAngle = Vector3.Angle(transform.forward, movementDirection);
        var crossProduct = Vector3.Cross(transform.forward, movementDirection).y;
        if (crossProduct < 0)
        {
            desiredRotationAngle *= -1;
        }

        if (desiredRotationAngle > 10 || desiredRotationAngle < -10)
        {
            transform.Rotate(Vector3.up * desiredRotationAngle * rotateSpeed * Time.deltaTime);
        }

    }
    private void RotateCharacter()
    {
        if (movementInput != Vector2.zero && movementDirection.magnitude > 0.1f)
        {

            Quaternion freeRotation = Quaternion.LookRotation(movementDirection, transform.up);
            var diferenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
            var eulerY = transform.eulerAngles.y;
            if (diferenceRotation < 0 || diferenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
            var euler = new Vector3(0, eulerY, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), rotateSpeed * Time.deltaTime);

        }
    }
    #endregion
    #endregion
}
