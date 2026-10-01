using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace StartUI4Controls
{
    internal class UI43DSphere : ContentControl
    {
        public static readonly DependencyProperty TextureSourceProperty =
            DependencyProperty.Register(
                nameof(TextureSource),
                typeof(ImageSource),
                typeof(UI43DSphere),
                new PropertyMetadata(null, OnTextureSourceChanged));

        public ImageSource TextureSource
        {
            get => (ImageSource)GetValue(TextureSourceProperty);
            set => SetValue(TextureSourceProperty, value);
        }

        public static readonly DependencyProperty AutoRotateProperty =
            DependencyProperty.Register(
                nameof(AutoRotate),
                typeof(bool),
                typeof(UI43DSphere),
                new PropertyMetadata(true, OnAutoRotateChanged));

        public bool AutoRotate
        {
            get => (bool)GetValue(AutoRotateProperty);
            set => SetValue(AutoRotateProperty, value);
        }

        public static readonly DependencyProperty AutoRotateSpeedProperty =
            DependencyProperty.Register(
                nameof(AutoRotateSpeed),
                typeof(double),
                typeof(UI43DSphere),
                new PropertyMetadata(30.0));

        public double AutoRotateSpeed
        {
            get => (double)GetValue(AutoRotateSpeedProperty);
            set => SetValue(AutoRotateSpeedProperty, value);
        }

        public static readonly DependencyProperty RotationAxisProperty =
            DependencyProperty.Register(
                nameof(RotationAxis),
                typeof(Vector3D),
                typeof(UI43DSphere),
                new PropertyMetadata(new Vector3D(0, 1, 0)));

        public Vector3D RotationAxis
        {
            get => (Vector3D)GetValue(RotationAxisProperty);
            set => SetValue(RotationAxisProperty, value);
        }

        public static readonly DependencyProperty AutoRotateDirectionProperty =
            DependencyProperty.Register(
                nameof(AutoRotateDirection),
                typeof(double),
                typeof(UI43DSphere),
                new PropertyMetadata(100.0));

        public double AutoRotateDirection
        {
            get => (double)GetValue(AutoRotateDirectionProperty);
            set => SetValue(AutoRotateDirectionProperty, value);
        }

        public static readonly DependencyProperty DragSensitivityProperty =
            DependencyProperty.Register(
                nameof(DragSensitivity),
                typeof(double),
                typeof(UI43DSphere),
                new PropertyMetadata(100.0));

        public double DragSensitivity
        {
            get => (double)GetValue(DragSensitivityProperty);
            set => SetValue(DragSensitivityProperty, value);
        }

        public static readonly DependencyProperty SphereRadiusProperty =
            DependencyProperty.Register(
                nameof(SphereRadius),
                typeof(double),
                typeof(UI43DSphere),
                new PropertyMetadata(1.0, OnSphereRadiusChanged));

        public double SphereRadius
        {
            get => (double)GetValue(SphereRadiusProperty);
            set => SetValue(SphereRadiusProperty, value);
        }

        public static readonly DependencyProperty MeshSlicesProperty =
            DependencyProperty.Register(
                nameof(MeshSlices),
                typeof(int),
                typeof(UI43DSphere),
                new PropertyMetadata(64, OnMeshChanged));

        public int MeshSlices
        {
            get => (int)GetValue(MeshSlicesProperty);
            set => SetValue(MeshSlicesProperty, value);
        }

        public static readonly DependencyProperty MeshStacksProperty =
            DependencyProperty.Register(
                nameof(MeshStacks),
                typeof(int),
                typeof(UI43DSphere),
                new PropertyMetadata(32, OnMeshChanged));

        public int MeshStacks
        {
            get => (int)GetValue(MeshStacksProperty);
            set => SetValue(MeshStacksProperty, value);
        }

        public static readonly DependencyProperty DiffuseColorProperty =
            DependencyProperty.Register(
                nameof(DiffuseColor),
                typeof(Color),
                typeof(UI43DSphere),
                new PropertyMetadata(Colors.White, OnDiffuseColorChanged));

        public Color DiffuseColor
        {
            get => (Color)GetValue(DiffuseColorProperty);
            set => SetValue(DiffuseColorProperty, value);
        }

        public static readonly DependencyProperty SpecularColorProperty =
            DependencyProperty.Register(
                nameof(SpecularColor),
                typeof(Color),
                typeof(UI43DSphere),
                new PropertyMetadata(Colors.White, OnSpecularColorChanged));

        public Color SpecularColor
        {
            get => (Color)GetValue(SpecularColorProperty);
            set => SetValue(SpecularColorProperty, value);
        }

        public static readonly DependencyProperty SpecularPowerProperty =
            DependencyProperty.Register(
                nameof(SpecularPower),
                typeof(double),
                typeof(UI43DSphere),
                new PropertyMetadata(80.0, OnSpecularPowerChanged));

        public double SpecularPower
        {
            get => (double)GetValue(SpecularPowerProperty);
            set => SetValue(SpecularPowerProperty, value);
        }

        public static readonly DependencyProperty LightIntensityProperty =
            DependencyProperty.Register(
                nameof(LightIntensity),
                typeof(double),
                typeof(UI43DSphere),
                new PropertyMetadata(1.0, OnLightIntensityChanged));

        public double LightIntensity
        {
            get => (double)GetValue(LightIntensityProperty);
            set => SetValue(LightIntensityProperty, value);
        }

        public static readonly DependencyProperty AmbientIntensityProperty =
            DependencyProperty.Register(
                nameof(AmbientIntensity),
                typeof(double),
                typeof(UI43DSphere),
                new PropertyMetadata(0.3, OnAmbientIntensityChanged));

        public double AmbientIntensity
        {
            get => (double)GetValue(AmbientIntensityProperty);
            set => SetValue(AmbientIntensityProperty, value);
        }

        private Viewport3D _viewport;
        private PerspectiveCamera _camera;
        private ModelVisual3D _sphereVisual;
        private GeometryModel3D _geometryModel;
        private MeshGeometry3D _sphereMesh;
        private DiffuseMaterial _diffuseMaterial;
        private SpecularMaterial _specularMaterial;
        private MaterialGroup _materialGroup;
        private QuaternionRotation3D _quaternionRotation;
        private RotateTransform3D _rotateTransform;
        private DirectionalLight _mainLight;
        private AmbientLight _ambientLight;

        private Point _lastMousePos;
        private bool _isDragging;
        private Quaternion _accumulatedRotation = Quaternion.Identity;
        private Quaternion _autoRotationAccum = Quaternion.Identity;
        private DispatcherTimer _autoRotateTimer;
        private DateTime _lastAutoRotateTick;

        public UI43DSphere()
        {
            BuildVisualTree();
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        private void BuildVisualTree()
        {
            _viewport = new Viewport3D
            {
                ClipToBounds = true
            };

            _camera = new PerspectiveCamera
            {
                Position = new Point3D(0, 0, 4),
                LookDirection = new Vector3D(0, 0, -1),
                UpDirection = new Vector3D(0, 1, 0),
                FieldOfView = 45
            };
            _viewport.Camera = _camera;

            var modelGroup = new Model3DGroup();

            _ambientLight = new AmbientLight(
                Color.FromRgb(
                    (byte)(255 * AmbientIntensity),
                    (byte)(255 * AmbientIntensity),
                    (byte)(255 * AmbientIntensity)));
            modelGroup.Children.Add(_ambientLight);

            _mainLight = new DirectionalLight(
                Color.FromRgb(
                    (byte)(255 * LightIntensity),
                    (byte)(255 * LightIntensity),
                    (byte)(255 * LightIntensity)),
                new Vector3D(-1, -1, -2));
            modelGroup.Children.Add(_mainLight);

            _sphereMesh = CreateSphereMesh(SphereRadius, MeshSlices, MeshStacks);

            _diffuseMaterial = new DiffuseMaterial(
                new SolidColorBrush(DiffuseColor));
            _specularMaterial = new SpecularMaterial(
                new SolidColorBrush(SpecularColor),
                SpecularPower);
            _materialGroup = new MaterialGroup();
            _materialGroup.Children.Add(_diffuseMaterial);
            _materialGroup.Children.Add(_specularMaterial);

            _geometryModel = new GeometryModel3D
            {
                Geometry = _sphereMesh,
                Material = _materialGroup,
                BackMaterial = _materialGroup
            };

            _quaternionRotation = new QuaternionRotation3D(_accumulatedRotation);
            _rotateTransform = new RotateTransform3D(_quaternionRotation);

            var transformGroup = new Transform3DGroup();
            transformGroup.Children.Add(_rotateTransform);
            _geometryModel.Transform = transformGroup;

            var lightModelGroup = new Model3DGroup();
            lightModelGroup.Children.Add(modelGroup);
            lightModelGroup.Children.Add(_geometryModel);

            _sphereVisual = new ModelVisual3D { Content = lightModelGroup };
            _viewport.Children.Add(_sphereVisual);

            _viewport.MouseLeftButtonDown += OnMouseDown;
            _viewport.MouseMove += OnMouseMove;
            _viewport.MouseLeftButtonUp += OnMouseUp;
            _viewport.MouseLeave += OnMouseLeave;

            _autoRotateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };
            _autoRotateTimer.Tick += OnAutoRotateTick;

            this.Content = _viewport;
        }

        private MeshGeometry3D CreateSphereMesh(double radius, int slices, int stacks)
        {
            var mesh = new MeshGeometry3D();

            slices = Math.Max(3, slices);
            stacks = Math.Max(2, stacks);

            for (int i = 0; i <= stacks; i++)
            {
                double phi = Math.PI * i / stacks;
                double y = radius * Math.Cos(phi);
                double ringRadius = radius * Math.Sin(phi);

                for (int j = 0; j <= slices; j++)
                {
                    double theta = 2 * Math.PI * j / slices;
                    double x = ringRadius * Math.Cos(theta);
                    double z = ringRadius * Math.Sin(theta);

                    mesh.Positions.Add(new Point3D(x, y, z));
                    mesh.TextureCoordinates.Add(new Point((double)j / slices, (double)i / stacks));
                }
            }

            for (int i = 0; i < stacks; i++)
            {
                for (int j = 0; j < slices; j++)
                {
                    int topLeft = i * (slices + 1) + j;
                    int topRight = topLeft + 1;
                    int bottomLeft = (i + 1) * (slices + 1) + j;
                    int bottomRight = bottomLeft + 1;

                    mesh.TriangleIndices.Add(topLeft);
                    mesh.TriangleIndices.Add(bottomLeft);
                    mesh.TriangleIndices.Add(topRight);

                    mesh.TriangleIndices.Add(topRight);
                    mesh.TriangleIndices.Add(bottomLeft);
                    mesh.TriangleIndices.Add(bottomRight);
                }
            }

            return mesh;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (AutoRotate)
            {
                _lastAutoRotateTick = DateTime.Now;
                _autoRotateTimer.Start();
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _autoRotateTimer.Stop();
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _isDragging = true;
                _lastMousePos = e.GetPosition(_viewport);
                _viewport.Cursor = Cursors.Hand;
                Mouse.Capture(_viewport);
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;

            Point currentPos = e.GetPosition(_viewport);
            Vector delta = currentPos - _lastMousePos;
            _lastMousePos = currentPos;

            if (delta.Length < 0.5) return;

            double sensitivity = DragSensitivity * 0.005;
            Quaternion deltaRotation = ComputeTrackballRotation(delta, sensitivity);
            _accumulatedRotation = deltaRotation * _accumulatedRotation;
            ApplyRotation();
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                _viewport.Cursor = null;
                Mouse.Capture(null);
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                _viewport.Cursor = null;
                Mouse.Capture(null);
            }
        }

        private Quaternion ComputeTrackballRotation(Vector delta, double sensitivity)
        {
            double angleX = delta.Y * sensitivity;
            double angleY = delta.X * sensitivity;

            Quaternion rotX = new Quaternion(new Vector3D(1, 0, 0), angleX);
            Quaternion rotY = new Quaternion(new Vector3D(0, 1, 0), angleY);

            return rotY * rotX;
        }

        private void OnAutoRotateTick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            double elapsed = (now - _lastAutoRotateTick).TotalSeconds;
            _lastAutoRotateTick = now;

            if (elapsed <= 0 || elapsed > 0.5) elapsed = 0.016;

            double angle = AutoRotateSpeed * elapsed * AutoRotateDirection * Math.PI / 180.0;
            if (Math.Abs(angle) < 0.0001) return;

            Vector3D axis = RotationAxis;
            if (axis.LengthSquared < 0.0001) axis = new Vector3D(0, 1, 0);
            axis.Normalize();

            Quaternion deltaRotation = new Quaternion(axis, angle);
            _autoRotationAccum = deltaRotation * _autoRotationAccum;
            ApplyRotation();
        }

        private void ApplyRotation()
        {
            _quaternionRotation.Quaternion = _autoRotationAccum * _accumulatedRotation;
        }

        private void RebuildMesh()
        {
            if (_sphereMesh == null) return;
            _sphereMesh.Positions.Clear();
            _sphereMesh.TextureCoordinates.Clear();
            _sphereMesh.TriangleIndices.Clear();

            var newMesh = CreateSphereMesh(SphereRadius, MeshSlices, MeshStacks);
            foreach (var pos in newMesh.Positions)
                _sphereMesh.Positions.Add(pos);
            foreach (var tc in newMesh.TextureCoordinates)
                _sphereMesh.TextureCoordinates.Add(tc);
            foreach (var idx in newMesh.TriangleIndices)
                _sphereMesh.TriangleIndices.Add(idx);
        }

        private static void OnTextureSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (UI43DSphere)d;
            if (control._diffuseMaterial != null)
            {
                if (e.NewValue is ImageSource imgSource)
                {
                    control._diffuseMaterial.Brush = new ImageBrush(imgSource)
                    {
                        ViewportUnits = BrushMappingMode.Absolute
                    };
                }
                else
                {
                    control._diffuseMaterial.Brush = new SolidColorBrush(control.DiffuseColor);
                }
            }
        }

        private static void OnAutoRotateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (UI43DSphere)d;
            if ((bool)e.NewValue)
            {
                control._lastAutoRotateTick = DateTime.Now;
                control._autoRotateTimer?.Start();
            }
            else
            {
                control._autoRotateTimer?.Stop();
            }
        }

        private static void OnSphereRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (UI43DSphere)d;
            control.RebuildMesh();
        }

        private static void OnMeshChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (UI43DSphere)d;
            control.RebuildMesh();
        }

        private static void OnDiffuseColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (UI43DSphere)d;
            if (control._diffuseMaterial != null && control.TextureSource == null)
            {
                control._diffuseMaterial.Brush = new SolidColorBrush((Color)e.NewValue);
            }
        }

        private static void OnSpecularColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (UI43DSphere)d;
            if (control._specularMaterial != null)
            {
                control._specularMaterial.Brush = new SolidColorBrush((Color)e.NewValue);
            }
        }

        private static void OnSpecularPowerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (UI43DSphere)d;
            if (control._specularMaterial != null)
            {
                control._specularMaterial.SpecularPower = (double)e.NewValue;
            }
        }

        private static void OnLightIntensityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (UI43DSphere)d;
            if (control._mainLight != null)
            {
                double intensity = (double)e.NewValue;
                byte b = (byte)Math.Max(0, Math.Min(255, (int)(255 * intensity)));
                control._mainLight.Color = Color.FromRgb(b, b, b);
            }
        }

        private static void OnAmbientIntensityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (UI43DSphere)d;
            if (control._ambientLight != null)
            {
                double intensity = (double)e.NewValue;
                byte b = (byte)Math.Max(0, Math.Min(255, (int)(255 * intensity)));
                control._ambientLight.Color = Color.FromRgb(b, b, b);
            }
        }
    }
}