using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bullethell
{
    abstract class MovementPattern
    {
        public abstract void UpdateDirection(GameObject enemy);
    }
}