using UnityEngine;

namespace Assets.Scripts.Model
{
    public class Shot
    {
        private float damage;
        public float Damage
        {
            get { return damage; }
            private set { damage = value; }
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

        public Shot(float damage, Vector3 impactPoint, Vector3 originPoint)
        {
            Damage = damage;
            ImpactPoint = impactPoint;
            OriginPoint = originPoint;
        }
        public Shot(float modifier, Shot shot)
        {
            Damage = shot.Damage * modifier;
            ImpactPoint = shot.impactPoint;
            OriginPoint = shot.originPoint;
        }
    }
}
