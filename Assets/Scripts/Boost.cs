using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    private float boostLife;
    private const int BOOST_INITIAL_LIFE = 50;
    private const float COEF_TIME= 3.2f;
    private const float COEF_DISTANCE = 3.2f;
    private const float delay = 30f;
    // public GameObject boost;
    public static Boost instance;
    // Use this for initialization

    void Start()
    {
        boostLife = BOOST_INITIAL_LIFE;
        instance = this;
        boostLife = boostLife + Mathf.Abs(gameObject.transform.position.z)*COEF_DISTANCE;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        boostLife -= COEF_TIME * Time.deltaTime;
        //boostLife -= 0.01 * Time.deltaTime;

        if (boostLife < 0f)
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
            BoostManager.Instance.boosts.Push(gameObject);
            //StartCoroutine(Recycle());
        }
    }

    void FixedUpdate()
    {

    }

    void Recycle() {
        boostLife = BOOST_INITIAL_LIFE;
        if (gameObject.tag == "boost")
        {
            BoostManager.Instance.boosts.Push(gameObject);
        }
        else  {
            BoostManager.Instance.obstacles.Push(gameObject);
        }
        
        Debug.Log("Recycling");
    }

}
