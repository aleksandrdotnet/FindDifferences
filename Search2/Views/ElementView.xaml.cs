using System.Windows;
using System.Windows.Media;

namespace Search2.Views
{
    public partial class ElementView
    {
        public static readonly DependencyProperty BrushProperty = DependencyProperty.Register(
            "Brush", typeof(SolidColorBrush), typeof(ElementView), new PropertyMetadata(default(SolidColorBrush)));

        public SolidColorBrush Brush
        {
            get { return (SolidColorBrush) GetValue(BrushProperty); }
            set { SetValue(BrushProperty, value); }
        }
        public ElementView()
        {
            InitializeComponent();
        }
    }
}
