using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DevAge.TestApp
{
    [Sample("Other controls", 38, "Evaluate ADO.NET Expressions")]
    public partial class frmSample38 : Form
    {
        public frmSample38()
        {
            InitializeComponent();
        }

        private void btEval_Click(object sender, EventArgs e)
        {
            try
            {
                lblResult.Text = DevAge.Data.DataSetHelper.Eval(txtExpression1.Text, null).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            DataTable custTable = new DataTable("Customers");
            // add columns
            custTable.Columns.Add("id", typeof(int));
            custTable.Columns.Add("name", typeof(string));
            custTable.Columns.Add("min", typeof(int));
            custTable.Columns.Add("max", typeof(int));
            custTable.Columns.Add("expression", typeof(string));
            custTable.Columns.Add("result", typeof(string));

            custTable.ColumnChanged += new DataColumnChangeEventHandler(custTable_ColumnChanged);

            Random rnd = new Random();
            // add ten rows
            for (int id = 1; id <= 10; id++)
            {
                int valMin = rnd.Next(0, 1000);

                string expression = "max - min + " + DevAge.Data.DataSetHelper.ExpressionFormat(Math.Round(rnd.NextDouble(), 2));

                DataRow row = custTable.Rows.Add(
                    new object[] { id, 
                            string.Format("customer{0}", id), 
                            valMin, 
                            valMin + rnd.Next(1, 1000), 
                            expression, 
                            System.DBNull.Value} );

                CalculateResult(row);
            }

            dataGrid1.DataSource = custTable.DefaultView;

        }

        void custTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            if (e.Column.ColumnName != "result")
                CalculateResult(e.Row);
        }

        private void CalculateResult(DataRow row)
        {
            try
            {
                row["result"] = DevAge.Data.DataSetHelper.EvalRowExpression(row, (string)row["expression"]);
            }
            catch (Exception ex)
            {
                row["result"] = "ERROR:" + ex.Message;
            }
        }
    }
}