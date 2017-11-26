using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public static bool moveGrassAndSky;
    public GameObject player;
    public float smoothing;

    const float MAX_DISTANCE = 30f;

    Vector3 initialPosition;
    float totalDistanceTraveled;

    void Start() {
    	initialPosition = transform.position;
    	totalDistanceTraveled = 0f;
    	moveGrassAndSky = false;
    }

    private void FixedUpdate() {
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        if(newPos.x > initialPosition.x) {
        	totalDistanceTraveled += Vector3.Distance(initialPosition, newPos);
        }
        if(totalDistanceTraveled > MAX_DISTANCE) {
        	moveGrassAndSky = true;
        }
        this.transform.position = Vector3.Lerp(this.transform.position, newPos, Time.fixedDeltaTime * smoothing);
    }
}
//References: https://unity3d.com/learn/tutorials/projects/roll-ball-tutorial/moving-camera?playlist=17141
