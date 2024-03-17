using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayer : MonoBehaviour
{
    public bool doubleJump = false;
   
    public float moveSpeed;
    
    public float jumpForce;
    
    public float garavityScale = 5f;

    public float rotateSpeed = 5f;
    
    private Vector3 moveDirection;

    public CharacterController charController;
    // Trae la cámara
    public Camera playerCamera;

    //trae al Player
    public GameObject playerModel;

    public Animator animator;

    public ParticleSystem particleWalk;

    //Flag fuction audio && particles
    private bool functionAudioWalk = false;

    private bool functionAudioRun = false;


    private float rangeSpeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float yStore = moveDirection.y;


        if (Input.GetKey(KeyCode.LeftShift)){
            moveSpeed = 10;
        }
        else
        {
            moveSpeed = 3;
        }

        //Movimiento       
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
            moveDirection.Normalize();
            moveDirection *= moveSpeed;
            moveDirection.y = yStore;

            charController.Move(moveDirection * Time.deltaTime);

            rangeSpeed = (Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));


        //Sound Walk, Run and Particles
        if (rangeSpeed > 0 && rangeSpeed < 12 && moveSpeed == 3 && charController.isGrounded)
        {
            particleWalk.gameObject.SetActive(true);
           
            AudioManager.instance.StopSFX(1);
            
            if (!functionAudioWalk)//Check Flag
            {

                AudioWalk();

                functionAudioWalk = true;// Flag Walk

                functionAudioRun = false;
            }
        }
       
        if (rangeSpeed > 12 && moveSpeed == 10 && charController.isGrounded)
        {

            AudioManager.instance.StopSFX(2);

            if (!functionAudioRun)// Flag Walk
            {

                AudioRun();

                functionAudioRun = true;// Flag Walk

                functionAudioWalk = false;
            }

        }
      
        if (rangeSpeed <= 0 || charController.isGrounded == false)
        {
            particleWalk.gameObject.SetActive(false);
            
            functionAudioWalk = false;// Flag Walk
            functionAudioRun = false;
            AudioManager.instance.StopSFX(2);
            AudioManager.instance.StopSFX(1);
        }


        //Salto

        if (charController.isGrounded)
        {
            moveDirection.y = -1f;

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
                doubleJump = true;
                
                //audio jump
                AudioManager.instance.PlaySFX(0);

            }
        }
        else if (Input.GetButtonDown("Jump") && doubleJump == true)
        {
            moveDirection.y = jumpForce;
            doubleJump = false;

            //audio jump
            AudioManager.instance.PlaySFX(0);
        }


        //Gravedad
        moveDirection.y += Physics.gravity.y * Time.deltaTime * garavityScale;


        //Solo rota si hay movimiento del Player
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            //El player rota con la cámara
            transform.rotation = Quaternion.Euler(0f, playerCamera.transform.rotation.eulerAngles.y, 0f);

            //El player rotahacia la dirección a donde camina
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));

            //Rota suavemente
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);

        }

        //afecta los datos del animator. Le envía datos al parametro Speed
        animator.SetFloat("Speed", Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        //Afecta el grounded para saber cuando está en el suelo
        animator.SetBool("Grounded", charController.isGrounded);

    }

    void AudioWalk()
    { 
        //Audio walk
        AudioManager.instance.PlaySFX(2);
    }

    void AudioRun()
    {
        //Audio Run
        AudioManager.instance.PlaySFX(1);
    }

}
