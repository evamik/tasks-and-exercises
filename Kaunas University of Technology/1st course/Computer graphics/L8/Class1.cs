using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.EditorInput;


namespace L8
{
    public class Class1
    {
        [CommandMethod("pele")]
        public void pele()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.GetDocument(db);
            Editor ed = doc.Editor;

            // Užklausa skersmeniui
            PromptPointOptions bazinisTaskas_o = new PromptPointOptions("\nĮveskite bazinį tašką: ");
            bazinisTaskas_o.AllowNone = false;
            PromptPointResult bazinisTaskas_r = ed.GetPoint(bazinisTaskas_o);
            Point3d bazinisTaskas = bazinisTaskas_r.Value;


            Transaction tr = db.TransactionManager.StartTransaction();

            using (tr)
            {
                Arc arc1 = new Arc(bazinisTaskas, 10d, 270, 90);
                arc1.SetDatabaseDefaults(db);

                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);

                btr.AppendEntity(arc1);
                tr.AddNewlyCreatedDBObject(arc1, true);
                tr.Commit();
            }
        }
    }
}
