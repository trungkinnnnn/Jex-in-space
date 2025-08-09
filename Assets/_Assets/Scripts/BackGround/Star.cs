using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
public class Star : MonoBehaviour
{

    // Data
    [SerializeField] StarScripTable _starData;

    private SpriteRenderer _sr;

    // moving
    private float _timeLive;
    private float _timeRemaining;
    private float _speedStar;
    private float _angle;
    private float _scale;
    private Vector3 _direction;

    public float minPosition = 0.1f;
    public float maxPosition = 1.1f;

    // alpha
    private Color _color;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Reset_Star();

        ChangeAlpha(0);
    }

    void Update()
    {
        transform.position += _direction * _speedStar * Time.deltaTime;

        _timeRemaining -= Time.deltaTime;
        if (_timeRemaining < 0f)
        {
            Reset_Star();
        }

        float t = _timeRemaining/_timeLive;
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
        
        if (viewPort.x < -minPosition || viewPort.x > maxPosition) _direction.x *= -1;
        if (viewPort.y < -minPosition || viewPort.y > maxPosition) _direction.y *= -1;
    }    

    private void Reset_Star()
    {
        _timeLive = Random.Range(_starData.timelive_min, _starData.timelive_max);
        _timeRemaining = _timeLive;

        _speedStar = Random.Range(_starData.min_speed, _starData.max_speed);
        _angle = Random.Range(0, 360);

        _scale = Random.Range(_starData.scale_min, _starData.scale_max);
        transform.localScale = new Vector2(_scale, _scale);

        _direction = new Vector2(Mathf.Cos(_angle * Mathf.Deg2Rad), Mathf.Sin(_angle * Mathf.Deg2Rad));
        ChangeAlpha(0);
    }

    private void ChangeAlpha(float value)
    {   
        _color = _sr.color;
        _color.a = Mathf.Clamp01(value);
        _sr.color = _color;
    }    

}
