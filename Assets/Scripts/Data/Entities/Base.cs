using System;
using TD.Logic.RuntimeState;
using UnityEngine;

namespace TD.Data.Entities
{
    [Serializable, CreateAssetMenu(menuName = "TD/Entities/Base")]
    public class Base : Entity
    {
        public override EntityState BuildState()
        {
            return new EntityState(this);
        }
    }
}