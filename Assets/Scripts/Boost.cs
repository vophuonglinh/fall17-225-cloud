using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    private float boostLife = 30;
    private const float coef = 0.6f;
    private const float delay = 30f;
    // public GameObject boost;
    public static Boost instance;
    // Use this for initialization

    void Start()
    {
        instance = this;
        boostLife += gameObject.transform.position.y*0.2f + Mathf.Abs(gameObject.transform.position.x)*0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        boostLife -= coef * Time.deltaTime;

        if (boostLife < 0)
        {
            Debug.Log("wrong");
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

    private void FixedUpdate()
    {

    }
    public float life
    {
        get { return boostLife; }
        set { boostLife = value; }
    }

    //IEnumerator Recycle()
    //{
    //    //GetComponent<Rigidbody>.isKinematic = false;
    //    yield return new WaitForSeconds(delay);
    //    //gameObject.SetActive(false);
        
    //}

    void Recycle() {
        BoostManager.Instance.boosts.Push(gameObject);
        Debug.Log("Recycling");
    }

}
