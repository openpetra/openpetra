/* This is an automatically generated file:
 * Do not change manually.
 */
using Gtk;
using System;
using System.Collections;
using System.Data;
using Mono.Unix;
using Ict.Common.GTK;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Petra.Shared.Interfaces.MSysMan;
using Ict.Petra.Shared.MSysMan.Data;

namespace Ict.Petra.Client.MSysMan
{
    public partial class TFrmMaintainUsers : TFrmBrowseEdit
    {
        public TFrmMaintainUsers() : base (Catalog.GetString("Maintain Users"), 700, 500)
        {
            InitToolbar(true, true, true);
            Ict.Petra.Shared.Interfaces.MSysMan.TableMaintenance.UIConnectors.ISysManUIConnectorsTableMaintenance
            UIConnector = TRemote.MSysMan.TableMaintenance.UIConnectors.SysManTableMaintenance();
            DataTable table = UIConnector.GetData(SUserTable.GetTableDBName());
            InitGrid(table);
            AssembleAndShow();
        }
    }
}