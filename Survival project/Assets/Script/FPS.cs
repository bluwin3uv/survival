using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GGL; // manny's plugin

public class FPS : MonoBehaviour
{
    //public GameObject target;
    public float moveSpeed; // player walk speed.
    public float jumpForce; // how high the player jumps.
    public float mouseSensitivity; // mouse sensitivity.
    public GameObject bullet; // bullet prefab
    public Transform bulletPoint;
    public bool foundTarget;
    [Header("GUI")] // gui header.
    public Texture2D targetTexture; // applying texure 
  //  public Texture2D foundTexture;
    public Rect targetLocation; // where the texure is located
    private bool locked; // 
    private float pitch; // rotation of x axis
    private float yaw; // rotation of y axis
    private GameObject camm; // the FPS camera
    public float gravity = 14f; // gravity 
    private CharacterController controller; // character controller
    private Vector3 movement; // player movement

    void Start()
    {
        controller = GetComponent<CharacterController>(); // getting the charecter contoller
        camm = GameObject.Find("Main Camera"); // finding the main camera
    }

    void Update()
    {
        Movement(); // movement function
        if(locked) // the camera only rotates when locked is checked
        {
            CameraRotation(); // calls the camera rotation funtion
        }
        Lock(); // locked funtion
       // Shoot();
    }

    void Shoot()
    {
        RaycastHit hit;
        Debug.DrawRay(camm.transform.position, camm.transform.forward, Color.blue);
        if(Physics.Raycast(camm.transform.position, camm.transform.forward,out hit))
        {
            if(hit.collider.tag == "Target")
            {
                foundTarget = true;
            }
            else 
            {
                foundTarget = false;
            }
        }
       
        if (Input.GetMouseButtonDown(0)|| Input.GetButtonDown("Fire2"))
        {
            Instantiate(bullet,bulletPoint.transform.position,bulletPoint.transform.rotation);
        }
    }
         

    void OnGUI()
    {
        float scrW = Screen.width / 16;
        float scrH = Screen.height / 9;
        GUI.DrawTexture(new Rect(scrW * targetLocation.x, scrH * targetLocation.y, scrW * targetLocation.width, scrH * targetLocation.height), targetTexture);
    }

    void Movement()
    {
        if (controller.isGrounded)
        {   // setting the arrow keys inputs 
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            movement = new Vector3(h, 0, v); // defining movement
            movement = transform.TransformDirection(movement); // transform direction
            movement *= moveSpeed; // mutipliy by the move speed 
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1")) 
            {
                movement.y = jumpForce; // jumping
            }
        }
        movement.y -= gravity * Time.deltaTime; // created gravity
        controller.Move(movement * Time.deltaTime); 
    }

    void CameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        // incresing or decreasing values using mouse
        pitch -= mouseY * mouseSensitivity;
        yaw += mouseX * mouseSensitivity;
        camm.transform.localEulerAngles = new Vector3(pitch, 0, 0); // rotating camera up/down
        transform.eulerAngles = new Vector3(0, yaw, 0); //rotating player left/ right
        // locks the camera looking or down
        if(pitch > 90)
        {
            pitch = 90;
        }
        if(pitch < - 90)
        {
            pitch = -90;
        }
    }

    void Hide(bool isHiding) 
    {
        if(isHiding)
        {   
            Cursor.lockState = CursorLockMode.Locked;// locks cursor in the middle of the screen;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void Lock()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Hide(locked);
            locked = false;
        }

        if(Input.GetMouseButtonDown(0))
        {
            locked = true;
            Hide(true);
        }
    }
}
