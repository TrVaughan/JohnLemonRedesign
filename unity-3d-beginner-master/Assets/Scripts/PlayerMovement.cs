using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; 

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    public Transform teleportDesitination;
    public Transform teleportDesitination2;

    private int health;
    public TextMeshProUGUI healthText;

    private int tokens;
    public TextMeshProUGUI tokenCount;

    
    // Start is called before the first frame update
    void Start()
    {
      m_Animator = GetComponent<Animator>();
      m_Rigidbody = GetComponent<Rigidbody>();
      m_AudioSource = GetComponent<AudioSource>();

      health = 60;
      tokens = 0;
      SetHealthText();
      SetTokenCount();
    }

    // FixedUpdate is called every fixed framerate frame
    void FixedUpdate()
    {
      float horizontal = Input.GetAxis("Horizontal");
      float vertical = Input.GetAxis("Vertical");

      m_Movement.Set(horizontal, 0f, vertical);
      m_Movement.Normalize();

      bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
      bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
      bool isWalking = hasHorizontalInput || hasVerticalInput;
      m_Animator.SetBool("IsWalking", isWalking);
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

      Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
      m_Rotation = Quaternion.LookRotation(desiredForward);
    }

    void OnAnimatorMove()
    {
      m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
      m_Rigidbody.MoveRotation(m_Rotation);
    }

   void OnTriggerEnter(Collider other)
   {

      if (other.gameObject.CompareTag ("Teleport"))
		{
        transform.position = teleportDesitination.position;
		}

        if (other.gameObject.CompareTag ("Teleport2"))
		  {
        transform.position = teleportDesitination2.position;
	  	}

      if (other.gameObject.CompareTag ("Damage"))
      {
        health = health - 20;
        SetHealthText ();
        other.gameObject.SetActive (false);
      }

      if (other.gameObject.CompareTag ("Token"))
      {
        tokens = tokens +1;
        SetTokenCount();
        other.gameObject.SetActive (false);
        if (tokens == 4)
          {
            SceneManager.LoadScene("MainScene");
            //Application.Quit ();
          }
      }
    }

    void SetHealthText()
    {
      healthText.text = "Health: " + health.ToString();
    }

    void SetTokenCount()
    {
      tokenCount.text = "Tokens: " + tokens.ToString();
    }

}
