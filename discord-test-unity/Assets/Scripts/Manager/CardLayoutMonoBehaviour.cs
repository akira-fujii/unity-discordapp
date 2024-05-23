using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BYNetwork
{
    public static class CardLayoutUtil
    {
        private const float GrabPosY = 1.9f;
        // ステート状況に応じて、(自分の)カード位置を一括更新する
    
        // カード座標をスナップする
        
        // 自由ドラッグ
        public static Vector3 ToFreeGrabPos(Vector3 pos)
        {
            var newPos = pos;
            newPos.y = GrabPosY;
            return newPos;
        }
        
    }
}