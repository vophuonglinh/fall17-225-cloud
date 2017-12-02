﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Lean.Touch

{
    public class PlayerController : MonoBehaviour
    {

        private Rigidbody rb;
        public float speed;
        public float translateFactor = 20;
        private bool isStarted = false;     // test if game has started
        private int collisionCnt = 1000;
        private int inCloudTime = 0;
        public ParticleSystem sparkleEffect;
        public GameController gamecontroller;
        private int count;
        private int lastCount;
        private int life = 3;
        public Text countText;
        public Text highScore;
        public Text lifeText;
        private const int ALLOWED_IN_CLOUD_TIME = 500;
        private const int SPARKLE_POS_OFFSET_X = 10;
        List<ParticleSystem.Particle> exit = new List<ParticleSystem.Particle>();

        [Tooltip("Ignore fingers with StartedOverGui?")]
        public bool IgnoreGuiFingers = true;

        [Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
        public int RequiredFingerCount;

        [Tooltip("Does translation require an object to be selected?")]
        public LeanSelectable RequiredSelectable;

        [Tooltip("The camera the translation will be calculated using (default = MainCamera)")]
        public Camera Camera;

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            Start();
        }
#endif

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            count = 0;
            lastCount = count;
            SetCountText();
            GameObject gameControllerObject = GameObject.FindWithTag("GameController");
            gamecontroller = gameControllerObject.GetComponent<GameController>();

            highScore.text = PlayerPrefs.GetInt("HightScore", 0).ToString();
            Debug.Log(PlayerPrefs.GetInt("HightScore", 0));
            Debug.Log("Start!");

            if (RequiredSelectable == null)
            {
                RequiredSelectable = GetComponent<LeanSelectable>();
            }
        }


        void Update()
        {
            collisionCnt--;
            if (lastCount != count)
            {
                //boost the player speed!
                float boost = 100f;
                rb.AddForce(rb.velocity.normalized * boost * Time.deltaTime, ForceMode.Impulse);
                Debug.Log(rb.velocity.magnitude);
                lastCount = count;
            }
            if (collisionCnt >= 1000 && collisionCnt <= 2000) {
                TextMesh warning = GetComponentInChildren<TextMesh>();
                warning.color = Color.green;
            }

            if (!isStarted)
            {
                if (Input.anyKey || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    rb.AddForce(new Vector3(0.0f, 0.0f, 100f) * speed);
                    isStarted = true;
                }
            }

            else
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    transform.position += Vector3.right * speed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.position += Vector3.left * speed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    transform.position += Vector3.up * speed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    transform.position += Vector3.down * speed * Time.deltaTime;
                }
            }

            // If we require a selectable and it isn't selected, cancel translation
            if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
            {
                return;
            }

            // Get the fingers we want to use
            var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount, RequiredSelectable);

            // Calculate the scaledDelta value based on these fingers
            var scaledDelta = LeanGesture.GetScaledDelta(fingers);

            // Perform the translation
            Translate(scaledDelta);
        }
        private void LateUpdate()
        {
            Debug.Log("------------------------------"+collisionCnt);
            collisionCnt = 2000;
            Debug.Log("============================reset");
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
            collisionCnt++;
            if (other.gameObject.CompareTag("Cloud"))
            {
                //TODO the + or - 50 should be based on the direction of swipe
                Vector3 sparklePosition = gameObject.transform.position.x<0?new Vector3(gameObject.transform.position.x - SPARKLE_POS_OFFSET_X,
                    gameObject.transform.position.y, gameObject.transform.position.z+35): new Vector3(gameObject.transform.position.x + SPARKLE_POS_OFFSET_X,
                    gameObject.transform.position.y, gameObject.transform.position.z+35);
                TextMesh warning = GetComponentInChildren<TextMesh>();
                warning.color = Color.red;
                sparkleEffect.transform.position = sparklePosition;
                sparkleEffect.Play();
                if (inCloudTime >= ALLOWED_IN_CLOUD_TIME)
                {
                    endGame();
                    Debug.Log("Game Over!");
                }
            }
        }

        //Methods I found might check if it is colliding with something.
        void OnCollisionStay()
        {
            Debug.Log("-----------works");
        }
        void OnParticleTrigger(ParticleSystem other)
        {
            //if (other.gameObject.CompareTag("Cloud") && other == ParticleSystemTriggerEventType.Exit) {

            //}
            int numExit = other.GetTriggerParticles(ParticleSystemTriggerEventType.Exit, exit);
            if (numExit != 0)
            {
                Debug.Log("---------------WORKS");
            }
            else {
                Debug.Log("---------------WORKS");
            }
        }


        void endGame()
        {
            gamecontroller.GameOver();

        }

        protected virtual void Translate(Vector2 scaledDelta)
        {
            // If camera is null, try and get the main camera, return true if a camera was found
            if (LeanTouch.GetCamera(ref Camera) == true)
            {
                // Screen position of the transform
                var screenPosition = Camera.WorldToScreenPoint(transform.position);

                // Add the deltaPosition
                screenPosition += (Vector3)scaledDelta;

                // Convert back to world space
                transform.position = Camera.ScreenToWorldPoint(screenPosition);
            }
        }
    }
}
