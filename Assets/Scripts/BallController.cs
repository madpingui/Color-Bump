using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField]
    private float thrust = 150f;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float wallDistance = 5f;
    [SerializeField]
    private float minCamDistance = 3f;

    private Vector2 lastMousePos;

    void Update()
    {
        if (GameManager.singleton.GameEnded)
            return;

        Vector2 deltaPos = Vector2.zero;

        if (Input.GetMouseButton(0))
        {
            if (!GameManager.singleton.GameStarted)
                GameManager.singleton.StartGame();


            Vector2 currentMousePos = Input.mousePosition;

            if(lastMousePos == Vector2.zero)
                lastMousePos = currentMousePos;

            deltaPos = currentMousePos - lastMousePos;
            lastMousePos = currentMousePos;

            Vector3 force = new Vector3(deltaPos.x, 0, deltaPos.y) * thrust;
            rb.AddForce(force);
        }
        else
        {
            lastMousePos = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        //if (GameManager.singleton.GameEnded)
        //    return;

        if (GameManager.singleton.GameStarted)
        {
            rb.MovePosition(transform.position + Vector3.forward * 15 * Time.fixedDeltaTime);
        }
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;

        if(transform.position.x < -wallDistance)
        {
            pos.x = -wallDistance;
        }
        else if(transform.position.x > wallDistance)
        {
            pos.x = wallDistance;
        }

        if(transform.position.z < Camera.main.transform.position.z + minCamDistance)
        {
            pos.z = Camera.main.transform.position.z + minCamDistance;
        }

        transform.position = pos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.singleton.GameEnded)
            return;

        if(collision.gameObject.tag == "Death")
            GameManager.singleton.EndGame(false);
    }
}
