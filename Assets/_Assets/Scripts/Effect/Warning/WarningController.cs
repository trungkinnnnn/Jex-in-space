
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarningController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject _prefabsWarningRed;
    [SerializeField] GameObject _prefabsWarningOrange;

    [Header("References")]
    [SerializeField] Transform _playerTransform;
    [SerializeField] LayerMask _asteroidLayerMark;

    [Header("Setting")]
    [SerializeField] float _warningRadius = 2f;
    [SerializeField] float _viewPortMinPlayer = 0.1f;
    [SerializeField] float _viewPortMaxPlayer = 0.9f;
    [SerializeField] float _viewPortMinAst = 0.05f;
    [SerializeField] float _viewPortMaxAst = 1.05f;
    [SerializeField] float _viewPortEdgeOffset = 0.01f;

    [Header("Performance")]
    [SerializeField] float _checkInterval = 0.02f;
    [SerializeField] int _overlapBuffSize = 15;
    private bool _hasTarget = false;   

    private Camera _camera;

    private readonly Dictionary<Ast, (GameObject warningObj, Edge edeg)> _warningObjs = new Dictionary<Ast, (GameObject warningObj, Edge edeg)>();

    private Collider2D[] _overlapResults;
    private float _timer = 0f;

    private void Awake()
    {
        _camera = Camera.main;
        _overlapResults = new Collider2D[_overlapBuffSize];
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer > 0) return;

        if (ViewPortMath.IsInSidePlayerViewPort(_camera, _playerTransform.position, _viewPortMinPlayer, _viewPortMaxPlayer))
        {
            //Debug.Log("Inside");
            ClearAllWarning();
            _hasTarget = false;
            _timer = _checkInterval;
            return;
        }

        if(!_hasTarget && _timer > 0) return;

        _hasTarget =  UpdateWarning();  
            
       _timer = _hasTarget ? 0f : _checkInterval;
            
    }


    private void ClearAllWarning()
    {
        foreach(var obj in _warningObjs.Values)
        {
            DestroySmoothWarning(obj.warningObj);
        } 
        _warningObjs.Clear();   
    }    

    private bool UpdateWarning()
    {
        int count = Physics2D.OverlapCircleNonAlloc(_playerTransform.position, _warningRadius, _overlapResults,_asteroidLayerMark);
        //Debug.Log("CheckCount : " + count);
        HashSet<Ast> astIncheck = new HashSet<Ast>();

        float vpPlayerZ = _camera.WorldToViewportPoint(_playerTransform.position).z;

        for (int i = 0; i < count; i++)
        {
            Ast ast = CheckAstValid(i);
            if(ast == null) continue;

            astIncheck.Add(ast);

            Vector3 vpAst = _camera.WorldToViewportPoint(ast.transform.position);
            Vector3 vpPlayer = _camera.WorldToViewportPoint(_playerTransform.position);

            var intersect = ViewPortMath.GetEdgeIntersection(vpPlayer, vpAst);
            if(!intersect.HasValue) continue;

            Edge edge = intersect.Value.edge;
            Vector3 edgePosViewPort = intersect.Value.position;


            // Nếu ast chưa có warning thì tạo
            if(!_warningObjs.TryGetValue(ast, out var tuple))
            {
                GameObject warningObj = Instantiate(ReturnObjWarning(ast.type));
                warningObj.transform.SetParent(this.transform, true);
                _warningObjs[ast] = (warningObj, edge);
            }    
            else
            {
                if(tuple.edeg != edge)
                {
                    _warningObjs[ast] = (tuple.warningObj, edge);
                }
            }

            Vector3 adjustedPos = ViewPortMath.ClampToEdgeWithOffset(edgePosViewPort, edge, _viewPortEdgeOffset);
            Vector3 warningWorldPos = _camera.ViewportToWorldPoint(new Vector3(adjustedPos.x, adjustedPos.y, vpPlayer.z));
            
            if(_warningObjs.TryGetValue(ast, out var entry) && entry.warningObj != null)
            {
                entry.warningObj.transform.position = warningWorldPos;
            }    

        }

        RemoveAstOut(astIncheck);

        return astIncheck.Count > 0;
    }

    private Ast CheckAstValid(int i)
    {
        Collider2D col = _overlapResults[i];
        if (col == null) return null;

        Ast ast = col.GetComponent<Ast>();
        if (ast == null) return null;

        if(!ViewPortMath.IsAsteroidOutSiteViewPort(_camera, ast.transform.position, _viewPortMinAst, _viewPortMaxAst)) return null;

        if(ast.type == AsteroidType.AstNon) return null;

        return ast;
    }

    private GameObject ReturnObjWarning(AsteroidType type)
    {
        return type == AsteroidType.AstNormal ? _prefabsWarningOrange : _prefabsWarningRed;
    }

    private void RemoveAstOut(HashSet<Ast> astIncheck)
    {
        var astRemove = new List<Ast>();
        foreach(var key in _warningObjs.Keys)
        {
            if(!astIncheck.Contains(key))
            {
                DestroySmoothWarning(_warningObjs[key].warningObj);
                astRemove.Add(key);
            }    
        }   
        
        foreach(var obj in astRemove)
        {
            _warningObjs.Remove(obj);
        }    

    }   

    private void DestroySmoothWarning(GameObject obj)
    {
        if (obj == null) return;    

        Warning w = obj.GetComponent<Warning>();
        if (w == null) return;

        w.SmoothDestroy();
    }    

}
