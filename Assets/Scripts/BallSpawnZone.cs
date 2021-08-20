using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnZone : MonoBehaviour
{

    [SerializeField]
    Transform[] zonePoints;

    private GameObject field;
    public GameObject a;
    private GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }
    private void OnDrawGizmosSelected()
    {
        if (zonePoints.Length > 1)
        {
            Gizmos.color = Color.red;
            for (int i = 0, j = 1; j < zonePoints.Length; ++i, ++j)
            {

                Gizmos.DrawLine(zonePoints[i].position, zonePoints[j].position);
            }
            Gizmos.DrawLine(zonePoints[3].position, zonePoints[0].position);
        }
    }


/*    private float GetSmallerSideSize()
    {
        if(zonePoints.Length==4)
        {
            var p0p1 = Vector3.Distance(zonePoints[0].position, zonePoints[1].position);
            Debug.Log("pop1 " + p0p1);
            var p1p2 = Vector3.Distance(zonePoints[1].position, zonePoints[2].position);

            var result = p0p1 > p1p2 ? p0p1 / 2f : p1p2 / 2f;
            return result;
        }
        else
        {
            return 0;
        }
    }*/

    public  Vector3 GetSpawnPosition()
    {
        //a.transform.position =  new Vector3(this.transform.position.y + Random.Range(-dist, dist), 0, this.transform.position.x + Random.Range(-dist, dist));
        if(gameController.GetIsARMode())
            return this.transform.position + new Vector3(Random.Range(-0.7f, 0.7f), 0, Random.Range(-0.7f, 0.7f));
        else
         return this.transform.position + new Vector3(Random.Range(-8.5f, 8.5f), 0, Random.Range(-8.5f, 8.5f));        
    }

}
