using System;
using UnityEngine;

namespace HL
{
    public class MovingPlatform : MonoBehaviour
    {
        Rigidbody2D rb;

        [SerializeField] private float speed;
        [SerializeField] private Transform spriteObject;
        [SerializeField] private Transform[] points;

        private Vector3 targetPos;
        private Vector3 moveDirection;
        private int currentPointIndex;

        private void Awake()
        {
            rb = GetComponentInChildren<Rigidbody2D>();
        }

        private void Start()
        {
            if (points.Length != 0)
            {
                spriteObject.position = points[0].position;
                targetPos = points[1].position;
                CalculateDirection();
            }
            else
                Debug.Log("No set points for moving platform.");
        }

        private void Update()
        {
            if (Vector2.Distance(spriteObject.position, targetPos) < 0.25f)
            {
                currentPointIndex = (currentPointIndex + 1) % points.Length;
                targetPos = points[currentPointIndex].position;
                CalculateDirection();
            }
        }

        private void FixedUpdate()
        {
            rb.velocity = moveDirection * speed;
        }

        private void CalculateDirection()
        {
            moveDirection = (targetPos - spriteObject.position).normalized;
        }
    }
}