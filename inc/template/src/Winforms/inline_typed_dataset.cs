{##INLINETYPEDDATASET}
#region Local Typed DataSet

private TLocalMainTDS FMainDS;

private class TLocalMainTDS: TTypedDataSet
{
    private {#DETAILTABLE}Table Table{#DETAILTABLE}; 

    public {#DETAILTABLE}Table {#DETAILTABLE}
    {
        get
        {
            return this.Table{#DETAILTABLE};
        }
    }

    protected override void InitTables()
    {
        this.Tables.Add(new {#DETAILTABLE}Table("{#DETAILTABLE}"));
    }

    protected override void InitTables(System.Data.DataSet ds)
    {
        if ((ds.Tables.IndexOf("{#DETAILTABLE}") != -1))
        {
            this.Tables.Add(new {#DETAILTABLE}Table("{#DETAILTABLE}"));
        }
    }

    protected override void MapTables()
    {
        this.InitVars();
        base.MapTables();
        if ((this.Table{#DETAILTABLE} != null))
        {
            this.Table{#DETAILTABLE}.InitVars();
        }
    }

    public override void InitVars()
    {
        this.DataSetName = "PrivateScreenTDS";
        this.Table{#DETAILTABLE} = (({#DETAILTABLE}Table)(this.Tables["{#DETAILTABLE}"]));
    }

    protected override void InitConstraints()
    {

    }
 }
 
 #endregion