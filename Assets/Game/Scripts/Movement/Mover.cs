using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Movement{
    public class Mover : MonoBehaviour
    {
        [SerializeField] float speed = 2f;

        public void Move(Vector3 playerPos)
        {
            // rigidBody.MovePosition(rigidBody.position + shooterPosition * speed * Time.deltaTime);
            transform.position += playerPos * speed * Time.deltaTime;
        }


    }
}

