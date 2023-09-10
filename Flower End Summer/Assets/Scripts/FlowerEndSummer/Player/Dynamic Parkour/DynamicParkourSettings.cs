using UnityEngine;
using UnityEngine.Serialization;

namespace FlowerEndSummer
{
    [CreateAssetMenu(menuName = "Dynamic Parkour/New Parkour", fileName = "Add Parkour") ]
    public class DynamicParkourSettings : ScriptableObject
    {
        [SerializeField] private string animationName;
        [SerializeField] private float minHeight;
        [SerializeField] private float maxHeight;
        [SerializeField] protected bool mirror;

        //TargetMatch 해결
        [FormerlySerializedAs("enableTargetMatch")]
        [Header("Dynamic Tartget Match")]
        [SerializeField] private bool isTargetMatch = true;
        [SerializeField] private AvatarTarget avataMatch;
        [SerializeField] private float targetStartTime;
        [SerializeField] private float targetMatchTime;
        [SerializeField] private string parkourTag;
        [SerializeField] Vector3 matchPosWeight = new Vector3(0, 1, 0);
        
        public Vector3 MatchPos { get; set; }
        public string AnimationName => animationName;

        public bool IsTargetMatch => isTargetMatch;
        public AvatarTarget AvataMatch => avataMatch;
        public float TargetStartTime => targetStartTime;
        public float TargetMatchTime => targetMatchTime;
        public Vector3 MatchPosWeight => matchPosWeight;
        public virtual bool CheckDir(WolfRaycastData wolfData, Transform player)
        {
            if (!string.IsNullOrEmpty(parkourTag) && wolfData.forwardRaycastHit.transform.tag != parkourTag)
            {
                return false;
            }
            
            float height = wolfData.heightRaycastHit.point.y - player.position.y;
            Debug.Log(height);
            if (height < minHeight || height > maxHeight)
            {
                return false;
            }

            if (isTargetMatch is true)
            {
                MatchPos = wolfData.heightRaycastHit.point;
                Debug.Log(wolfData.heightRaycastHit.point);
            }
            
            return true;
        }
        
        
    }
}