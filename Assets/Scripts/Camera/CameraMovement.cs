using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private Transform player;

        private Vector3 offset;
        private Camera cam;

        private void Awake()
        {
            offset = new Vector3(player.position.x - transform.position.x, player.position.y - transform.position.y, player.position.z - transform.position.z);
            cam = Camera.main;
        }

        private void FixedUpdate()
        {
            transform.position = player.position - offset;
            transform.LookAt(player);
        }
    }
}