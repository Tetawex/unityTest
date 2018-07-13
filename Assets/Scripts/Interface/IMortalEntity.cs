using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Interface
{
    public interface IMortalEntity
    {
        bool Dead { get; }
        void Die();
    }
}
