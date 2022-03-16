using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Core{
    public class ColliderController : MonoBehaviour
    {
        [Header("Reference to the polygon collider ground counterpart")]
        [SerializeField] PolygonCollider2D polyGroundCollider;
        [Header("Will be generated based on the polyGroundCollider")]
        [SerializeField] MeshCollider GroundCollider;
        [Header("List of PolygonCollider Counterpart")]
        [SerializeField] List<PolygonCollider2D> PolygonCol2dHole = new List<PolygonCollider2D>();
        [Header("All players/enemy/hole/ who has a polygon counterpart")]
        [SerializeField] GameObject[] playersHole;
        [Header("Generated mesh which the mesh collider will based on")]
        [SerializeField]Mesh GeneratedMesh;

        
        private void Start() {
            //get all players and then instantiate reference
            //instantiate their poly collider representation
            playersHole = findAllHolePlayer();
            SpawnPolyCounterPart(playersHole);
        }

        private void FixedUpdate() { 
               if(PolygonCol2dHole == null)return;
                GenerateHole(); //generate hole to mesh collider
                GenerateMeshCollider();
        }
        

        void GenerateHole()
        {
           polyGroundCollider.pathCount = playersHole.Length + 1; // Get how many players
           for(int p = 0; p < playersHole.Length; p++){
                Vector2[] pointsPositions = PolygonCol2dHole[p].GetPath(0); // get the initial path of the polygon collider
                for (int index = 0; index < pointsPositions.Length; index++)
                {
                    pointsPositions[index] = PolygonCol2dHole[p].transform.TransformPoint(pointsPositions[index]);
                }
                polyGroundCollider.SetPath(p+1, pointsPositions); //set path and pointPosition for the ground collider to generate a hole in mesh collider
           }
            
        }

        void GenerateMeshCollider(){ //generate mesh collider
            if(GeneratedMesh != null) GeneratedMesh = null;
                GeneratedMesh = polyGroundCollider.CreateMesh(true,true);
                GroundCollider.sharedMesh = GeneratedMesh;

        }
      
         GameObject[] findAllHolePlayer(){
            HoleStats[] hole = FindObjectsOfType(typeof(HoleStats)) as HoleStats[]; //get all who has HoleStats Component
            GameObject[] gbPlayers = new GameObject[hole.Length];
            for(int i = 0; i < hole.Length;i++){
                gbPlayers[i] = hole[i].GetComponent<Transform>().gameObject;
            }
            return gbPlayers;
        }

        void SpawnPolyCounterPart(GameObject[] playersGB){ // spawn polygoncollider counterpart
            for(int index = 0; index < playersGB.Length; index ++){
                GameObject poly = Instantiate(playersGB[index].GetComponent<IPolyCounterPart>().GetPolyCounterPart(),transform.position,transform.rotation,this.transform);
                playersGB[index].GetComponent<IPolyCounterPart>().SetPoly(poly);
                poly.GetComponent<AdaptTransform>().SetReference(playersGB[index].transform);
                PolygonCol2dHole.Add(poly.GetComponent<PolygonCollider2D>());
            }
        }
    }
}

