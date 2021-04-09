using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _basePos;

    [SerializeField] private float xzMoveIntensity = 0.1f;
    [SerializeField] private float yMoveIntensity = 0.1f;
    
    void Start()
    {
        _basePos = transform.position;
    }

    void Update()
    {
        List<Player> players = GameController.INSTANCE.GetPlayers();
        if (players.Count > 0)
        {
            List<Vector3> positions = new List<Vector3>();
            Vector3 averagePos = Vector3.zero;
            float maxDist = 0f;
            foreach (var player in players)
            {
                // save maxdist as a variable so that we can use this for zooming the camera in and out
                foreach (var position in positions)
                {
                    float dist = Vector3.Distance(player.transform.position, position);
                    if (dist > maxDist)
                        maxDist = dist;
                }
                
                // save the position and add to averagePos for processing
                positions.Add(player.transform.position);
                averagePos += player.transform.position;
            }

            // calculate final avg xz pos
            averagePos /= players.Count;

            // final position processing (move on xz to avg players, move on y based on how far apart)
            Vector3 xzOffset = (averagePos - _basePos) * xzMoveIntensity;
            float yOffsetScalar = maxDist * yMoveIntensity;
            Vector3 yOffset = Vector3.back * yOffsetScalar;
            transform.position = _basePos + new Vector3(xzOffset.x, 0f, xzOffset.z) + yOffset;
        }
        else
        {
            transform.position = _basePos;
        }
    }
}
