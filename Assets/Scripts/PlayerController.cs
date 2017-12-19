using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace Lean.Touch

{

    public class PlayerController : MonoBehaviour
    {

        // basic game and player variables
        private Rigidbody rb;
        public GameController gameController;
        public float speed;
        private bool isStarted = false;

        // cloud collision variables
        private const int ALLOWED_IN_CLOUD_TIME = 5;
        private const int SPARKLE_POS_OFFSET_X = 10;
        private int inCloudTime = 0;
        private int cloudCollisionCount = 0;
        public Text timer;
        public ParticleSystem blast;
        public ParticleSystem sparkleEffect;

        // rainbow square indicators
        private ArrayList collected;
        private const int NUM_COLORS = 6;
        private GameObject redSq;
        private GameObject orangeSq;
        private GameObject yellowSq;
        private GameObject greenSq;
        private GameObject blueSq;
        private GameObject violetSq;

        // score & life variables
        private int count;
        private int lastCount;
        private int life = 3;
        public Text countText;
        public Text highScore;
        public Text lifeText;

        // variables for player movement (required by Lean Touch)
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

          // Set up player and game controllers
          rb = GetComponent<Rigidbody>();
          GameObject gameControllerObject = GameObject.FindWithTag("GameController");
          gameController = gameControllerObject.GetComponent<GameController>();

          // Set up rainbow indicator set
          redSq = GameObject.FindGameObjectWithTag("Red");
          orangeSq = GameObject.FindGameObjectWithTag("Orange");
          yellowSq = GameObject.FindGameObjectWithTag("Yellow");
          greenSq = GameObject.FindGameObjectWithTag("Green");
          blueSq = GameObject.FindGameObjectWithTag("Blue");
          violetSq = GameObject.FindGameObjectWithTag("Violet");
          ClearSquares();
          collected = new ArrayList();

          // Set up on-screen text
          count = 0;
          lastCount = count;
          SetCountText();
          highScore.text = "Best: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
          timer.enabled = false;

          // Check if touch movement requires a selectable object (Lean Touch condition)
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
                float boost = 500f / rb.velocity.magnitude;
                rb.AddForce(rb.velocity.normalized * boost, ForceMode.Impulse);
                lastCount = count;
            }
            resetTimeOutofCloud(checkOutOfCloud());

            // Check if game started and implement touch & keyboard input accordingly
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

            // Touch input
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

        // Detect collision with other game objects
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Boost"))
            {
                Material boostMaterial = other.GetComponent<Renderer>().material;
                LogBoostColor(boostMaterial.ToString());
                count += 1;
                SetCountText();
                ScoreAnimationScript.Instance.PlayAnimation();
                Blast(other);
            }

            if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Lightning"))
            {
                DecrementLife();
                Blast(other);
            }

            if (count > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", count);
                highScore.text = "Best: " + count.ToString();
            }

            if (other.gameObject.CompareTag("Cloud"))
            {
                inCloudTime++;
                cloudCollisionCount++;
                Vector3 sparklePosition = gameObject.transform.position.x < 0 ? new Vector3(gameObject.transform.position.x - SPARKLE_POS_OFFSET_X,
                   gameObject.transform.position.y, gameObject.transform.position.z + 30) : new Vector3(gameObject.transform.position.x + SPARKLE_POS_OFFSET_X,
                   gameObject.transform.position.y, gameObject.transform.position.z + 30);
                SetTimer();
                sparkleEffect.transform.position = sparklePosition;
                sparkleEffect.Play();
                if (inCloudTime >= ALLOWED_IN_CLOUD_TIME)
                {
                    gameController.GameOver();
                }
            }
        }

        // clear squares on the rainbow panel
        public void ClearSquares()
        {
            redSq.SetActive(false);
            orangeSq.SetActive(false);
            yellowSq.SetActive(false);
            greenSq.SetActive(false);
            blueSq.SetActive(false);
            violetSq.SetActive(false);
        }

        // blast effect
        void Blast(Collider collider)
        {
          blast = GetComponentInChildren<ParticleSystem>();
          blast.transform.position =new Vector3(collider.transform.position.x, collider.transform.position.y, collider.transform.position.z+40);
          blast.Play();
          collider.gameObject.SetActive(false);
        }

        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Cloud"))
            {
                cloudCollisionCount--;
            }

        }

        bool checkOutOfCloud()
        {
            if ((cloudCollisionCount == 0))
            { return true; }
            else { return false; }
        }

        void resetTimeOutofCloud(bool outCloud)
        {
            if (outCloud)
            {
                if (timer.enabled == true)
                {
                    HideTimer();
                }
            }
        }


        void LogBoostColor(string colName)
        {
            string colLet = colName[0].ToString();
            if (!collected.Contains(colLet))
            {
                collected.Add(colLet);
                UpdatePanel(colLet);
            }
            else //if collected contains letter, duplicate color, set ruined and should be cleared
            {
                RainbowScript.Instance.PlaySetRuined();
                StartCoroutine(DelayForAnim(1));
                collected.Clear();
                DecrementLife();
            }
            if (collected.Count == NUM_COLORS)  //if set is complete
            {
                RainbowScript.Instance.PlayAnimation();
                collected.Clear();
                StartCoroutine(DelayForAnim(1));

                count += 30;
                SetCountText();
            }
        }

        IEnumerator DelayForAnim(int seconds) {
            yield return new WaitForSeconds(seconds);
            ClearSquares();
        }

        // update the rainbow panel
        void UpdatePanel(string col)
        {
            switch (col) {
                case "R":
                    redSq.SetActive(true);
                    break;
                case "O":
                    orangeSq.SetActive(true);
                    break;
                case "Y":
                    yellowSq.SetActive(true);
                    break;
                case "G":
                    greenSq.SetActive(true);
                    break;
                case "B":
                    blueSq.SetActive(true);
                    break;
                case "V":
                    violetSq.SetActive(true);
                    break;
            }

        }

        void SetCountText()
        {
            countText.text = "Score: " + count.ToString();
        }

        void SetLifeText()
        {
            lifeText.text = "Life: " + life.ToString();
            LifeTextScript.Instance.PlayAnimation();
        }

        void DecrementLife()
        {
            life -= 1;
            lifeText.text = "Life: " + life.ToString();
            LifeTextScript.Instance.PlayAnimation();
            if (life <= 0)
            {
                gameController.GameOver();
            }
        }

        void SetTimer()
        {
            timer.enabled = true;
            timer.text = (ALLOWED_IN_CLOUD_TIME - inCloudTime).ToString() + " s left!";
        }

        void HideTimer()
        {

            inCloudTime = 0;
            timer.enabled = false;
        }

        // Move the player
        protected virtual void Translate(Vector2 scaledDelta)
        {
            // If camera is null, try and get the main camera, return true if a camera was found
            if (LeanTouch.GetCamera(ref Camera) == true)
            {
                // Screen position of the transform
                var screenPos = Camera.WorldToScreenPoint(transform.position);

                float fingerResponsiveness = 100f, fingerCurve = 1.5f;
                scaledDelta = scaledDelta.normalized
                     * (float) Math.Pow(scaledDelta.magnitude / fingerResponsiveness, fingerCurve)
                     * fingerResponsiveness;

                // Add the deltaPosition
                screenPos += (Vector3)scaledDelta;

                // Convert back to world space
                Vector3 worldPos = Camera.ScreenToWorldPoint(screenPos);
                transform.position = new Vector3(Mathf.Clamp(worldPos.x, -60f, 125f), Mathf.Clamp(worldPos.y, 20, 200f), worldPos.z);
            }
        }
    }
}
