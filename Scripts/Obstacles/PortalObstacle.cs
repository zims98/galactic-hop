using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalObstacle : MonoBehaviour
{
    [SerializeField] float speed = 4;
    public GameMaster masterScript;

    void Update()
    {
        //transform.position += Vector3.left * speed * Time.deltaTime;
        if (!masterScript.gameIsOver && masterScript.gameStarted)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
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
