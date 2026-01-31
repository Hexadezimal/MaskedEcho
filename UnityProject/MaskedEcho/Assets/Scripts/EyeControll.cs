using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeControll : MonoBehaviour
{
    public GameObject eyes;
    public Camera camera;
    public Transform target;
    public float speed = 2;
    public float intensity = 0.3f;
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (camera != null)
        {
            EyesAim();
        }
        // if (target != null)
        // {
        //     EyesAim2();
        // }
    }
    void EyesAim()
    {
        /* Get the mouse position in world space rather than screen space. */
        //var mouseWorldCoord = camera.ScreenPointToRay(Input.mousePosition).origin;
        //var mouseWorldCoord = camera.ScreenToWorldPoint(Input.mousePosition);

        /* Get a vector pointing from initialPosition to the target. Vector shouldn't be longer than maxDistance. */
        var originToMouse = Input.mousePosition - this.transform.localPosition;
        Debug.Log(originToMouse);
        originToMouse = Vector3.ClampMagnitude(originToMouse, intensity);

        /* Linearly interpolate from current position to mouse's position. */
        eyes.transform.position = Vector3.Lerp(eyes.transform.position, this.transform.position + originToMouse, speed * Time.deltaTime);
    }

    void EyesAim2()
    {

        /* Get a vector pointing from initialPosition to the target. Vector shouldn't be longer than maxDistance. */
        var originToTarget = target.position - this.transform.position;
        originToTarget = Vector3.ClampMagnitude(originToTarget, intensity);

        /* Linearly interpolate from current position to mouse's position. */
        eyes.transform.position = Vector3.Lerp(eyes.transform.position, this.transform.position + originToTarget, speed * Time.deltaTime);
    }
}
