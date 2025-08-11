
using System.Collections.Generic;
using UnityEngine;

public class WarningController : MonoBehaviour
{
    [SerializeField] GameObject _prefabsWarningRed;
    [SerializeField] GameObject _prefabsWarningOrange;
    [SerializeField] Transform _positionPlayer;
    [SerializeField] LayerMask _layerAst;
    [SerializeField] float _warningRadius = 2f;
    [SerializeField] float _minPositionCheckPlayer = 0.1f;
    [SerializeField] float _maxPositionCheckPlayer = 0.9f;
    [SerializeField] float _minPositionCheckAst = 0.05f;
    [SerializeField] float _maxPositionCheckAst = 1.05f;
    [SerializeField] float _edgeOffset = 0.01f;


    private Dictionary<Ast, (GameObject warningObj, Edge edeg)> _warningObjs = new Dictionary<Ast, (GameObject warningObj, Edge edeg)>();

    private void Update()
    {
        if (!CheckPositionPlayer(_positionPlayer.position))
        {
            ClearAllWarning();
            return;
        }    
        Debug.Log("Warning");
        CheckWarnings();
    }

    private void ClearAllWarning()
    {
        foreach(var obj in _warningObjs.Values)
        {
            Destroy(obj.warningObj);
        } 
        _warningObjs.Clear();   
    }    

    private bool CheckPositionPlayer(Vector3 pos)
    {
        Vector3 posPlayer = Camera.main.WorldToViewportPoint(pos);
        if (posPlayer.x > _minPositionCheckPlayer && posPlayer.x < _maxPositionCheckPlayer &&
            posPlayer.y > _minPositionCheckPlayer && posPlayer.y < _maxPositionCheckPlayer)return false;
        return true;
    }

    private bool CheckPositionAst(Vector3 pos)
    {
        Vector3 posAst = Camera.main.WorldToViewportPoint(pos);
        if (posAst.x < -_minPositionCheckAst || posAst.x > _maxPositionCheckAst ||
            posAst.y < -_minPositionCheckAst || posAst.y > _maxPositionCheckAst) return true;
        return false;
    }

    private void CheckWarnings()
    {
        Collider2D[] nearbyAsts = Physics2D.OverlapCircleAll(_positionPlayer.position, _warningRadius, _layerAst);
        Debug.Log("CheckCount : " + nearbyAsts.Length);
        HashSet<Ast> astIncheck = new HashSet<Ast>();

        Vector3 vpPlayer = Camera.main.WorldToViewportPoint(_positionPlayer.position);

        foreach(Collider2D c in nearbyAsts)
        {
            Ast ast = c.GetComponent<Ast>();
            if(ast == null || !CheckPositionAst(ast.transform.position) || ast.type == AsteroidType.AstNon) continue;

            astIncheck.Add(ast);

            Vector3 vpAst = Camera.main.WorldToViewportPoint(c.transform.position);

            Edge edge;
            Vector3 edgePosViewPort;

            if(_warningObjs.TryGetValue(ast, out var tuple))
            {
                edge = tuple.edeg;
                edgePosViewPort = CalculateEdgePosition(vpAst, vpPlayer, edge);
            }    
            else
            {
                // Khởi tạo và gán giá trị struct
                var intersection = GetEdgeIntersection(vpPlayer, vpAst);
                if(!intersection.HasValue) continue;

                edge = intersection.Value.edge;
                edgePosViewPort = intersection.Value.position;

                // Nếu chưa có key, tạo key và gán giá trị
                if (!_warningObjs.ContainsKey(ast))
                {
                    GameObject warningObj = Instantiate(ReturnObjWarning(ast.type));
                    _warningObjs[ast] = (warningObj, edge);
                }    
            }

            Vector3 adjustedPos = AdjustPositionInViewport(edgePosViewPort, edge, _edgeOffset);
            Vector3 warningWorldPos = Camera.main.ViewportToWorldPoint(new Vector3(adjustedPos.x, adjustedPos.y, vpPlayer.z));
            _warningObjs[ast].warningObj.transform.position = warningWorldPos;

        }

        RemoveAstOut(astIncheck);
    }


    /*
        Ta có ViewPort [0, 1]
        Vì Vector 3 dir = vpAst - vpPlayer (Khi quy đổi ra đơn vị viewport ta hiểu rằng 1 điểm đi từ vpPlayer = 0 đến vpAst = 1)
        Từ đó ta có công thức tổng quát :
        P(t) = vpPlayer + t * dir (Phương trình đường thẳng ax + b = y), Vì ta bắt đầu từ vpPlayer => P(0) = 0
        0 = vpPlayer + t * dir <=> t = (0 - vpPlayer)/dir
        
        Xuất phát từ đó ta có:
        
        P_x(t) = vpPlayer_x + t * dir_x (1)
        P_y(t) = vpPlayer_y + t * dir_y (2)
        
        Vì đây là ví dụ xét trục left nên => P_x = 0 như vây:
        t = (0 - vpPlayer_x)/dir_x 
        Thay vào phương trình 2 ta được:
        P_y(t) = vpPlayer_y + (0 - vpPlayer_x) / dir_x * dir_y 

        Tương tự với các cạnh còn lại
     */

    private Vector3 CalculateEdgePosition(Vector3 vpAst, Vector3 vpPlayer, Edge edge)
    {
        Vector3 dir = vpAst - vpPlayer;
        switch(edge)
        {
            case Edge.left:
                return new Vector3(0f, vpPlayer.y + (0f - vpPlayer.x) / dir.x * dir.y, 0f);
            case Edge.right:
                return new Vector3(1f, vpPlayer.y + (1f - vpPlayer.x) / dir.x * dir.y, 0f);
            case Edge.top:
                return new Vector3(vpPlayer.x + (1f - vpPlayer.y) / dir.y * dir.x, 1f, 0f);
            case Edge.bottom:
                return new Vector3(vpPlayer.x + (0f - vpPlayer.y) / dir.y * dir.x, 0f, 0f);
            default:
                return vpAst;
        }
    }    

    private Vector3 AdjustPositionInViewport(Vector3 pos, Edge edge, float offSet)
    {
        Vector3 adjusted = pos;
        switch(edge)
        {
            case Edge.left:
                adjusted.x = offSet;
                break;
            case Edge.right:
                adjusted.x = 1 - offSet;
                break;
            case Edge.top:
                adjusted.y = 1f - offSet;
                break;
            case Edge.bottom:
                adjusted.y = offSet;
                break;
        }
        return adjusted;
    }

    // Tìm xem đường thẳng dir cắt cạnh nào ???
    private EdgeIntersection? GetEdgeIntersection(Vector3 vpPlayer, Vector3 vpAst)
    {
        Vector3 direction = vpAst - vpPlayer;

        // left
        if(Mathf.Abs(direction.x) > 0.001f)
        {
            float t = (0f - vpPlayer.x) / direction.x;
            if(t >= 0f && t <= 1f)
            {
                Vector3 intersect = vpPlayer + t * direction;
                if (intersect.y >= 0 && intersect.y <= 1f)
                    return new EdgeIntersection(vpPlayer, Edge.left);
            }    
        }

        // right
        if (Mathf.Abs(direction.x) > 0.001f)
        {
            float t = (1f - vpPlayer.x) / direction.x;
            if (t >= 0f && t <= 1f)
            {
                Vector3 intersect = vpPlayer + t * direction;
                if (intersect.y >= 0 && intersect.y <= 1f)
                    return new EdgeIntersection(vpPlayer, Edge.right);
            }
        }

        // bottom
        if(Mathf.Abs(direction.y) > 0.001f)
        {
            float t = (0f - vpPlayer.y) / direction.y;
            if (t >= 0f && t <= 1)
            {
                Vector3 intersect = vpPlayer + t * direction;
                if(intersect.x >= 0 && intersect.x <= 1f)
                    return new EdgeIntersection(vpPlayer, Edge.bottom);   
            }    
        }

        // top
        if (Mathf.Abs(direction.y) > 0.001f)
        {
            float t = (1f - vpPlayer.y) / direction.y;
            if (t >= 0f && t <= 1)
            {
                Vector3 intersect = vpPlayer + t * direction;
                if (intersect.x >= 0 && intersect.x <= 1f)
                    return new EdgeIntersection(vpPlayer, Edge.top);
            }
        }
        return null;

    }

    private void RemoveAstOut(HashSet<Ast> astIncheck)
    {
        var astRemove = new List<Ast>();
        foreach(var key in _warningObjs.Keys)
        {
            if(!astIncheck.Contains(key))
            {
                Destroy(_warningObjs[key].warningObj);
                astRemove.Add(key);
            }    
        }   
        
        foreach(var obj in astRemove)
        {
            _warningObjs.Remove(obj);
        }    

    }   
    private GameObject ReturnObjWarning(AsteroidType type)
    {
        return type == AsteroidType.AstNormal ? _prefabsWarningOrange : _prefabsWarningRed;
    }    

}
