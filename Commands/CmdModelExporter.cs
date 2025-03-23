using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using ModelExporter.Exporter.OBJExporter;
using Nice3point.Revit.Toolkit.External;
using System.IO;

namespace ModelExporter.Commands
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CmdModelExporter : ExternalCommand
    {
        public static Color DefaultColor = new(127, 127, 127);
        public override void Execute()
        {
            var fileName = Path.Combine(Path.GetTempPath(), "OBJFile.obj");
            var uiapp = ExternalCommandData.Application;
            var uidoc = uiapp.ActiveUIDocument;
            var application = uiapp.Application;
            var document = uidoc.Document;

            var set = uidoc.Selection.GetElementIds();
            int n = set.Count;

            var collector = (0 < n) ?
                new FilteredElementCollector(document, set) :
                new FilteredElementCollector(document);

            collector
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent();

            ObjExporter exporter = new();
            new Models.Exporter(exporter, collector, application.Create.NewGeometryOptions());
            exporter.ExportTo(fileName);

            TaskDialog td = new("OBJ preview")
            {
                Id = "ObjExporterId",
                MainIcon = TaskDialogIcon.TaskDialogIconInformation,
                Title = "OBJ file preview",
                TitleAutoPrefix = true,
                AllowCancellation = true,
                MainInstruction = "OBJ export is finished.",
                MainContent = "OBJ export is finished, would you like to preview this file?",
                CommonButtons = TaskDialogCommonButtons.No | TaskDialogCommonButtons.Yes
            };

            if (td.Show() == TaskDialogResult.Yes)
                new Windows.Previewer(fileName).Show();
        }
    }
}