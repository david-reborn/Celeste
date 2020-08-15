using UnityEngine;
using System.Collections;


namespace myd.celeste.demo
{
    public static class ColliderUtil
    {
        public static Collider2D Collide2DWithRay(Vector2 position, Vector2 direct, float distance)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, direct, distance);
            return hit.collider;
        }

        public static Collider2D OverlapBox(BoxCollider2D source, Vector2 direct, float distance, float angle, LayerMask mask)
        {
            Vector2 point = (Vector2)source.gameObject.transform.position + source.offset + direct * distance;
            Collider2D target = Physics2D.OverlapBox(point, source.size - Vector2.one * 0.05f, angle, mask);
            return target;
        }

        public static Collider2D OverlapBox(BoxCollider2D source, Vector2 point, float angle, LayerMask mask)
        {
            point += source.offset;
            Collider2D target = Physics2D.OverlapBox(point, source.size - Vector2.one * 0.05f, angle, mask);
            return target;
        }

        public static bool CollidePoint(Vector2 position, LayerMask mask)
        {
            return Physics2D.OverlapPoint(position, mask);
        }

        public static bool CollideCheck(BoxCollider2D collider, Vector2 point, LayerMask mask)
        {
            point = point + collider.offset;
            Collider2D target = Physics2D.OverlapBox(point, collider.size - Vector2.one * 0.05f, 0, mask);
            return target != null;
        }

        public static Collider2D CollideFirst(BoxCollider2D source, Vector2 direct, float distance, float angle, LayerMask mask)
        {
            return OverlapBox(source, direct, distance, angle, mask);
        }
    }
}