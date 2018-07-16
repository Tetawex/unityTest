using Assets.Scripts.Controller;
using Assets.Scripts.Interface;
using Assets.Scripts.Model;
using Assets.Scripts.Util;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Controller
{
    public class EnemyHitboxController : MonoBehaviour, IDamageableEntity
    {
        public BodyPart bodyPart;

        //how to put a fkin interface in there???
        public EnemyController Root;

        private IDictionary<BodyPart, float> bodyPartMultiplierDictionary;


        public EnemyHitboxController()
        {
            bodyPartMultiplierDictionary = new Dictionary<BodyPart, float>();
            bodyPartMultiplierDictionary[BodyPart.HEAD] = 2f;
            bodyPartMultiplierDictionary[BodyPart.TORSO] = 1f;
            bodyPartMultiplierDictionary[BodyPart.LEG] = 1f;
            bodyPartMultiplierDictionary[BodyPart.ARM] = 1f;
            bodyPartMultiplierDictionary[BodyPart.ARMORED_TORSO] = 1f;
        }
        public void ReceiveShot(Shot shot)
        {
            Root.ReceiveShot(new LocationalShot(bodyPartMultiplierDictionary[bodyPart], bodyPart, shot));
            if (bodyPart == BodyPart.ARMORED_TORSO) ;
            //bodyPart = BodyPart.TORSO;
        }

        // Use this for initialization
        void Start()
        {
            if (PlatformTypeUtil.IsMobileplatform())
            {
                transform.localScale *= 1.7f;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
