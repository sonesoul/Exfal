using Microsoft.Xna.Framework;

namespace Exfal.Drawing
{
    public class RectScaler
    {
        public Rectangle Fit(in Point source, in Rectangle target)
        {
            Point size = target.Size;

            int targetWidth = size.X;
            int targetHeight = size.Y;

            float originalAspect = (float)source.X / source.Y;
            float targetAspect = (float)targetWidth / targetHeight;

            //target rect wider than original
            if (targetAspect > originalAspect)
            {
                int finalWidth = (int)(size.Y * originalAspect);

                Point finalLocation = new(
                    (targetWidth - finalWidth) / 2,
                    (targetHeight / 2) - (size.Y / 2));

                Point finalSize = new(finalWidth, size.Y);

                return new(finalLocation, finalSize);
            }
            //target rect higher than original
            else
            {
                int finalHeight = (int)(size.X / originalAspect);

                Point finalLocation = new(
                    (targetWidth / 2) - (size.X / 2),
                    (targetHeight - finalHeight) / 2);

                Point finalSize = new(size.X, finalHeight);

                return new(finalLocation, finalSize);
            }
        }
        public Rectangle Stretch(in Point _, in Rectangle target)
        {
            return target;
        }
    }
}