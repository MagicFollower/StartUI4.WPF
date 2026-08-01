using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StartUI4Controls
{
    public class UI4Grid : Grid
    {
        public UI4Grid()
        {
            Background = new LinearGradientBrush
            {
                StartPoint = new Point(0.5, 0),
                EndPoint = new Point(0.5, 1),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop(Color.FromArgb(255, 238, 244, 248), 0.0), // #FFEEF4F8
                    new GradientStop(Color.FromArgb(255, 243, 243, 243), 1.0)  // #FFF3F3F3
                }
            };
        }
    }
}