using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 8f;
    private Rigidbody2D rb;
    public float smoothTime = 0.2f;
    public float jumpForce = 10f;
    public float maxY = 20f;
    private float moveInput;
    private bool isGrounded;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float radius = 0.02f;
    public LayerMask Ground;

    [Header("Particle System")]
    public ParticleSystem smokeFx;
    public ParticleSystem jumpFx;
    public int jumpEmitCount = 10;

    [Header("Audio System")]
    public AudioSource src;
    public AudioClip jump, hit, collect;

    [Header("Other")]
    public Transform spawnPoint;
    public float smoothReset = 0.02f;

    private int layerIndex;

    private bool facingRight = true;

    private ParticleSystem.EmissionModule smokeEmission;
    private bool smokeInitialized = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        layerIndex = LayerMask.NameToLayer("Objects");

        gameObject.transform.position = spawnPoint.position;

        if (smokeFx != null)
        {
            var main = smokeFx.main;
            main.loop = true;
            
            smokeEmission = smokeFx.emission;
            smokeEmission.enabled = false;
            smokeInitialized = true;
        }
        else
        {
            Debug.LogWarning("smokeFx není přiřazený v Inspectoru");
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        //SKOK
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, Ground);
    
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            
            jumpFx.Play();

            src.clip = jump;
            src.Play();
        }
    
        //FLIPOVANI SPRITU

        if(moveInput > 0 && !facingRight)
        {
            Flip();
        }

        if(moveInput < 0 && facingRight)
        {
            Flip();
        }
    
        //MAX FALL
        if(rb.linearVelocity.y < maxY)
        {
            
        }


        bool movingOnGround = isGrounded && Mathf.Abs(moveInput) > 0.01f;

        if (smokeInitialized)
        {
            if (movingOnGround)
            {
                if (!smokeEmission.enabled)
                {
                    smokeEmission.enabled = true;
                    if (!smokeFx.isPlaying) smokeFx.Play();
                }
            }
            else
            {
                if (smokeEmission.enabled)
                {
                    smokeEmission.enabled = false;
                }
            }
        }

    }

    private void FixedUpdate()
    {
        float targetSpeed = moveInput * speed;
        float smoothSpeed = Mathf.Lerp(rb.linearVelocity.x, targetSpeed, smoothTime);
    
        rb.linearVelocity = new Vector2(smoothSpeed, rb.linearVelocity.y);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    public void HitByProjectile()
    {
        if (src != null && hit != null)
        {
            src.clip = hit;
            src.Play();
        }
    }

    public void NpcCollected()
    {
        if (src != null && collect != null)
        {
            src.clip = collect;
            src.Play();
        }
    }

}

