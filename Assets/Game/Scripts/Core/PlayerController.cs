using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Movement;

namespace Game.Core{
    public class PlayerController : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] Vector3 movement = new Vector2();
        [SerializeField] Mover mover;
        void Start()
        {
            if (GetComponent<Mover>() != null) mover = GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update()
        {
            #if UNITY_EDITOR
                    //hide gamepad
                    movement.x = Input.GetAxis("Horizontal");
                    movement.z = Input.GetAxis("Vertical");
            #endif

            #if UNITY_ANDROID
                    //gamepad input
            #endif

        }

        void FixedUpdate()
        {
            mover.Move(movement);
        }
    }
}

