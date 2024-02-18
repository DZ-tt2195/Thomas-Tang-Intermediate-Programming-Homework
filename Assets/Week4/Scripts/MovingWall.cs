using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ThomasTang.Week4
{
    public class MovingWall : MonoBehaviour
    {
        NavMeshAgent agent;
        Vector3 originalPosition;
        [SerializeField] Vector3 newPosition;

        enum MovingTowards { Original, New}; //track if this is moving to a new position, or its original position
        MovingTowards moving = MovingTowards.Original;

        private void Awake()
        {
            originalPosition = transform.position;
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(RecalculatePosition), 0f, 2f); //every 2 seconds, move back and forth
        }

        void RecalculatePosition()
        {
            if (moving == MovingTowards.New) //if this was moving to the new position, go back to original
            {
                agent.SetDestination(originalPosition);
                moving = MovingTowards.Original;
            }
            else if (moving == MovingTowards.Original) //if this was moving to the original position, go to new
            {
                agent.SetDestination(newPosition);
                moving = MovingTowards.New;
            }
        }
    }
}