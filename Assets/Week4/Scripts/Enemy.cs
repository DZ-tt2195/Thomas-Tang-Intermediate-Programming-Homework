using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ThomasTang.Week4
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] Transform mainBody;
        [SerializeField] Animator anim;
        NavMeshAgent nav;
        Transform mainCamera;

        void Awake()
        {
            mainCamera = Camera.main.transform;
            nav = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(CalculateLookingDirection), 0, 1f); //every second, recalculate facing angle
        }

        void CalculateLookingDirection()
        {
            float currentRelativeDirection = Vector3.Dot(nav.velocity, mainCamera.right); 
            if (currentRelativeDirection > 0 && mainBody.localScale.x > 0 || //if this goes left, look left
                currentRelativeDirection < 0 && mainBody.localScale.x < 0) //if this goes right, look right
            {
                var localS = mainBody.localScale;
                localS.x *= -1;
                mainBody.localScale = localS;
            }
        }

        private void Update()
        {
            nav.SetDestination(Player.instance.transform.position); //every frame, aim towards the player

            if (nav.velocity.magnitude < 0.1f) //change animation based on speed
            {
                anim.SetBool("isWalking", false);
            }
            else
            {
                anim.SetBool("isWalking", true);
                anim.SetFloat("Velocity", nav.velocity.magnitude);
            }
        }
    }
}