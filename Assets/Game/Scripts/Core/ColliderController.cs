using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Core{
    public class ColliderController : MonoBehaviour
    {
        [SerializeField] PolygonCollider2D[] holePoly2ds;
        [SerializeField] PolygonCollider2D polyGroundCollider;
        [SerializeField] MeshCollider GroundCollider;
        [SerializeField] GameObject[] players;
        [SerializeField]Mesh GeneratedMesh;

        private void FixedUpdate() {
                holeMovements(); //check position of every hole per fixed update and genrate hole to mesh collider
                GenerateMeshCollider();
        }

        void holeMovements(){
            for (int i = 0; i < players.Length; i++)
            {
                holePoly2ds[i].gameObject.transform.position = new Vector2(players[i].transform.position.x, players[i].transform.position.z);
            }
            GenerateHole();
            
        }

        

        void GenerateHole()
        {
           polyGroundCollider.pathCount = players.Length + 1; // Get how many players
           for(int p = 0; p < players.Length; p++){
                Vector2[] pointsPositions = holePoly2ds[p].GetPath(0); // get the initial path of the polygon collider
                for (int index = 0; index < pointsPositions.Length; index++)
                {
                    //pointsPositions[index] = (Vector2)holePoly2d.transform.position;
                    pointsPositions[index] = holePoly2ds[p].transform.TransformPoint(pointsPositions[index]);
                }
                polyGroundCollider.SetPath(p+1, pointsPositions); //set path and pointPosition for the ground collider to generate a hole in mesh collider
           }
            

            // = to length of players
            //iterate setpath
            

            
            
        }

        void GenerateMeshCollider(){
            if(GeneratedMesh != null) GeneratedMesh = null;
                GeneratedMesh = polyGroundCollider.CreateMesh(true,true);
                GroundCollider.sharedMesh = GeneratedMesh;

        }
    }
}

