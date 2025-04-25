using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUnit : MonoBehaviour
{
    public float offset = 1.3f;
    private Transform target;
    private void OnEnable()
    {
        target = transform.parent;
    }
    private void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(target.transform.position + Vector3.up * offset);
        transform.position = screenPoint;
    }
}
