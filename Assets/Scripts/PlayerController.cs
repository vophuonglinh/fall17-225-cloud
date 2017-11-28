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
    private int inCloudTime = 0;
    public ParticleSystem sparkleEffect;
    public GameController gamecontroller;
    private int count;
    private int life = 3;
    public Text countText;
    public Text highScore;
    public Text lifeText;
    private const int ALLOWED_IN_CLOUD_TIME = 500;
    private const int SPARKLE_POS_OFFSET_X = 50;
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
        if (life <= 0)
        {
            endGame();
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
        if (other.gameObject.CompareTag("Obstacle"))
        {
            life -= 1;
            SetLifeText();
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

    void SetLifeText()
    {
        lifeText.text = "Life: " + life.ToString();
    }

    // collision that ends the game
    void OnParticleCollision(GameObject other)
    {
        // cloud collision
        inCloudTime++;
        if (other.gameObject.CompareTag("Cloud"))
        {
            //TODO the + or - 50 should be based on the direction of swipe
            Vector3 sparklePosition = new Vector3(gameObject.transform.position.x - SPARKLE_POS_OFFSET_X, 
                gameObject.transform.position.y,gameObject.transform.position.z);
            sparkleEffect.transform.position = sparklePosition;
            sparkleEffect.Play();
            Debug.Log("collided cloud!");
            if (inCloudTime >= ALLOWED_IN_CLOUD_TIME) {
                endGame();
                Debug.Log("Game Over!");
            }
        }
    }

    void endGame(){
        gamecontroller.GameOver();

    }
}
