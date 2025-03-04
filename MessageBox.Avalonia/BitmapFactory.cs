using Avalonia.Media.Imaging;
using Avalonia.Platform;

using System;

namespace MessageBoxSlim.Avalonia
{
    public static class BitmapFactory
    {
        public static Bitmap Load(Uri uri)
        {
            using (var stream = AssetLoader.Open(uri))
            {
                return new Bitmap(stream);
            }
        }

        public static Bitmap Load(string uri)
        {
            using (var stream = AssetLoader.Open(new Uri(uri)))
            {
                return new Bitmap(stream);
            }
        }
    }
}
