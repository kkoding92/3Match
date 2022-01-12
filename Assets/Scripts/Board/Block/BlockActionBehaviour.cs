﻿using KKoding92.Scriptable;
using KKoding92.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KKoding92.Board
{
    public class BlockActionBehaviour : MonoBehaviour
    {
        [SerializeField] BlockConfig m_BlockConfig;
        public bool isMoving { get; set; }

        Queue<Vector3> m_MovementQueue = new Queue<Vector3>(); //x=col, y=row, z = acceleration

        /*
         * 아래쪽으로 주어진 거리만큼 이동한다.
         * fDropDistance : 이동할 스텝 수 즉, 거리 (unit)   
         */
        public void MoveDrop(Vector2 vtDropDistance)
        {
            m_MovementQueue.Enqueue(new Vector3(vtDropDistance.x, vtDropDistance.y, 1));

            if (!isMoving)
            {
                StartCoroutine(DoActionMoveDrop());
            }
        }

        IEnumerator DoActionMoveDrop(float acc = 1.0f)
        {
            isMoving = true;

            while (m_MovementQueue.Count > 0)
            {
                Vector2 vtDestination = m_MovementQueue.Dequeue();

                int dropIndex = System.Math.Min(9, System.Math.Max(1, (int)Mathf.Abs(vtDestination.y)));
                float duration = m_BlockConfig.dropSpeed[dropIndex - 1];

                yield return CoStartDropSmooth(vtDestination, duration * acc);
            }

            isMoving = false;
            yield break;
        }

        IEnumerator CoStartDropSmooth(Vector2 vtDropDistance, float duration)
        {
            Vector3 to = new Vector3(transform.position.x + vtDropDistance.x, transform.position.y - vtDropDistance.y, transform.position.z);
            yield return Action2D.MoveTo(transform, to, duration);
        }
    }
}
