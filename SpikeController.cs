using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    public float moveDistance = 1f;
    public float cycleTime = 0.2f;
    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(MoveSpike());
    }
    
    public void StartMoving() 
    {
        StartCoroutine(MoveSpike());
    }

    public IEnumerator MoveSpike()
    {
        while(true)
        {
            yield return Move(Vector3.up * moveDistance, cycleTime / 2);
            yield return new WaitForSeconds(0.5f);
            yield return Move(Vector3.down * moveDistance, cycleTime / 2);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator Move(Vector3 direction, float duration)
    {
        float elapsed = 0;
        Vector3 startPosition = transform.position;

        while(elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, startPosition + direction, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPosition + direction;
    }
}