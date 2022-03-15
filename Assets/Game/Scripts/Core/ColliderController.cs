using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Core{
    public class ColliderController : MonoBehaviour
    {
        
        [SerializeField] PolygonCollider2D polyGroundCollider;
        [SerializeField] MeshCollider GroundCollider;
        [SerializeField] List<PolygonCollider2D> PolygonCol2dHole = new List<PolygonCollider2D>();
        [SerializeField] GameObject[] playersHole;
        [SerializeField]Mesh GeneratedMesh;

        
        private void Start() {
            //get all players with then instantiate reference
            //instantiate their poly collider representation
            //upon instatiation set their player control
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
                    //pointsPositions[index] = (Vector2)holePoly2d.transform.position;
                    pointsPositions[index] = PolygonCol2dHole[p].transform.TransformPoint(pointsPositions[index]);
                }
                polyGroundCollider.SetPath(p+1, pointsPositions); //set path and pointPosition for the ground collider to generate a hole in mesh collider
           }
            
        }

        void GenerateMeshCollider(){
            if(GeneratedMesh != null) GeneratedMesh = null;
                GeneratedMesh = polyGroundCollider.CreateMesh(true,true);
                GroundCollider.sharedMesh = GeneratedMesh;

        }
      
         GameObject[] findAllHolePlayer(){
            //GameObject[] gbPlayers = GameObject.FindGameObjectsWithTag("Player");
            HoleStats[] hole = FindObjectsOfType(typeof(HoleStats)) as HoleStats[]; //if a Object has a HoleStats given its a Hole
            GameObject[] gbPlayers = new GameObject[hole.Length];
            for(int i = 0; i < hole.Length;i++){
                gbPlayers[i] = hole[i].GetComponent<Transform>().gameObject;
            }
            return gbPlayers;
        }

        void SpawnPolyCounterPart(GameObject[] playersGB){
            for(int index = 0; index < playersGB.Length; index ++){
                GameObject poly = Instantiate(playersGB[index].GetComponent<IPolyCounterPart>().GetPolyCounterPart(),transform.position,transform.rotation,this.transform);
                poly.GetComponent<AdaptTransform>().SetReference(playersGB[index].transform);
                PolygonCol2dHole.Add(poly.GetComponent<PolygonCollider2D>());
            }
        }
    }
}

