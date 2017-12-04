using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    //private const float delay = 30f;
    // public GameObject boost;
    public static Boost instance;
    // Use this for initialization
    private GameObject player;

    void Start()
    {
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);        
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

    void FixedUpdate()
    {

    }

    private void LateUpdate()
    {
        //check if player's Z position is greater than boost's position, push onto stack
        float playerPosZ = player.transform.position.z;
        float boostPosZ = transform.position.z;
        if ((playerPosZ - boostPosZ) > 50.0)
        {
            processUselessObjects();
        }

    }


}
