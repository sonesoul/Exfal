using System.Collections.Generic;

namespace Exfal.Drawing
{
    public class CameraCollection : SortedDictionary<int, Camera>
    {
        public int PopKey()
        {
            int expected = 0;

            foreach (var k in Keys)
            {
                if (k > expected)
                {
                    break;
                }

                expected++;
            }

            return expected;
        }
    }
}