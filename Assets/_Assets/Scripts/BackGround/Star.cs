using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class Star : MonoBehaviour
{

    // Data
    [SerializeField] StarScripTable starData;

    private SpriteRenderer sr;

    // moving
    private float timeLive;
    private float timeRemaining;
    private float speedStar;
    private float angle;
    private float scale;
    private Vector3 direction;

    public float minPosition = 0.1f;
    public float maxPosition = 1.1f;

    // alpha
    private Color color;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Reset_Star();

        color = sr.color;
        color.a = 0;
        sr.color = color;
    }

    void Update()
    {
        transform.position += direction * speedStar * Time.deltaTime;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0f)
        {
            Reset_Star();
        }

        float t = timeRemaining/timeLive;
        if(t < 0.2f)
        {
            ChangeAlpha(Mathf.InverseLerp(0f, 0.2f, t));    
        }
        else if(t > 0.5)
        {
            ChangeAlpha(Mathf.InverseLerp(0.8f, 0.5f, t));
        }

        CheckOutScreen(transform.position);

    }

    private void CheckOutScreen(Vector3 position)
    {
        Vector3 viewPort = Camera.main.WorldToViewportPoint(position);
        
        if (viewPort.x < -minPosition || viewPort.x > maxPosition) direction.x *= -1;
        if (viewPort.y < -minPosition || viewPort.y > maxPosition) direction.y *= -1;
    }    

    private void Reset_Star()
    {
        timeLive = Random.Range(starData.timelive_min, starData.timelive_max);
        timeRemaining = timeLive;

        speedStar = Random.Range(starData.min_speed, starData.max_speed);
        angle = Random.Range(0, 360);

        scale = Random.Range(starData.scale_min, starData.scale_max);
        transform.localScale = new Vector2(scale, scale);

        direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        ChangeAlpha(0);
    }

    private void ChangeAlpha(float value)
    {   
        color = sr.color;
        color.a = Mathf.Clamp01(value);
        sr.color = color;
    }    

}
