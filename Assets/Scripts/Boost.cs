using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    private float boostLife;
    private const int BOOST_INITIAL_LIFE = 50;
    private const float COEF_DECREASE_TIME= 3.2f;
    private const float COEF_DISTANCE = 3.2f;
    //private const float delay = 30f;
    // public GameObject boost;
    public static Boost instance;
    // Use this for initialization
    private GameObject player;

    void Start()
    {
        boostLife = BOOST_INITIAL_LIFE;
        instance = this;
        boostLife = boostLife + Mathf.Abs(gameObject.transform.position.z)*COEF_DISTANCE;

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        boostLife -= COEF_DECREASE_TIME * Time.deltaTime;


        //check if player's Z position is greater than boost's position, push onto stack
        float playerPosZ = player.transform.position.z;
        float boostPosZ = transform.position.z;
        if ((playerPosZ - boostPosZ) > 50.0)
        {
            gameObject.SetActive(false);
            Recycle();
            BoostManager.Instance.boosts.Push(gameObject);
        }
        
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            processUselessBoosts();
        }
    }

    void processUselessBoosts() {
        gameObject.SetActive(false);
        Recycle();
        BoostManager.Instance.boosts.Push(gameObject);
    }

    void Recycle()
    {
        boostLife = BOOST_INITIAL_LIFE;
        if (gameObject.tag == "boost")
        {
            BoostManager.Instance.boosts.Push(gameObject);
        }
        else
        {
            BoostManager.Instance.obstacles.Push(gameObject);
        }        
    }

    void FixedUpdate()
    {

    }


}
