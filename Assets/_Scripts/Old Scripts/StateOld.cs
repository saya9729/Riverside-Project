using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbstractClass
{
    public abstract class StateOld : MonoBehaviour
    {
        public abstract void EnterState();
        public abstract void UpdateState();
        public abstract void ExitState();
        public abstract void PhysicsUpdateState();
    }
}
