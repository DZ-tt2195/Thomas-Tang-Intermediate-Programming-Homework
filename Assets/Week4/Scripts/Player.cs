using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace ThomasTang.Week4
{
    public class Player : MonoBehaviour
    {
        public static Player instance;
        bool alive = true;
        [SerializeField] TextMesh textbox;
        float survivalTime = 0f;

        [SerializeField] float speed;
        [SerializeField] Transform mainBody;
        [SerializeField] Animator anim;
        [SerializeField] Enemy enemyPrefab;
        NavMeshAgent nav;

        private void Awake()
        {
            nav = GetComponent<NavMeshAgent>();
            instance = this;
        }

        private void Start()
        {
            InvokeRepeating(nameof(SpawnEnemy), 1f, 5f); //every 5 seconds, spawn an enemy
        }

        void SpawnEnemy()
        {
            Enemy newEnemy = Instantiate(enemyPrefab); //create a new enemy
            newEnemy.transform.position = new Vector3(0, 0, 0);
        }

        private void Update()
        {
            textbox.text = $"Time Survived: {(int)survivalTime}"; //update the textbox for the timer

            if (alive) //if this is alive
            {
                survivalTime += Time.deltaTime; //increase survival time

                float horizontalInput = Input.GetAxis("Horizontal"); 
                float verticalInput = Input.GetAxis("Vertical");

                Vector3 movement = new(horizontalInput, 0f, verticalInput); //convert inputs into moving around
                transform.Translate(speed * Time.deltaTime * movement);

                if ((horizontalInput < 0 && mainBody.localScale.x < 0) //if this starts going left, look left
                    || horizontalInput > 0 && mainBody.localScale.x > 0) //if this starts going right, look right
                {
                    var localS = mainBody.localScale;
                    localS.x *= -1;
                    mainBody.localScale = localS;
                }

                if (nav.velocity.magnitude < 0.1f) //switch animation between walking and running
                {
                    anim.SetBool("isWalking", false);
                }
                else
                {
                    anim.SetBool("isWalking", true);
                    anim.SetFloat("Velocity", speed);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (alive && other.CompareTag("Enemy")) //if this hits an enemy, it dies
            {
                alive = false;
                anim.SetTrigger("Dying");
                nav.isStopped = true;
                Invoke(nameof(RestartScene), 2f); //reload the scene
            }
        }

        void RestartScene()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
}