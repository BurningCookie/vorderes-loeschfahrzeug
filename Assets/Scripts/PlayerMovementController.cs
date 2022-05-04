using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerMovementController : NetworkBehaviour
{
    public float speed;
    private float gravity = 1f;
    public CharacterController crcon;
    public GameObject PlayerModel;
    public GameObject PlayerObject;
    public GameObject own_camera;
    public GameObject own_glasses;
    public Transform groundcheck;
    public float distancetoground;
    public LayerMask groundLayerMask;
    public MouseLook MouseLook;
    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        PlayerModel.SetActive(false);
        own_camera.SetActive(false);
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name == "Game")
        {
            if(PlayerModel.activeSelf == false)
            {
                PlayerModel.SetActive(true);
            }

            if(isLocalPlayer)
            {
                ToggleOwnObjects();
                SetPosition();
            }
            
            if(hasAuthority)
            {
                Movement();
                MouseLook.MouseMovement();
            }
        }
    }

    private void ToggleOwnObjects()
    {
        own_camera.SetActive(true);
        own_glasses.SetActive(false);
    }

    public void SetPosition()
    {
        PlayerObject.transform.position = new Vector3(Random.Range(-5, 5), 2f, Random.Range(-5, 5));
    }

    public void Movement()
    {
        isGrounded = Physics.CheckSphere(groundcheck.position, distancetoground, groundLayerMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        crcon.Move(move * speed * Time.deltaTime);
        velocity.y -= gravity * Time.deltaTime;
        crcon.Move(velocity);
    }
}
