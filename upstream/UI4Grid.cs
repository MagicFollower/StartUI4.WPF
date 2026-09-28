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
                    new GradientStop(Color.FromArgb(255, 225, 236, 245), 0.0), //#E1ECF5
                    new GradientStop(Color.FromArgb(255, 255, 255, 255), 1.0)  
                }
            };
        }
    }
}