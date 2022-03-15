using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core{
    public interface IPolyCounterPart
    {
       GameObject GetPolyCounterPart();
        void UpdateStats();
        void SetPoly(GameObject poly);
    }
}

