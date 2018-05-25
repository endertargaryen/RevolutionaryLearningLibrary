using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace RevolutionaryLearningLibrary.Controllers
{
	public class HelperMethods
	{
		private const double MAX_WIDTH = 400;

		public static int GetAuthenticationId(IPrincipal user)
		{
			string idValue = user.Identity.GetUserId();

			return Convert.ToInt32(idValue);
		}

		public static void ResizeImage(string path)
		{
			Bitmap destImage = null;

			using (Image image = Image.FromFile(path))
			{
				double divisor = MAX_WIDTH / (double)image.Width;

				int width = (int)((double)image.Width * divisor);
				int height = (int)((double)image.Height * divisor);

				var destRect = new Rectangle(0, 0, width, height);
				destImage = new Bitmap(width, height);

				destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

				using (var graphics = Graphics.FromImage(destImage))
				{
					graphics.CompositingMode = CompositingMode.SourceCopy;
					graphics.CompositingQuality = CompositingQuality.Default;
					graphics.InterpolationMode = InterpolationMode.Default;
					graphics.SmoothingMode = SmoothingMode.Default;
					graphics.PixelOffsetMode = PixelOffsetMode.Default;

					using (var wrapMode = new ImageAttributes())
					{
						wrapMode.SetWrapMode(WrapMode.Clamp);
						graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
					}
				}
			}

			// phone compatibility
			destImage.RotateFlip(RotateFlipType.Rotate90FlipNone);

			destImage.Save(path);
		}
	}
}