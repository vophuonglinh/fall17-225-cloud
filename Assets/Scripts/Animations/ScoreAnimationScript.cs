using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreAnimationScript : MonoBehaviour
{
    public Animator anim;
    public static ScoreAnimationScript instance;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("1"))
        //{

        //}
    }
    public void PlayAnimation()
    {
        anim.Play("ScoreAnimation");
    }

    public static ScoreAnimationScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ScoreAnimationScript>();
            }
            return instance;
        }
    }
}
