using System.Windows.Media;

namespace PromptFavorites.Helpers
{
    /// <summary>全局配色：浅蓝强调色 + 中性灰，悬浮用更深的同色系保证对比度。</summary>
    public static class Theme
    {
        public static readonly Color Accent = Color.FromRgb(0x3D, 0x9B, 0xD9);
        public static readonly Color AccentHover = Color.FromRgb(0x1C, 0x76, 0xB4);
        public static readonly Color Neutral = Color.FromRgb(0xC8, 0xC8, 0xD2);
        public static readonly Color NeutralHover = Color.FromRgb(0x9A, 0xC9, 0xEE);
        public static readonly Color Favorite = Color.FromRgb(0xFF, 0xB8, 0x00);
        public static readonly Color FavoriteEnd = Color.FromRgb(0xFF, 0x8C, 0x00);
        public static readonly Color FavoriteHover = Color.FromRgb(0xE0, 0x8A, 0x00);

        public static readonly SolidColorBrush AccentBrush = Frozen(Accent);
        public static readonly SolidColorBrush AccentHoverBrush = Frozen(AccentHover);
        public static readonly SolidColorBrush NeutralBrush = Frozen(Neutral);
        public static readonly SolidColorBrush NeutralHoverBrush = Frozen(NeutralHover);
        public static readonly SolidColorBrush FavoriteHoverBrush = Frozen(FavoriteHover);

        private static SolidColorBrush Frozen(Color c)
        {
            var b = new SolidColorBrush(c);
            b.Freeze();
            return b;
        }
    }
}
