using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Great_Space_Race.Objects
{
    public static class Helpers
    {
        public static BEPUutilities.Matrix toBEPU(this Microsoft.Xna.Framework.Matrix mat) 
        {
            BEPUutilities.Matrix ret = new BEPUutilities.Matrix();

            ret.M11 = mat.M11;
            ret.M12 = mat.M12;
            ret.M13 = mat.M13;
            ret.M14 = mat.M14;
            ret.M21 = mat.M21;
            ret.M22 = mat.M22;
            ret.M23 = mat.M23;
            ret.M24 = mat.M24;
            ret.M31 = mat.M31;
            ret.M32 = mat.M32;
            ret.M33 = mat.M33;
            ret.M34 = mat.M34;
            ret.M41 = mat.M41;
            ret.M42 = mat.M42;
            ret.M43 = mat.M43;
            ret.M44 = mat.M44;

            return ret;
        }

        public static Microsoft.Xna.Framework.Matrix toXNA(this BEPUutilities.Matrix mat)
        {
            Microsoft.Xna.Framework.Matrix ret = new Microsoft.Xna.Framework.Matrix();

            ret.M11 = mat.M11;
            ret.M12 = mat.M12;
            ret.M13 = mat.M13;
            ret.M14 = mat.M14;
            ret.M21 = mat.M21;
            ret.M22 = mat.M22;
            ret.M23 = mat.M23;
            ret.M24 = mat.M24;
            ret.M31 = mat.M31;
            ret.M32 = mat.M32;
            ret.M33 = mat.M33;
            ret.M34 = mat.M34;
            ret.M41 = mat.M41;
            ret.M42 = mat.M42;
            ret.M43 = mat.M43;
            ret.M44 = mat.M44;

            return ret;
        }
    }
}
