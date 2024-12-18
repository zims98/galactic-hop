using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    // Bouncing
    [Header("Bouncing")]
    [SerializeField] float bounceForce = 6;

    // Flying
    [Header("Flying")]
    [SerializeField] float startFlyForce = 3.5f; // 3.5f
    [SerializeField] float maxFlyForce = 5f; // 3.5f
    float flyAcceleration;

    // Particles
    [Header("Particles")]
    [SerializeField] ParticleSystem deathEffect;
    [SerializeField] ParticleSystem shipExplosion;
    [SerializeField] ParticleSystem jetpackEffect;
    [SerializeField] ParticleSystem transitionEffect;
    [SerializeField] ParticleSystem starAbsorbEffect;
    [SerializeField] ParticleSystem superStarAbsorbEffect;

    // Enable/Disable Objects
    [Header("Toggle Objects")]
    [SerializeField] GameObject thrusterObject;
    [SerializeField] BoxCollider2D groundCollider;

    // Script References
    [Header("Script References")]
    [SerializeField] Score scoreScript;
    [SerializeField] BackgroundFader backgroundScript;
    [SerializeField] GameMaster masterScript;


    [Header("Sprites")] [SerializeField] Sprite bounceSprite, flySprite;

    // Component References
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator anim;
    BoxCollider2D boxCollider;

    bool isJumping;
    bool isFlying;

    [HideInInspector] public bool isInFlyMode = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        flyAcceleration = startFlyForce;
    }

    void Update()
    {
        if (!isInFlyMode) // Bounce Mode - Default
        {
            if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKeyDown(KeyCode.Mouse0)))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                AudioManager.PlayStatic("Jump");

                //rb.velocity = Vector2.up * bounceForce;
                isJumping = true;

                Instantiate(jetpackEffect, transform.position, Quaternion.Euler(90, 0, 0));
            }         
            
            // da
        }

        if (isInFlyMode) // Fly Mode
        {
            if (Input.GetKey(KeyCode.Space) || (Input.GetKey(KeyCode.Mouse0)))
            {
                //rb.velocity = Vector2.up * flyAcceleration;
                isFlying = true;

                if (flyAcceleration < maxFlyForce)
                {
                    flyAcceleration += 6 * Time.deltaTime;
                }
            }
            else
            {
                isFlying = false;
                flyAcceleration = startFlyForce;
            }


            RotatePlayer();
        }
        else isFlying = false;

        if (masterScript.gameStarted) // Unfreeze Player's Rigidbody when game has started
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }

    void FixedUpdate()
    {
        if (!isInFlyMode) // "Bounce Mode"
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("isFalling", true);
            }
            else anim.SetBool("isFalling", false);
        }

        if (isJumping)
        {
            rb.velocity = Vector2.up * bounceForce;
            isJumping = false;
        }

        if (isFlying)
        {
            rb.velocity = Vector2.up * flyAcceleration;           
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {       
        if (other.CompareTag("Scoring"))
        {
            scoreScript.IncrementScore();
            ParticleSystem starOb = Instantiate(starAbsorbEffect, other.gameObject.transform.position, Quaternion.identity);

            AudioManager.PlayStatic("Scoring");

            foreach (Transform child in other.gameObject.transform)
            {
                Destroy(child.gameObject);
            }

            starOb.transform.parent = other.transform;
        }

        if (other.CompareTag("ExtraScoring"))
        {
            scoreScript.SuperIncrementScore();
            ParticleSystem starOb = Instantiate(superStarAbsorbEffect, other.gameObject.transform.position, Quaternion.identity);

            AudioManager.PlayStatic("Scoring"); // Change?

            foreach (Transform child in other.gameObject.transform)
            {
                Destroy(child.gameObject);
            }

            starOb.transform.parent = other.transform;
        }

        if (other.CompareTag("Warp"))
        {
            scoreScript.IncrementScore();
            ParticleSystem transition = Instantiate(transitionEffect, other.gameObject.transform.position, Quaternion.identity);
            transition.transform.parent = other.transform;

            AudioManager.PlayStatic("Portal");

            if (!isInFlyMode) // Enable Fly
            {               
                isInFlyMode = true;
                Destroy(other);

                AudioManager.PlayStatic("Thrusters");

                boxCollider.size = new Vector2(0.815f, 0.32f);
                boxCollider.offset = new Vector2(0f, -0.03f);

                thrusterObject.SetActive(true);
                groundCollider.enabled = false;

                anim.SetBool("inSpace", true);
                sr.sprite = flySprite;
                backgroundScript.fadeOut = true;
            }
            
            else if (isInFlyMode) // Disable Fly
            {
                transform.rotation = Quaternion.identity; // Reset rotation
                isInFlyMode = false;
                Destroy(other);

                AudioManager.StopStatic("Thrusters");

                boxCollider.size = new Vector2(0.46f, 0.88f);
                boxCollider.offset = new Vector2(0f, 0f);

                thrusterObject.SetActive(false);
                groundCollider.enabled = true;

                anim.SetBool("inSpace", false);
                sr.sprite = bounceSprite;              
                backgroundScript.fadeIn = true;
            }           
        }
    }

    void RotatePlayer()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg; //Set the angle

        angle = Mathf.Clamp(angle, -35, 35); // Limit the angle

        Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward); // Declare the target rotation using the desired angle

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 2 * Time.deltaTime); // Lerp the rotation to its target rotation over time
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            if (isInFlyMode)
            {
                Instantiate(shipExplosion, transform.position, Quaternion.identity);
                AudioManager.PlayStatic("Explosion");
            }
            else
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
                AudioManager.PlayStatic("Splatter");
            }

            AudioManager.StopStatic("Thrusters");          

            masterScript.GameOver();

            Destroy(gameObject);           
        }
    }
}
