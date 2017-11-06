using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour {
    public float boostLife = 15;
    private const float coef = 0.5f;
    private const float delay = 6f;
    //public GameObject boost;
    public static Boost instance;
    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        boostLife -= coef * Time.deltaTime;
        if (boostLife <= 0)
        {

            StartCoroutine(Recycle());
            Debug.Log("TRASH1");
            gameObject.active = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("TRASH2");
            StartCoroutine(Recycle());
        }
    }

    private void FixedUpdate()
    {

    }
    public float life
    {
        get { return boostLife; }
        set { boostLife = value; }
    }

    IEnumerator Recycle()
    {
        //		yield return new WaitForSeconds(fallDelay);
        //GetComponent<Rigidbody>.isKinematic = false;
        yield return new WaitForSeconds(delay);
        BoostManager.Instance.boosts.Push(gameObject);
        //Debug.Log("TRASH");
    }

}