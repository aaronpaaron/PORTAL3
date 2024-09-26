using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : PortalTraveller {
    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float smoothMoveTime = 0.1f;
    public float jumpForce = 8;
    public float gravity = 18;

    public bool lockCursor;
    public float mouseSensitivity = 5;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float rotationSmoothTime = 0.03f;

    CharacterController controller;
    Camera cam;
    public float yaw;
    public float pitch;
    float smoothYaw;
    float smoothPitch;

    float yawSmoothV;
    float pitchSmoothV;
    float verticalVelocity;
    Vector3 velocity;
    Vector3 smoothV;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public Animator animator;

    PickupPortalGun pickupPortalGun;

    bool jumping;
    float lastGroundedTime;
    bool disabled;

    // Äänen toistamiseen tarvittavat muuttujat
    public AudioClip walkSound; // Kävelyääni
    public AudioClip runSound;  // Juoksuääni

    // Äänien hallintaan liittyvät muuttujat
    private AudioSource walkAudioSource; // AudioSource kävelyääneelle
    private AudioSource runAudioSource;  // AudioSource juoksuääneelle

    void Start () {
        cam = Camera.main;
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        controller = GetComponent<CharacterController> ();

        yaw = transform.eulerAngles.y;
        pitch = cam.transform.localEulerAngles.x;
        smoothYaw = yaw;
        smoothPitch = pitch;
        Cursor.visible = false;

        // Luo AudioSource-komponentit kävely- ja juoksuäänille
        walkAudioSource = gameObject.AddComponent<AudioSource>();
        walkAudioSource.clip = walkSound;
        walkAudioSource.loop = true; // Jatkuva toisto

        runAudioSource = gameObject.AddComponent<AudioSource>();
        runAudioSource.clip = runSound;
        runAudioSource.loop = true; // Jatkuva toisto
    }

      public void AdjustSpeed(float newSpeed)
    {
        mouseSensitivity = newSpeed;
        //Debug.Log("Mouse sensitivity changed to: " + mouseSensitivity);
    }

    void Update () 
    {
        PlayerPrefs.SetFloat("currentSensitivity", mouseSensitivity);
         
             if (!PauseMenu.isPaused)
        {
            Cursor.visible = false;
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Break();
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            disabled = !disabled;
        }

        if (disabled) {
            return;
        }
        {
          float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;
        Vector3 worldInputDir = transform.TransformDirection(inputDir);

        float currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed;
        Vector3 targetVelocity = worldInputDir * currentSpeed;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothV, smoothMoveTime);

        verticalVelocity -= gravity * Time.deltaTime;
        velocity = new Vector3(velocity.x, verticalVelocity, velocity.z);

        var flags = controller.Move(velocity * Time.deltaTime);
        if (flags == CollisionFlags.Below) {
            jumping = false;
            lastGroundedTime = Time.time;
            verticalVelocity = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            float timeSinceLastTouchedGround = Time.time - lastGroundedTime;
            if (controller.isGrounded || (!jumping && timeSinceLastTouchedGround < 0.15f)) {
                jumping = true;

                if (PickupPortalGun.equippedWeapon == PickupPortalGun.Weapon.PortalGun) {
                    animator = GameObject.FindWithTag("PortalGun").GetComponent<Animator>();
                    animator.Rebind();
                    animator.SetTrigger("isJumping");
                }

                verticalVelocity = jumpForce;
            }
        }

        float mX = Input.GetAxisRaw("Mouse X");
        float mY = Input.GetAxisRaw("Mouse Y");

        yaw += mX * mouseSensitivity;
        pitch -= mY * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
        smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);



        transform.eulerAngles = Vector3.up * smoothYaw;
        cam.transform.localEulerAngles = Vector3.right * smoothPitch;

        // Soita ääntä pelaajan liikkeen mukaan
        PlayMovementSound(inputDir);
    }

    void PlayMovementSound(Vector3 inputDir) {
        if (controller.isGrounded) { // Tarkista, onko pelaaja maassa
            if (inputDir.magnitude > 0) { // Pelaaja liikkuu
                if (Input.GetKey(KeyCode.LeftShift)) { // Pelaaja juoksee
                    if (!runAudioSource.isPlaying) {
                        walkAudioSource.Stop(); // Pysäytä kävelyääni
                        runAudioSource.Play(); // Soita juoksuääni
                    }
                } else { // Pelaaja kävelee
                    if (!walkAudioSource.isPlaying) {
                        runAudioSource.Stop(); // Pysäytä juoksuääni
                        walkAudioSource.Play(); // Soita kävelyääni
                    }
                }
            } else {
                // Pelaaja ei liikku, pysäytä kaikki äänet
                walkAudioSource.Stop();
                runAudioSource.Stop();
            }
        } else {
            // Pelaaja ei ole maassa, pysäytä kaikki äänet
            walkAudioSource.Stop();
            runAudioSource.Stop();
        }
    }

    public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        transform.position = pos;
        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle(smoothYaw, eulerRot.y);
        yaw += delta;
        smoothYaw += delta;
        transform.eulerAngles = Vector3.up * smoothYaw;
        velocity = toPortal.TransformVector(fromPortal.InverseTransformVector(velocity));
        Physics.SyncTransforms();
    }

    public void SetGunAnimatorComponent(Animator gunAnimator) {
        animator = gunAnimator;
    }
}
