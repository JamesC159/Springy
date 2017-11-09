using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject player;
    public float smoothing;

    private void FixedUpdate() {
        Vector3 newPos = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
        this.transform.position = Vector3.Lerp(this.transform.position, newPos, Time.fixedDeltaTime * smoothing);
    }
}
//References: https://unity3d.com/learn/tutorials/projects/roll-ball-tutorial/moving-camera?playlist=17141
