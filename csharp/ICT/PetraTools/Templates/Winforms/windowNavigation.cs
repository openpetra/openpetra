/* auto generated with nant generateWinforms from {#XAMLSRCFILE} and template windowNavigation
 *
 * DO NOT edit manually, DO NOT edit with the designer
 * use a user control if you need to modify the screen content
 *
 */
{#GPLFILEHEADER}
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Ict.Petra.Shared;
using System.Resources;
using System.Collections.Specialized;
using Mono.Unix;
using Ict.Common;
using Ict.Petra.Client.App.Core;
using Ict.Petra.Client.App.Core.RemoteObjects;
using Ict.Common.Controls;
using Ict.Petra.Client.CommonForms;

namespace {#NAMESPACE}
{

  /// auto generated: {#FORMTITLE}
  public partial class {#CLASSNAME}: System.Windows.Forms.Form
  {
    /// constructor
    public {#CLASSNAME}(IntPtr AParentFormHandle) : base()
    {
      //
      // Required for Windows Form Designer support
      //
      InitializeComponent();
    }

    {#EVENTHANDLERSIMPLEMENTATION}

    private void TFrmPetra_Closed(object sender, EventArgs e)
    {
        // TODO? Save Window position
    }
  }
}

{##ADDNAVIGATIONPANELS}
this.sptNavigation.Panel1.Controls.Add(this.{#PANELDEPARTMENTNAME});

{##ADDNAVIGATIONBUTTONS}
this.sptNavigation.Panel2.Controls.Add(this.{#BUTTONNAME});

{##BUTTONCONTROLCREATION}
this.{#BUTTONNAME} = new System.Windows.Forms.RadioButton();

{##BUTTONCONTROLINITIALISATION}
// 
// {#BUTTONNAME}
// 
this.{#BUTTONNAME}.Appearance = System.Windows.Forms.Appearance.Button;
this.{#BUTTONNAME}.Dock = System.Windows.Forms.DockStyle.Bottom;
this.{#BUTTONNAME}.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
this.{#BUTTONNAME}.ImageKey = "{#BUTTONIMAGE}";
//this.{#BUTTONNAME}.ImageList = this.imageListButtons;
this.{#BUTTONNAME}.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
this.{#BUTTONNAME}.Name = "{#BUTTONNAME}";
this.{#BUTTONNAME}.Text = "{#BUTTONLABEL}";
this.{#BUTTONNAME}.Size = new System.Drawing.Size(200, 24);

{##BUTTONCONTROLDECLARATION}
private System.Windows.Forms.RadioButton {#BUTTONNAME};

{##IMAGEBUTTONSSETKEYNAME}
this.imageListButtons.Images.SetKeyName({#IMAGEINDEX}, "{#IMAGENAME}");
