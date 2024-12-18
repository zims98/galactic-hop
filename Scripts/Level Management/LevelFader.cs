using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFader : MonoBehaviour
{
    Animator anim;
    int levelToLoad;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void FadeToLevel(int levelIndex) // Public method which can be called from other scripts
    {
        levelToLoad = levelIndex;
        anim.SetTrigger("Fade");
    }

    public void OnFadeComplete() // Animation event - triggers at end of animation/crossfade
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
