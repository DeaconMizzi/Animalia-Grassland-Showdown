using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPosX;
    private float spriteWidth;

    public Transform cam;
    public float parallaxEffect;

    private void Start()
    {
        startPosX = transform.position.x;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        float camX = cam.position.x;
        float dist = camX * parallaxEffect;
        float temp = camX * (1 - parallaxEffect);

        // 🔥 Instantly snap — no smoothing
        transform.position = new Vector3(startPosX + dist, transform.position.y, transform.position.z);

        // 🔁 Seamless wrap logic
        if (temp > startPosX + spriteWidth)
            startPosX += spriteWidth;
        else if (temp < startPosX - spriteWidth)
            startPosX -= spriteWidth;
    }
}
