using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    private void Update()
    {
        Camelow();
    }

    private void Camelow()
    {
        Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, Time.deltaTime * 5f);
    }
}
