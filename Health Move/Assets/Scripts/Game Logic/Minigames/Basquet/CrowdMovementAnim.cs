using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMovementAnim : MonoBehaviour
{
    [SerializeField] float speed = .1f;
    int direction = 1;
    [SerializeField] float maxMove = .5f;
    float originalPosition;

    private void Start()
    {
        originalPosition = transform.position.y;
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * direction);

        if (transform.position.y > originalPosition + maxMove || transform.position.y < originalPosition - maxMove)
            direction *= -1;
    }
}
