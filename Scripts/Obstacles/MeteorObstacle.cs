using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorObstacle : MonoBehaviour
{
    [SerializeField] float speed = 4;
    public GameMaster masterScript;
    public Player playerScript;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //transform.position += Vector3.left * speed * Time.deltaTime;
        if (!masterScript.gameIsOver && masterScript.gameStarted)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (playerScript.isInFlyMode)
        {
            anim.SetTrigger("Meteor_Fade_In");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ObstacleDestroyer"))
        {
            Destroy(gameObject);
        }
    }
}
