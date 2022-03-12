using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Core{
    public class ColliderController : MonoBehaviour
    {
        [SerializeField] PolygonCollider2D holePoly2d;
        [SerializeField] PolygonCollider2D polyGroundCollider;
        [SerializeField] MeshCollider GroundCollider;
        [SerializeField] GameObject player;


        [SerializeField]Mesh GeneratedMesh;

        private void Start()
        {
            if(player == null) player = GameObject.FindGameObjectWithTag("Player");
            
        }

        private void FixedUpdate() {
            if(player.transform.hasChanged == true){
                player.transform.hasChanged = false;
                holePoly2d.gameObject.transform.position = new Vector2(player.transform.position.x, player.transform.position.z);
                holePoly2d.transform.localScale = player.transform.localScale * .5f;
                GenerateHole();
                GenerateMeshCollider();
            }
            
           
        }

        void GenerateHole()
        {
            Vector2[] pointsPositions = holePoly2d.GetPath(0);

            for(int index = 0; index < pointsPositions.Length;index++){
               //pointsPositions[index] = (Vector2)holePoly2d.transform.position;
               pointsPositions[index] = holePoly2d.transform.TransformPoint(pointsPositions[index]);
            }

            polyGroundCollider.pathCount = 2;
            polyGroundCollider.SetPath(1,pointsPositions);
        }

        void GenerateMeshCollider(){
            if(GeneratedMesh != null) GeneratedMesh = null;
                GeneratedMesh = polyGroundCollider.CreateMesh(true,true);
                GroundCollider.sharedMesh = GeneratedMesh;

        }
    }
}

