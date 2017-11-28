using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    private float boostLife = 30;
    private const float coef = 3.2f;
    private const float delay = 30f;
    // public GameObject boost;
    public static Boost instance;
    // Use this for initialization

    void Start()
    {
        instance = this;
        boostLife = boostLife + Mathf.Abs(gameObject.transform.position.z)*0.06f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        boostLife -= coef * Time.deltaTime;
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
    //IEnumerator Recycle()
    //{
    //    //GetComponent<Rigidbody>.isKinematic = false;
    //    yield return new WaitForSeconds(delay);
    //    //gameObject.SetActive(false);
        
    //}

    void Recycle() {
        boostLife = 30;
        BoostManager.Instance.boosts.Push(gameObject);
        Debug.Log("Recycling");
    }

}
