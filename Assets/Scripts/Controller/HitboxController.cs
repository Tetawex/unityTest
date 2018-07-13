using Assets.Scripts.Controller;
using Assets.Scripts.Interface;
using Assets.Scripts.Model;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Controller
{
    public class HitboxController : MonoBehaviour, IDamageableEntity
    {
        public BodyPart bodyPart;

        //how to put a fkin interface in there???
        public EnemyController Root;

        private IDictionary<BodyPart, float> bodyPartMultiplierDictionary;


        public HitboxController()
        {
            bodyPartMultiplierDictionary = new Dictionary<BodyPart, float>();
            bodyPartMultiplierDictionary[BodyPart.HEAD] = 2f;
            bodyPartMultiplierDictionary[BodyPart.TORSO] = 1f;
            bodyPartMultiplierDictionary[BodyPart.LEG] = 0.7f;
            bodyPartMultiplierDictionary[BodyPart.ARM] = 0.7f;
            bodyPartMultiplierDictionary[BodyPart.ARMORED_TORSO] = 0.3f;
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

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
