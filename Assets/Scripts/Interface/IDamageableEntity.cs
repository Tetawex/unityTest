using Assets.Scripts.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Assets.Scripts.Interface
{
    public interface IDamageableEntity
    {
        void ReceiveShot(Shot shot);
    }
}
