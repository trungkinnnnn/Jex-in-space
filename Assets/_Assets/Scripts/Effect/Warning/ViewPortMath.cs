
using UnityEngine;

public static class ViewPortMath
{
    
    public static bool IsInSidePlayerViewPort(Camera cam, Vector3 worldPos, float min, float max)
    {
        Vector3 vp = cam.WorldToViewportPoint(worldPos);
        return (vp.x > min && vp.x < max && vp.y > min && vp.y < max);
    }

    public static bool IsAsteroidOutSiteViewPort(Camera cam, Vector3 worldPos, float min, float max)
    {
        Vector3 vp = cam.WorldToViewportPoint(worldPos);
        return (vp.x < -min || vp.x > max || vp.y < -min || vp.y > max);
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

    public static EdgeIntersection? GetEdgeIntersection(Vector3 vpPlayer, Vector3 vpAst)
    {
        Vector3 direction = vpAst - vpPlayer;
        const float EPS = 0.001f;

        // left => x = 0
        if(Mathf.Abs(direction.x) > EPS)
        {
            float t = (0f - vpPlayer.x) / direction.x;
            if(t >= 0f && t <= 1f)
            {
                Vector3 intersection = vpPlayer + t * direction;
                if (intersection.y >= 0f && intersection.y <= 1f)
                    return new EdgeIntersection(intersection, Edge.left);
            }
        }

        // right => x = 1
        if (Mathf.Abs(direction.x) > EPS)
        {
            float t = (1f - vpPlayer.x) / direction.x;
            if (t >= 0f && t <= 1f)
            {
                Vector3 intersect = vpPlayer + t * direction;
                if (intersect.y >= 0f && intersect.y <= 1f)
                    return new EdgeIntersection(intersect, Edge.right);
            }
        }

        // bottom => y = 0
        if (Mathf.Abs(direction.y) > EPS)
        {
            float t = (0f - vpPlayer.y) / direction.y;
            if(t >= 0f && t <= 1f)
            {
                Vector3 intersection = vpPlayer + t * direction;
                if(intersection.x >= 0f && intersection.x <= 1f)    
                    return new EdgeIntersection(intersection, Edge.bottom);
            }
        }

        // top => y = 1
        if(Mathf.Abs(direction.y) > EPS)
        {
            float t = (1f - vpPlayer.y) / direction.y;
            if (t >= 0f && t <= 1f)
            {
                Vector3 intersection = vpPlayer + t * direction;
                if(intersection.x >= 0f && intersection.x <= 1f) 
                    return new EdgeIntersection (intersection, Edge.top);
            }
        }
        return null;

    }


    // Trừ đi 1 khoảng để vị trí warning nằm trong màn hình
    public static Vector3 ClampToEdgeWithOffset(Vector3 viewPortPos, Edge edge, float offset)
    {
        Vector3 v = viewPortPos;
        switch(edge)
        {
            case Edge.left:
                v.x = Mathf.Clamp(offset, 0f, 1f);
                break;
            case Edge.right:
                v.x = Mathf.Clamp(1 - offset, 0f, 1f);
                break;
            case Edge.bottom:
                v.y = Mathf.Clamp(0f, offset, 1f);
                break;
            case Edge.top:
                v.y = Mathf.Clamp(0f, 1 - offset, 1f);
                break;
        }
        return v;
    }

}
