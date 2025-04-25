using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimer : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform player;
    [SerializeField] private float threshold = 1f;

    public Vector3 mousePos;
    public Vector3 direction;

    void Update()
    {
        if (cam == null || player == null)
            return;

        // Continously retrieve the position of mouse in the worldspace and store it as Vector3
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Optional: force 2D positioning

        direction = mousePos - player.position;

        if (direction.magnitude > threshold)
        {
            direction = direction.normalized * threshold;
        }

        transform.position = player.position + direction;
    }
    private void OnDrawGizmos()
    {
        if (player == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mousePos, 0.01f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(player.position, mousePos);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(player.position, player.position + direction);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(player.position + direction, 0.01f);
    }
}
