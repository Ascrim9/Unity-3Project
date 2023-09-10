using UnityEngine;

namespace FlowerEndSummer
{
    public class DynamicSettings : DynamicParkourSettings
    {
        public override bool CheckDir(WolfRaycastData wolfData, Transform player)
        {
            if (!base.CheckDir(wolfData, player))
            {
                return false;
            }

            var hitPoint = wolfData.forwardRaycastHit.transform.InverseTransformPoint(wolfData.forwardRaycastHit.point);

            if (hitPoint is { z: < 0, x: < 0 } || hitPoint is { z: > 0, x: > 0 })
            {
                mirror = true;
            }
            else
            {
                mirror = false;
            }
        }
    }
}