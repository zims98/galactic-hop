using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] GameMaster masterScript;
    MeshRenderer mr;

    [SerializeField] float speed = 1f;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (!masterScript.gameIsOver && masterScript.gameStarted)
            mr.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0);
    }

}
