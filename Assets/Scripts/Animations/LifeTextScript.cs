using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTextScript : MonoBehaviour {

    public Animator anim;
    public static LifeTextScript instance;

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
        anim.Play("LifeAnimation");
    }

    public static LifeTextScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<LifeTextScript>();
            }
            return instance;
        }
    }
}
