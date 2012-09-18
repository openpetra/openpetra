using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsSample.GridSamples
{
    [Sample("SourceGrid - Extensions", 49, "Data Binding - DataGrid Custom entities data binding")]
    public partial class frmSample49 : Form
    {
        public frmSample49()
        {
            InitializeComponent();
        }

        public class Employee
        {
            private string mFirstName = "New";
            public string FirstName
            {
                get { return mFirstName; }
                set { mFirstName = value; }
            }

            private string mLastName = "New";
            public string LastName
            {
                get { return mLastName; }
                set { mLastName = value; }
            }

            private decimal mSalary = 800;
            public decimal Salary
            {
                get { return mSalary; }
                set { mSalary = value; }
            }

            private DateTime mBirthday = DateTime.Today;
            public DateTime Birthday
            {
                get { return mBirthday; }
                set { mBirthday = value; }
            }
        }

        private List<Employee> mEmployeeList = new List<Employee>();
        private DevAge.ComponentModel.BoundList<Employee> mBoundList;

        private void frmSample49_Load(object sender, EventArgs e)
        {
            Employee emp1 = new Employee();
            emp1.Birthday = DateTime.Today.AddYears(-18);
            emp1.FirstName = "Peter";
            emp1.LastName = "Parker";
            emp1.Salary = 3500;
            mEmployeeList.Add(emp1);

            Employee emp2 = new Employee();
            emp2.Birthday = DateTime.Today.AddYears(-38);
            emp2.FirstName = "John";
            emp2.LastName = "Smith";
            emp2.Salary = 2800;
            mEmployeeList.Add(emp2);

            mBoundList = new DevAge.ComponentModel.BoundList<Employee>(mEmployeeList);
            mBoundList.ListChanged += new ListChangedEventHandler(mBoundList_ListChanged);

            dataGrid1.Columns.Add("FirstName", "FirstName", 
                                typeof(string)).Width = 100;
            dataGrid1.Columns.Add("LastName", "LastName",
                                typeof(string)).Width = 100;
            dataGrid1.Columns.Add("Salary", "Salary",
                                new SourceGrid.Cells.Editors.TextBoxCurrency(typeof(decimal))).Width = 100;
            dataGrid1.Columns.Add("Birthday", "Birthday",
                                typeof(DateTime)).Width = 100;

            dataGrid1.DataSource = mBoundList;
        }

        void mBoundList_ListChanged(object sender, ListChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.ListChangedType.ToString());
        }

        private void btViewNewEntities_Click(object sender, EventArgs e)
        {
            DevAge.ComponentModel.BoundList<Employee> changedList =
                    new DevAge.ComponentModel.BoundList<Employee>(mBoundList.AddedItems);

            changedList.AllowEdit = false;
            changedList.AllowNew = false;
            changedList.AllowDelete = false;

            gridChanges.DataSource = changedList;
        }

        private void btViewChangedEntities_Click(object sender, EventArgs e)
        {
            DevAge.ComponentModel.BoundList<Employee> changedList =
                    new DevAge.ComponentModel.BoundList<Employee>(mBoundList.EditedItems);

            changedList.AllowEdit = false;
            changedList.AllowNew = false;
            changedList.AllowDelete = false;

            gridChanges.DataSource = changedList;
        }

        private void btViewDeletedEntities_Click(object sender, EventArgs e)
        {
            DevAge.ComponentModel.BoundList<Employee> changedList =
                    new DevAge.ComponentModel.BoundList<Employee>(mBoundList.RemovedItems);

            changedList.AllowEdit = false;
            changedList.AllowNew = false;
            changedList.AllowDelete = false;

            gridChanges.DataSource = changedList;
        }
    }
}