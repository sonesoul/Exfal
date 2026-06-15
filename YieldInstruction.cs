using System;
using Exfal.Extensions;

namespace Exfal
{
    public class YieldInstruction
    {
        public IEnumerator WaitWhile(Func<bool> condition)
        {
            while (condition())
            {
                yield return null;
            }
        }

        public IEnumerator WaitForSeconds(float time)
        {
            while (time > 0)
            {
                time -= Time.Delta;
                yield return null;
            }
        }
        public IEnumerator WaitForRealSeconds(float time)
        {
            while (time > 0)
            {
                time -= Time.RealDelta;
                yield return null;
            }
        }

        public IEnumerator Interpolate(Func<float, float> action)
        {
            float elapsed = 0;

            while (elapsed >= 0 && elapsed <= 1)
            {
                elapsed = action(elapsed);

                if (elapsed < 0 || elapsed > 1)
                {
                    yield return null;

                    action(elapsed.Clamp01());

                    break;
                }

                yield return null;
            }
        }
    }
}