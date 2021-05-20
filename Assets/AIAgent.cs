using System;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    enum DirectionMove
    {
        up,
        down,
        left,
        right
    }

    private Rigidbody rb => GetComponent<Rigidbody>();
    public Transform targetGoal;
    public Action GoalComplite;
    [SerializeField] private float stepTimer;
    private float timer = 0;
    private bool StepTime;
    [SerializeField] private float speed;

    private void Start()
    {
        timer = stepTimer;
    }

    private void Update()
    {
        if (targetGoal == null) return;
        if (timer <= 0)
        {
            timer = stepTimer;
            if ((int) targetGoal.position.x > (int)transform.position.x)
            {
                OnMove(DirectionMove.right);
                return;
            }

            if ((int)targetGoal.position.x < (int)transform.position.x)
            {
                OnMove(DirectionMove.left);
                return;
            }

            if ((int)targetGoal.position.z < (int)transform.position.z)
            {
                OnMove(DirectionMove.down);
                return;
            }

            if ((int)targetGoal.position.z > (int)transform.position.z)
            {
                OnMove(DirectionMove.up);
            }
        }
        else
        {
            timer -= Time.deltaTime * 1;
        }
    }

    private void OnMove(DirectionMove direction)
    {
        Vector3 dir = new Vector3();
        switch (direction)
        {
            case DirectionMove.up:
                dir = Vector3.forward;
                break;
            case DirectionMove.down:
                dir = Vector3.back;
                break;
            case DirectionMove.left:
                dir = Vector3.left;
                break;
            case DirectionMove.right:
                dir = Vector3.right;
                break;
        }

        rb.AddForce(dir * (speed * 1000));
    }
}