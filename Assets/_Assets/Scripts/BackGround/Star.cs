
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Data
    [SerializeField] StarScripTable _starData;
    [SerializeField] List<GameObject> _starObjs;

    public float minPosition = 0.1f;
    public float maxPosition = 1.1f;
    private struct StarState
    {
        public Vector3 localPosition;
        public Vector3 direction;
        public float timeLive;
        public float timeRemaining;
        public float speedStar;
        public float scale;
        public SpriteRenderer _sr;
    }

    private List<StarState> starStates = new List<StarState>();
    private Camera _camera;


    private void Awake()
    {
        starStates.Clear();
        int i = 0;
        foreach (var obj in _starObjs)
        {
            StarState s = new StarState();
            s._sr = obj.GetComponent<SpriteRenderer>();
            Reset_Star(ref s, i++);
            s.localPosition = obj.transform.localPosition;
            starStates.Add(s);
        }
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        for (int i = 0; i < starStates.Count; i++)
        {
            var s = starStates[i];
            UpdateStar(ref s, i);
            starStates[i] = s;
            _starObjs[i].transform.localPosition = s.localPosition;
        }

    }

    private void UpdateStar(ref StarState s, int index)
    {
        s.localPosition += s.direction * s.speedStar * Time.deltaTime;

        s.timeRemaining -= Time.deltaTime;
        if (s.timeRemaining < 0f)
        {
            Reset_Star(ref s, index);
        }

        float t = s.timeRemaining / s.timeLive;
        if (t < 0.2f)
        {
            ChangeAlpha(s._sr, Mathf.InverseLerp(0f, 0.2f, t));
        }
        else if (t > 0.5)
        {
            ChangeAlpha(s._sr, Mathf.InverseLerp(0.8f, 0.5f, t));
        }

        CheckOutScreen(ref s);
    }

    private void CheckOutScreen(ref StarState star)
    {

        if (_camera == null)
        {
            Debug.LogError("Camera.main not found!");
            return;
        }

        Vector3 viewPort = _camera.WorldToViewportPoint(star.localPosition);

        if (viewPort.x < -minPosition || viewPort.x > maxPosition) star.direction.x *= -1;
        if (viewPort.y < -minPosition || viewPort.y > maxPosition) star.direction.y *= -1;
    }

    private void Reset_Star(ref StarState star, int index)
    {
        star.timeLive = Random.Range(_starData.timelive_min, _starData.timelive_max);
        star.timeRemaining = star.timeLive;

        star.speedStar = Random.Range(_starData.min_speed, _starData.max_speed);
        float angle = Random.Range(0, 360);

        star.scale = Random.Range(_starData.scale_min, _starData.scale_max);

        star.direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

        _starObjs[index].transform.localScale = new Vector3(star.scale, star.scale, 1f);

        ChangeAlpha(star._sr, 0f);
    }

    private void ChangeAlpha(SpriteRenderer sprite, float value)
    {
        Color color = sprite.color;
        color.a = Mathf.Clamp01(value);
        sprite.color = color;
    }

}