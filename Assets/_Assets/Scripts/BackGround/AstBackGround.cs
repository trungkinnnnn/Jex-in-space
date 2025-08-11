using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AstBackGround : MonoBehaviour
{
    [SerializeField] AstScipTableBackGround _data;
    [SerializeField] List<GameObject> _astObjs;
    public float minViewPort = 0.4f;
    public float maxViewPort = 1.4f;

    public struct AstTrast
    {
        public Vector3 position;
        public Vector3 direction;
        public float speed;
        public float speedRotation;
        public float currentAngle;
        public int rotationDirection;
        public SpriteRenderer _sr;
    }

    private List<AstTrast> _astTrasts = new List<AstTrast>();
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
        _astTrasts.Clear();
        foreach (var obj in _astObjs)
        {
            AstTrast ast = new AstTrast();
            ast._sr = GetComponent<SpriteRenderer>();
            RandomStar(ref ast);
            ast.position = obj.transform.position;
            _astTrasts.Add(ast);
        }
    }

    void Update()
    {
        for(int i = 0; i < _astTrasts.Count; i++)
        {
            var s = _astTrasts[i];
            UpdateAstTrasts(ref s, i);
            _astTrasts[i] = s;
            _astObjs[i].transform.position = s.position;
        }
    }


    private void UpdateAstTrasts(ref AstTrast ast, int index)
    {
        ast.position += ast.direction * ast.speed * Time.deltaTime;

        if(ast.rotationDirection == 1)
            ast.currentAngle += ast.speedRotation * Time.deltaTime;
        else
            ast.currentAngle -= ast.speedRotation * Time.deltaTime;

        _astObjs[index].transform.rotation = Quaternion.Euler(0f, 0f, ast.currentAngle);
        CheckOutScreen(ref ast);
    }

    private void CheckOutScreen(ref AstTrast ast)
    {
        if (_camera == null)
        {
            Debug.LogError("Camera.main not found! Ast");
            return;
        }

        Vector3 viewPort = _camera.WorldToViewportPoint(ast.position);

        if (viewPort.x < -minViewPort || viewPort.x > maxViewPort) ast.direction.x *= -1;
        if (viewPort.y < -minViewPort || viewPort.y > maxViewPort) ast.direction.y *= -1;
    }

    private void RandomStar(ref AstTrast ast)
    {
        ast.speed = Random.Range(_data.speedMin, _data.speedMax);
        ast.speedRotation = Random.Range(_data.speedRotationMin, _data.speedRotationMax);
        float angle = Random.Range(0, 360);
        ast.direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        ast.currentAngle = angle;
        ast.rotationDirection = Random.Range(0, 2) == 0 ? 1 : -1;
    }

  }

