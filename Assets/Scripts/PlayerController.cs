using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidad = 5f;

    public float jumpForce = 5f;
    
    public float rotateSpeed = 5f;

    public bool isGrounded;

    private Rigidbody rb;

    public Animator playerAnimator;


    void Start()
    {

        rb = GetComponent<Rigidbody>();

        playerAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        //Movimiento
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");
        Vector3 movimiento = new Vector3(movimientoHorizontal, 0.0f, movimientoVertical) * velocidad * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movimiento);
        
        //afecta los datos del animator. Le envía datos al parametro Speed
        playerAnimator.SetFloat("Speed", Mathf.Abs( movimiento.x) + Mathf.Abs(movimiento.z)) ;


       
    }

    private void Update()
    {
        //Salto
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

           
        }

        //Afecta el grounded para saber cuando está en el suelo
        playerAnimator.SetBool("Grounded", isGrounded);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grounded"))
        {
           
            isGrounded = true;
          
        }
    }
}
