using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float walkSpeed = 1f;
    public float sprintDuration = 5f;
    private float sprintCap; 

    public GameObject sprintIcon;
    public GameObject sprintBar;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    void Start ()
    {
        sprintCap = sprintDuration;
        m_Animator = GetComponent<Animator> ();
        m_Rigidbody = GetComponent<Rigidbody> ();
        m_AudioSource = GetComponent<AudioSource> ();
    }

    void FixedUpdate ()
    {
        //Sprinting Mod
        if (Input.GetKey(KeyCode.LeftShift) && sprintDuration > 0) 
        {
            walkSpeed = 2f;
            Debug.Log("Left Shift key was pressed");
            sprintIcon.SetActive(true);
            sprintBar.SetActive(true);

            sprintDuration -= 1 * Time.deltaTime;
        }
        else
        {
            walkSpeed = 1f;
            if (sprintDuration < sprintCap)
            {
                sprintDuration += 0.5f * Time.deltaTime;
            }
            sprintIcon.SetActive(false);
            sprintBar.SetActive(false);
        }

        sprintBar.GetComponent<Slider>().value = sprintDuration / sprintCap;

        //Sprinting Mod

        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);
        
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop ();
        }

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
    }

    void OnAnimatorMove ()
    {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * walkSpeed * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation (m_Rotation);
    }
}