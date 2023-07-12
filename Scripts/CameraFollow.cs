using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public float xMargin = 1f;		
    public float yMargin = 1f;		
    public float xSmooth = 8f;		
    public float ySmooth = 8f;	
    private float leftBounds = 0f;
    private float rightBounds = 0f;
    private float topBounds = 0;
    private Transform player;		
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        leftBounds = GameObject.FindGameObjectWithTag("BorderLeft").transform.position.x;
        rightBounds = GameObject.FindGameObjectWithTag("BorderRight").transform.position.x;
        topBounds = GameObject.FindGameObjectWithTag("BorderTop").transform.position.y;
        float size = Camera.main.orthographicSize;
        float aspect = (float)Screen.width/Screen.height;
        minX = aspect*size + leftBounds;
        maxX = rightBounds - aspect*size;
        minY = topBounds - size;
        maxY = minY;
    }


    bool CheckXMargin()
    {
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }


    bool CheckYMargin()
    {
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }
    void FixedUpdate()
    {
        TrackPlayer();
    }


    void TrackPlayer()
    {
        float targetX = transform.position.x;
        float targetY = transform.position.y;
        if (CheckXMargin())
            targetX = Mathf.Lerp(transform.position.x, player.position.x, xSmooth * Time.deltaTime);
        if (CheckYMargin())
            targetY = Mathf.Lerp(transform.position.y, player.position.y, ySmooth * Time.deltaTime);
        targetX = Mathf.Clamp(targetX, minX, maxX);
        targetY = Mathf.Clamp(targetY, minY, maxY);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
