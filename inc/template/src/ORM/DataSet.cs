// auto generated with nant generateORM
// Do not modify this file manually!
//
{#GPLFILEHEADER}

using Ict.Common;
using Ict.Common.Data;
using System;
using System.Data;
using System.Data.Odbc;
{#USINGNAMESPACES}

namespace {#NAMESPACE}
{
    {#CONTENTDATASETSANDTABLESANDROWS}
}

{##TYPEDDATASET}
 /// auto generated
[Serializable()]
public class {#DATASETNAME} : TTypedDataSet
{
    
    {#TYPEDDATATABLES}
    
    /// auto generated
    public {#DATASETNAME}() : 
            base("{#DATASETNAME}")
    {
    }
    
    /// auto generated for serialization
    public {#DATASETNAME}(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : 
            base(info, context)
    {
    }
    
    /// auto generated
    public {#DATASETNAME}(string ADatasetName) : 
            base(ADatasetName)
    {
    }

    {#TYPEDTABLEPROPERTY}
    
    /// auto generated
    public new virtual {#DATASETNAME} GetChangesTyped(bool removeEmptyTables)
    {
        return (({#DATASETNAME})(base.GetChangesTyped(removeEmptyTables)));
    }
    
    /// auto generated
    protected override void InitTables()
    {
        {#INITTABLESFRESH}
    }
    
    /// auto generated
    protected override void InitTables(System.Data.DataSet ds)
    {
        {#INITTABLESNOTALL}
    }
    
    /// auto generated
    protected override void MapTables()
    {
        this.InitVars();
        base.MapTables();
        {#MAPTABLES}
    }
    
    /// auto generated
    public override void InitVars()
    {
        this.DataSetName = "{#DATASETNAME}";
        {#INITVARSTABLE}
    }
    
    /// auto generated
    protected override void InitConstraints()
    {
        {#INITCONSTRAINTS}
        {#INITRELATIONS}
    }
}
{#IFDEF TABLELOOP}
{#TABLELOOP}
{#ENDIF TABLELOOP}

{##TYPEDTABLEPROPERTY}

/// auto generated
public {#TABLETYPENAME}Table {#TABLEVARIABLENAME}
{
    get
    {
        return this.Table{#TABLEVARIABLENAME};
    }
}

{##INITTABLESNOTALL}
if ((ds.Tables.IndexOf("{#TABLEVARIABLENAME}") != -1))
{
    this.Tables.Add(new {#TABLETYPENAME}Table("{#TABLEVARIABLENAME}"));
}

{##MAPTABLES}
if ((this.Table{#TABLEVARIABLENAME} != null))
{
    this.Table{#TABLEVARIABLENAME}.InitVars();
}

{##INITVARSTABLE}
this.Table{#TABLEVARIABLENAME} = (({#TABLETYPENAME}Table)(this.Tables["{#TABLEVARIABLENAME}"]));

{##INITCONSTRAINTS}
if (((this.Table{#TABLEVARIABLENAME2} != null) 
            && (this.Table{#TABLEVARIABLENAME1} != null)))
{
    this.FConstraints.Add(new TTypedConstraint("{#CONSTRAINTNAME}", "{#TABLEVARIABLENAME2}", new string[] {
                    {#COLUMNNAMES2}}, "{#TABLEVARIABLENAME1}", new string[] {
                    {#COLUMNNAMES1}}));
}

{##INITRELATIONS}
this.FRelations.Add(new TTypedRelation("{#RELATIONNAME}", "{#TABLEVARIABLENAMEPARENT}", new string[] {
                {#COLUMNNAMESPARENT}}, "{#TABLEVARIABLENAMECHILD}", new string[] {
                {#COLUMNNAMESCHILD}}, {#CREATECONSTRAINTS}));