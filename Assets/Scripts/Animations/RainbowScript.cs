using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowScript : MonoBehaviour {

    public Animator anim;
    public static RainbowScript instance;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayAnimation()
    {
        anim.Play("RainbowAnimation");
    }

    public void PlaySetRuined()
    {
        anim.Play("SetRuinedAnimation");
    }


    public static RainbowScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<RainbowScript>();
            }
            return instance;
        }
    }
}
