using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    AudioSource m_AudioSource;

    Animator m_Animator;

    public InputAction MoveAction;

    public float walkSpeed = 1.0f;
    public float turnSpeed = 20f;


    // Stamina variables
    public Image StaminaBar;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDecreaseRate = 10f;
    public float staminaIncreaseRate = 5f;

    // Running mechanic(speed boost as described in the assignment)
    public bool isRunning = false;

    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_Rigidbody = GetComponent<Rigidbody>();
        MoveAction.Enable();
        m_Animator = GetComponent<Animator>();
    }

    void Update()
    {
        //If statement for running mechanic
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentStamina > 0)
        {
            isRunning = true;
            walkSpeed = 2.0f; // Increase speed when running
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            walkSpeed = 1.0f; // Reset speed when not running
        }


        if (isRunning && currentStamina > 0)
    {
        currentStamina -= staminaDecreaseRate * Time.deltaTime;

        // If stamina runs out, stop running
        if (currentStamina <= 0)
        {
            currentStamina = 0;
            isRunning = false;
            walkSpeed = 1.0f;
        }
    }
    else
    {
        // Recover stamina when NOT running
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaIncreaseRate * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
        }
    }

    // Update UI bar
    StaminaBar.fillAmount = currentStamina / maxStamina;

    }

    void FixedUpdate()
    {
        var pos = MoveAction.ReadValue<Vector2>();

        float horizontal = pos.x;
        float vertical = pos.y;

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        m_Rigidbody.MoveRotation(m_Rotation);
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * walkSpeed * Time.deltaTime);

        

         // Handle footstep audio

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }
    }
}
