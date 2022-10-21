using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shingrix.Mode
{
    public interface IMode
    {
        public void Enter();
        public void Leave();
    }
}