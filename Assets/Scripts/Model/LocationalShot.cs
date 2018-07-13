using UnityEngine;

namespace Assets.Scripts.Model
{
    public class LocationalShot
    {
        private float damage;
        public float Damage
        {
            get { return damage; }
            private set { damage = value; }
        }

        private BodyPart bodyPart;
        public BodyPart BodyPart
        {
            get { return bodyPart; }
            private set { bodyPart = value; }
        }

        private Vector3 impactPoint;
        public Vector3 ImpactPoint
        {
            get { return impactPoint; }
            private set { impactPoint = value; }
        }

        private Vector3 originPoint;
        public Vector3 OriginPoint
        {
            get { return originPoint; }
            private set { originPoint = value; }
        }

        public LocationalShot(float damage, Vector3 impactPoint, Vector3 originPoint, BodyPart bodyPart)
        {
            Damage = damage;
            ImpactPoint = impactPoint;
            OriginPoint = originPoint;
        }
        public LocationalShot(float modifier, BodyPart bodyPart, Shot shot)
        {
            Damage = shot.Damage * modifier;
            ImpactPoint = shot.ImpactPoint;
            OriginPoint = shot.OriginPoint;
            BodyPart = bodyPart;
        }
    }
}
