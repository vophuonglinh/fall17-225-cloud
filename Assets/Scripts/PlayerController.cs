using System.Collections;
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
        private bool isStarted = false;
        private int inCloudTime = 0;
        public ParticleSystem sparkleEffect;
        //public ParticleSystem blastEffect;
        public GameController gamecontroller;
        private int count;
        private int lastCount;
        private int life = 3;
        public Text countText;
        public Text highScore;
        public Text lifeText;
        public Text Timer;
        ParticleSystem blast;
        private int cloudcollisioncount = 0;
        
        private const int ALLOWED_IN_CLOUD_TIME = 10;
        private const int SPARKLE_POS_OFFSET_X = 10;

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
            //blast = GetComponentInChildren<ParticleSystem>();
            Timer.enabled = false;
            if (RequiredSelectable == null)
            {
                RequiredSelectable = GetComponent<LeanSelectable>();
            }
        }


        void Update()
        {
            if (lastCount != count)
            {
                //boost the player speed!
                float boost = 100f;
                rb.AddForce(rb.velocity.normalized * boost * Time.deltaTime, ForceMode.Impulse);
                lastCount = count;
            }
            resetTimeOutofCloud(checkOutOfCloud());
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

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                gamecontroller.GameOver();
            }
            if (other.gameObject.CompareTag("Boost"))
            {
                count += 1;
                SetCountText();
                blast = GetComponentInChildren<ParticleSystem>();
                blast.transform.position = other.transform.position;
                blast.Play();
                other.gameObject.SetActive(false);

            }
            if (other.gameObject.CompareTag("Obstacle"))
            {
                life -= 1;
                SetLifeText();
                if (life <= 0)
                {
                    gamecontroller.GameOver();
                }
                blast = GetComponentInChildren<ParticleSystem>();
                blast.transform.position = other.transform.position;
                blast.Play();
                other.gameObject.SetActive(false);
            }

            if (count > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", count);
                highScore.text = count.ToString();
            }

            if (other.gameObject.CompareTag("Cloud"))
            {
                inCloudTime++;
                cloudcollisioncount++;
                Vector3 sparklePosition = gameObject.transform.position.x < 0 ? new Vector3(gameObject.transform.position.x - SPARKLE_POS_OFFSET_X,
                   gameObject.transform.position.y, gameObject.transform.position.z + 30) : new Vector3(gameObject.transform.position.x + SPARKLE_POS_OFFSET_X,
                   gameObject.transform.position.y, gameObject.transform.position.z + 30);
                SetTimer();
                sparkleEffect.transform.position = sparklePosition;
                sparkleEffect.Play();
                if (inCloudTime >= ALLOWED_IN_CLOUD_TIME)
                {
                    gamecontroller.GameOver();
                }
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

        void SetTimer()
        {
            Timer.enabled = true;
            Timer.text = (ALLOWED_IN_CLOUD_TIME - inCloudTime).ToString() + " s left!";
        }

        void HideTimer()
        {

            inCloudTime = 0;
            Timer.enabled = false;
        }

        // collision that ends the game

        //Here is the sparkle effects
        //TODO: detect collision by invisible plane instead of ParticleCollision
        /*
        void OnParticleCollision(GameObject other)
        {

            //if (other.gameObject.CompareTag("Lightning")) {
            //    life -= 1;
            //    SetLifeText();
            //    if (life <= 0)
            //    {
            //        gamecontroller.GameOver();
            //    }
            //}
        }
        */

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Cloud"))
            {
                cloudcollisioncount--;
            }

        }

        bool checkOutOfCloud()
        {
            if ((cloudcollisioncount == 0))
            { return true; }
            else { return false; }
        }
        void resetTimeOutofCloud(bool outCloud)
        {
            if (outCloud)
            {
                if (Timer.enabled == true)
                {
                    HideTimer();
                }
            }
        }


        protected virtual void Translate(Vector2 scaledDelta)
        {
            // If camera is null, try and get the main camera, return true if a camera was found
            if (LeanTouch.GetCamera(ref Camera) == true)
            {
                // Screen position of the transform
                var screenPos = Camera.WorldToScreenPoint(transform.position);

                // Add the deltaPosition
                screenPos += (Vector3)scaledDelta;

                // Convert back to world space
                transform.position = Camera.ScreenToWorldPoint(screenPos);
            }
        }
    }
}
