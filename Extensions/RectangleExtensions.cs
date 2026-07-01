using Exfal.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exfal.Extensions
{
    public static class RectangleExtensions
    {
        public static RectangleF ToRectangleF(in this Rectangle r)
        {
            return new(r.X, r.Y, r.Width, r.Height);
        } 

    }
}
