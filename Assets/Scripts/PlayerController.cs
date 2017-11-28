using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour
{

    private Rigidbody rb;
    public float speed;
    private bool isStarted = false;     // test if game has started

    public GameController gamecontroller;
    private int count;
    public Text countText;
    public Text highScore;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        gamecontroller = gameControllerObject.GetComponent<GameController>();

        highScore.text = PlayerPrefs.GetInt("HightScore", 0).ToString();
        Debug.Log(PlayerPrefs.GetInt("HightScore", 0));
        Debug.Log("Start!");
    }


    void Update()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (!isStarted)
                {                   // if game hasn't started, player touch screen
                    rb.AddForce(new Vector3(0.0f, 0.0f, 100f) * speed);
                    isStarted = true;
                }
            }
        }
    }


    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Debug.Log("collided ground!");
            endGame();
            Debug.Log("Game Over!");
        }
        if (other.gameObject.CompareTag("Boost"))
        {
            count += 1;
            SetCountText();
            transform.GetChild(0).position = other.transform.position;
            GetComponentInChildren<ParticleSystem>().Play();
            other.gameObject.SetActive(false);

        }
        if (count > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", count);
            highScore.text = count.ToString();
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
    }

    // collision that ends the game
    void OnParticleCollision(GameObject other)
    {
        // cloud collision
        if (other.gameObject.CompareTag("Cloud"))
        {
            Debug.Log("collided cloud!");
            endGame();
            Debug.Log("Game Over!");
            //Destroy(other.gameObject);
            //Destroy(gameObject);
        }
    }

    void endGame(){
        //Destroy(gameObject);
        gamecontroller.GameOver();

    }
}
