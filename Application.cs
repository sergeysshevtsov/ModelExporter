using ModelExporter.Commands;
using Nice3point.Revit.Toolkit.External;

namespace ModelExporter
{
    [UsedImplicitly]
    public class Application : ExternalApplication
    {
        public override void OnStartup()
        {
            CreateRibbon();
        }

        private void CreateRibbon()
        {
            var panel = Application.CreatePanel("Export");

            panel.AddPushButton<CmdModelExporter>("Model\nExporter")
                .SetImage("/ModelExporter;component/Resources/Icons/export16.png")
                .SetLargeImage("/ModelExporter;component/Resources/Icons/export32.png");
        }
    }
}