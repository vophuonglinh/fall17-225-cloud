﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public static Boost instance;
    private GameObject player;

    void Start()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag != "Lightning") {
            transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        //check if boost should be recycled
        float playerPosZ = player.transform.position.z;
        float boostPosZ = transform.position.z;
        if ((playerPosZ - boostPosZ) > 50.0)
        {
            processUselessObjects();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            processUselessObjects();
        }
    }

    void processUselessObjects() {
        gameObject.SetActive(false);
        Recycle();
    }

    void Recycle()
    {
        if (gameObject.tag == "Boost")
        {
            BoostManager.Instance.boosts.Push(gameObject);
        }
        else if (gameObject.tag == "Obstacle")
        {
            BoostManager.Instance.obstacles.Push(gameObject);
        }
        else {
            BoostManager.Instance.lightnings.Push(gameObject);
        }
    }




}
