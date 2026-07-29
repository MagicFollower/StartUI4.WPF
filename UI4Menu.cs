using System;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
namespace StartUI4Controls
{
    public class ObjectIsStringConverter : IValueConverter
    {
        public static readonly ObjectIsStringConverter Instance = new ObjectIsStringConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is string;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class UI4Menu : Menu
    {
        public static readonly DependencyProperty BarBackgroundProperty =
            DependencyProperty.Register(
                nameof(BarBackground),
                typeof(Brush),
                typeof(UI4Menu),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(248, 248, 248))));
        public Brush BarBackground
        {
            get => (Brush)GetValue(BarBackgroundProperty);
            set => SetValue(BarBackgroundProperty, value);
        }
        public static readonly DependencyProperty ItemHoverBrushProperty =
            DependencyProperty.Register(
                nameof(ItemHoverBrush),
                typeof(Brush),
                typeof(UI4Menu),
                new PropertyMetadata(new SolidColorBrush(Color.FromArgb(20, 0, 0, 0))));
        public Brush ItemHoverBrush
        {
            get => (Brush)GetValue(ItemHoverBrushProperty);
            set => SetValue(ItemHoverBrushProperty, value);
        }
        public static readonly DependencyProperty PopupCornerRadiusProperty =
            DependencyProperty.Register(
                nameof(PopupCornerRadius),
                typeof(CornerRadius),
                typeof(UI4Menu),
                new PropertyMetadata(new CornerRadius(6)));
        public CornerRadius PopupCornerRadius
        {
            get => (CornerRadius)GetValue(PopupCornerRadiusProperty);
            set => SetValue(PopupCornerRadiusProperty, value);
        }
        public static readonly DependencyProperty TextForegroundProperty =
            DependencyProperty.Register(
                nameof(TextForeground),
                typeof(Brush),
                typeof(UI4Menu),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(20, 20, 20))));
        public Brush TextForeground
        {
            get => (Brush)GetValue(TextForegroundProperty);
            set => SetValue(TextForegroundProperty, value);
        }
        private static bool _stylesInitialized = false;
        private static readonly object _lockObj = new object();
        static UI4Menu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UI4Menu), new FrameworkPropertyMetadata(typeof(UI4Menu)));
        }
        public UI4Menu()
        {
            EnsureStylesInitialized();
        }
        private static void EnsureStylesInitialized()
        {
            if (_stylesInitialized) return;
            lock (_lockObj)
            {
                if (_stylesInitialized) return;
                InitStyles();
                _stylesInitialized = true;
            }
        }
        private static void InitStyles()
        {
            string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            string xaml = $@"
<ResourceDictionary xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                    xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                    xmlns:ui='clr-namespace:StartUI4Controls;assembly={assemblyName}'>
    <ui:ObjectIsStringConverter x:Key='ObjectIsStringConv' />
    <Style TargetType='{{x:Type ui:UI4Menu}}'>
        <Setter Property='Background' Value='{{Binding BarBackground, RelativeSource={{RelativeSource Self}}}}' />
        <Setter Property='BorderThickness' Value='0' />
        <Setter Property='Padding' Value='4,2' />
        <Setter Property='FontSize' Value='13' />
        <Setter Property='ItemsPanel'>
            <Setter.Value>
                <ItemsPanelTemplate>
                    <StackPanel Orientation='Horizontal' />
                </ItemsPanelTemplate>
            </Setter.Value>
        </Setter>
        <Setter Property='Template'>
            <Setter.Value>
                <ControlTemplate TargetType='{{x:Type ui:UI4Menu}}'>
                    <Border Background='{{TemplateBinding Background}}' Padding='{{TemplateBinding Padding}}'>
                        <ItemsPresenter />
                    </Border>
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <Style TargetType='{{x:Type ui:UI4MenuElementItem}}'>
        <Setter Property='Foreground' Value='{{Binding TextForeground, RelativeSource={{RelativeSource AncestorType=ui:UI4Menu}}}}' />
        <Setter Property='Padding' Value='10,8' />
        <Setter Property='BorderThickness' Value='0' />
        <Setter Property='Cursor' Value='Hand' />
        <Setter Property='MinWidth' Value='80' />
        <Setter Property='Template'>
            <Setter.Value>
                <ControlTemplate TargetType='{{x:Type ui:UI4MenuElementItem}}'>
    <Border x:Name='PART_Border' CornerRadius='4' Background='Transparent' Padding='{{TemplateBinding Padding}}'>
        <Grid x:Name='PART_RootGrid'>
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width='Auto' />
                <ColumnDefinition Width='*' />
                <ColumnDefinition Width='Auto' />
            </Grid.ColumnDefinitions>
            <Grid Grid.Column='0' x:Name='PART_IconGrid' 
      Width='20' VerticalAlignment='Center' HorizontalAlignment='Center'>   
    <TextBlock x:Name='TextIcon' 
               Text='{{Binding TextIcon, RelativeSource={{RelativeSource TemplatedParent}}}}' 
               FontFamily='{{Binding IconFontFamily}}'
               FontSize='{{Binding IconFontSize}}' 
               Foreground='{{Binding IconForeground}}'
               VerticalAlignment='Center' HorizontalAlignment='Center' />
</Grid>
            <ContentPresenter Grid.Column='1' x:Name='PART_Content' 
                              ContentSource='Header' 
                              VerticalAlignment='Center' 
                              HorizontalAlignment='Left' 
                              Margin='10,0,0,0' />
            <TextBlock Grid.Column='2' x:Name='PART_KeyTipText' VerticalAlignment='Center' Foreground='#666666' Margin='0,0,0,0' FontSize='12'
                       Text='{{Binding KeyTip, RelativeSource={{RelativeSource TemplatedParent}}}}' Visibility='Collapsed'/>
            <Popup x:Name='PART_Popup' 
                   AllowsTransparency='True'
                   IsOpen='{{Binding IsSubmenuOpen, RelativeSource={{RelativeSource TemplatedParent}}}}'
                   Placement='Bottom'
                   PopupAnimation='Slide'
                   VerticalOffset='2'>
                <Border Background='White'
                        MinWidth='160'
                        CornerRadius='{{Binding PopupCornerRadius, RelativeSource={{RelativeSource AncestorType=ui:UI4Menu}}}}'
                        Padding='4'>
                    <Border.Effect>
                        <DropShadowEffect Color='#000000' Opacity='0.12' BlurRadius='8' ShadowDepth='2' />
                    </Border.Effect>
                    <ItemsPresenter />
                </Border>
            </Popup>
        </Grid>
    </Border>
    <ControlTemplate.Triggers>
        <Trigger Property='Role' Value='SubmenuHeader'>
            <Setter TargetName='PART_Popup' Property='Placement' Value='Right' />
            <Setter TargetName='PART_Popup' Property='HorizontalOffset' Value='2' />
            <Setter TargetName='PART_Popup' Property='VerticalOffset' Value='-2' />
            <Setter TargetName='PART_Content' Property='HorizontalAlignment' Value='Center' />
            <Setter TargetName='PART_KeyTipText' Property='Visibility' Value='Visible' />
        </Trigger>
        <Trigger Property='Role' Value='TopLevelHeader'>
            <Setter TargetName='PART_Content' Property='HorizontalAlignment' Value='Center' />
            <Setter TargetName='PART_KeyTipText' Property='Visibility' Value='Visible' />
        </Trigger>
        <Trigger Property='IsMouseOver' Value='True'>
            <Setter TargetName='PART_Border' Property='Background'
                    Value='{{Binding ItemHoverBrush, RelativeSource={{RelativeSource AncestorType=ui:UI4Menu}}}}' />
        </Trigger>
        <Trigger Property='IsSubmenuOpen' Value='True'>
            <Setter TargetName='PART_Border' Property='Background'
                    Value='{{Binding ItemHoverBrush, RelativeSource={{RelativeSource AncestorType=ui:UI4Menu}}}}' />
        </Trigger>
        <Trigger Property='IsEnabled' Value='False'>
            <Setter Property='Opacity' Value='0.4' />
        </Trigger>
        <DataTrigger Binding='{{Binding TextIcon, RelativeSource={{RelativeSource Self}}}}' Value='{{x:Null}}'>
            <Setter TargetName='PART_IconGrid' Property='Visibility' Value='Collapsed' />
            <Setter TargetName='TextIcon' Property='Visibility' Value='Collapsed' />
        </DataTrigger>
        <DataTrigger Binding='{{Binding TextIcon, RelativeSource={{RelativeSource Self}}}}' Value=''>
            <Setter TargetName='PART_IconGrid' Property='Visibility' Value='Collapsed' />
            <Setter TargetName='TextIcon' Property='Visibility' Value='Collapsed' />
        </DataTrigger>
        <DataTrigger Binding='{{Binding KeyTip, RelativeSource={{RelativeSource Self}}}}' Value='{{x:Null}}'>
            <Setter TargetName='PART_KeyTipText' Property='Visibility' Value='Collapsed' />
        </DataTrigger>
        <DataTrigger Binding='{{Binding KeyTip, RelativeSource={{RelativeSource Self}}}}' Value=''>
            <Setter TargetName='PART_KeyTipText' Property='Visibility' Value='Collapsed' />
        </DataTrigger>
    </ControlTemplate.Triggers>
</ControlTemplate>
            </Setter.Value>
        </Setter>
        <Setter Property='ItemsPanel'>
            <Setter.Value>
                <ItemsPanelTemplate>
                    <StackPanel Orientation='Vertical' />
                </ItemsPanelTemplate>
            </Setter.Value>
        </Setter>
    </Style>
    <Style TargetType='{{x:Type ui:UI4MenuSeparatorElement}}'>
        <Setter Property='Height' Value='1' />
        <Setter Property='Margin' Value='6,4' />
        <Setter Property='MinWidth' Value='20' />
        <Setter Property='Template'>
            <Setter.Value>
                <ControlTemplate TargetType='{{x:Type ui:UI4MenuSeparatorElement}}'>
                    <Border Background='{{TemplateBinding SeparatorColor}}' />
                </ControlTemplate>
            </Setter.Value>
        </Setter>
    </Style>
</ResourceDictionary>";
            try
            {
                var parserContext = new ParserContext();
                parserContext.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                parserContext.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
                parserContext.XmlnsDictionary.Add("ui", $"clr-namespace:StartUI4Controls;assembly={assemblyName}");
                var dict = (ResourceDictionary)XamlReader.Parse(xaml, parserContext);
                if (Application.Current != null)
                {
                    Application.Current.Resources.MergedDictionaries.Add(dict);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("UI4Menu error " + ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
    public class UI4MenuElementItem : MenuItem
    {
        public static readonly DependencyProperty TextIconProperty =
            DependencyProperty.Register(
                nameof(TextIcon),
                typeof(string),
                typeof(UI4MenuElementItem),
                new PropertyMetadata(""));
        public string TextIcon
        {
            get => (string)GetValue(TextIconProperty);
            set => SetValue(TextIconProperty, value);
        }
        public static readonly DependencyProperty IconFontFamilyProperty =
            DependencyProperty.Register(
                nameof(IconFontFamily),
                typeof(FontFamily),
                typeof(UI4MenuElementItem),
                new PropertyMetadata(new FontFamily("Segoe UI Symbol")));
        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        public static readonly DependencyProperty IconFontSizeProperty =
            DependencyProperty.Register(
                nameof(IconFontSize),
                typeof(double),
                typeof(UI4MenuElementItem),
                new PropertyMetadata(14d));
        public double IconFontSize
        {
            get => (double)GetValue(IconFontSizeProperty);
            set => SetValue(IconFontSizeProperty, value);
        }
        public static readonly DependencyProperty IconForegroundProperty =
            DependencyProperty.Register(
                nameof(IconForeground),
                typeof(Brush),
                typeof(UI4MenuElementItem),
                new PropertyMetadata(null));
        public Brush IconForeground
        {
            get => (Brush)GetValue(IconForegroundProperty);
            set => SetValue(IconForegroundProperty, value);
        }
        public static readonly DependencyProperty KeyTipProperty =
            DependencyProperty.Register(
                nameof(KeyTip),
                typeof(string),
                typeof(UI4MenuElementItem),
                new PropertyMetadata(""));
        public string KeyTip
        {
            get => (string)GetValue(KeyTipProperty);
            set => SetValue(KeyTipProperty, value);
        }
        static UI4MenuElementItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UI4MenuElementItem), new FrameworkPropertyMetadata(typeof(UI4MenuElementItem)));
        }
    }
    public class UI4MenuSeparatorElement : Separator
    {
        public static readonly DependencyProperty SeparatorColorProperty =
            DependencyProperty.Register(
                nameof(SeparatorColor),
                typeof(Brush),
                typeof(UI4MenuSeparatorElement),
                new PropertyMetadata(new SolidColorBrush(Color.FromRgb(220, 220, 220))));
        public Brush SeparatorColor
        {
            get => (Brush)GetValue(SeparatorColorProperty);
            set => SetValue(SeparatorColorProperty, value);
        }
        static UI4MenuSeparatorElement()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UI4MenuSeparatorElement), new FrameworkPropertyMetadata(typeof(UI4MenuSeparatorElement)));
        }
    }
}