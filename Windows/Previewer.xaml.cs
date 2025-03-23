using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace ModelExporter.Windows
{
    public partial class Previewer : Window
    {
        private readonly Model3DGroup modelGroup;
        private readonly BoxVisual3D mybox;
        private readonly Model3D model;
        public Model3D OBJModel { get; set; }

        public Previewer(string filePath)
        {
            InitializeComponent();

            ModelImporter importer = new();
            System.Windows.Media.Media3D.Material material = new DiffuseMaterial(new SolidColorBrush(Colors.Beige));
            importer.DefaultMaterial = material;

            modelGroup = new Model3DGroup();
            model = importer.Load(filePath);
            modelGroup.Children.Add(model);
            this.OBJModel = modelGroup;

            mybox = new BoxVisual3D
            {
                Height = 5,
                Width = 5,
                Length = 5
            };
            m_helix_viewport.Children.Add(mybox);

            RotateTransform3D myRotateTransform = new(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 90))
            {
                CenterX = 0,
                CenterY = 0,
                CenterZ = 0
            };
            modelGroup.Transform = myRotateTransform;
            overall_grid.DataContext = this;
        }
    }
}
