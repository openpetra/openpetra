using System;
using System.Collections.Generic;
using System.Text;

namespace DevAge.TestApp
{
    //TODO use these method with NUnit framewor

    public static class GenericTest
    {
        public static void Run()
        {
            TestValidator();

            TestStreamDataSet();

            TestBindingDataView();

            TestBindingList();
        }

        private static void TestValidator()
        {
            DevAge.ComponentModel.Validator.ValidatorTypeConverter dblConverter = 
                new DevAge.ComponentModel.Validator.ValidatorTypeConverter(typeof(double));
            dblConverter.CultureInfo = System.Globalization.CultureInfo.InvariantCulture;

            string strVal = dblConverter.ValueToString(50.4);

            double dblVal = (double)dblConverter.StringToValue("46.78");
        }

        private static void TestStreamDataSet()
        {
            //Create a sample DataSet
            System.Data.DataSet ds = new System.Data.DataSet();
            System.Data.DataTable table = ds.Tables.Add("Table1");
            table.Columns.Add("Col1", typeof(string));
            table.Columns.Add("Col2", typeof(double));
            table.Rows.Add("Value 1", 59.7);
            table.Rows.Add("Value 2", 59.9);

            byte[] buffer;

            //Serialize the DataSet (where ds is the dataset)
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
            {
                DevAge.Data.StreamDataSet.Write(stream, ds, 
                            DevAge.Data.StreamDataSetFormat.Binary);

                buffer = stream.ToArray();
            }

            //Deserialize the DataSet (where 'buffer' is the previous serialized byte[])
            System.Data.DataSet deserializedDs = new System.Data.DataSet();
            using (System.IO.MemoryStream stream = new System.IO.MemoryStream(buffer))
            {
                DevAge.Data.StreamDataSet.Read(stream, deserializedDs, 
                            DevAge.Data.StreamDataSetFormat.Binary, true);
            }

            System.Diagnostics.Debug.Assert(deserializedDs.Tables[0].Rows.Count == 2);
        }

        private static void TestBindingDataView()
        {
            //Create a sample DataSet
            System.Data.DataSet ds = new System.Data.DataSet();
            System.Data.DataTable table = ds.Tables.Add("Table1");
            table.Columns.Add("Col1", typeof(string));
            table.Columns.Add("Col2", typeof(double));
            table.Rows.Add("Value 1", 59.7);
            table.Rows.Add("Value 2", 59.9);


            System.Data.DataView dView = table.DefaultView;

            DevAge.ComponentModel.BoundDataView bd = new DevAge.ComponentModel.BoundDataView(dView);

            bd.ListChanged += delegate(object sender, System.ComponentModel.ListChangedEventArgs e) 
                        {
                            System.Diagnostics.Debug.WriteLine("ListChanged " + dView.Count);
                        };

            System.ComponentModel.PropertyDescriptorCollection props = bd.GetItemProperties();

            int r;

            r = bd.BeginAddNew();

            bd.SetEditValue(props[0], "Pluto");
            bd.SetEditValue(props[1], 394);

            bd.EndEdit(true);

            
            r = bd.BeginAddNew();

            bd.SetEditValue(props[0], "Peter");
            bd.SetEditValue(props[1], 89);

            bd.EndEdit(false);


            bd.BeginEdit(0);
            bd.SetEditValue(props[0], "Test");
            bd.EndEdit(true);

            bd.BeginEdit(0);
            bd.SetEditValue(props[0], "Test 2");
            bd.EndEdit(false);
        }

        private static void TestBindingList()
        {
            List<Employee> list = new List<Employee>();
            Employee emp1 = new Employee();
            emp1.Name = "Peter";
            emp1.Address = "New York";
            list.Add(emp1);
            Employee emp2 = new Employee();
            emp2.Name = "John";
            emp2.Address = "Boston";
            list.Add(emp2);


            DevAge.ComponentModel.BoundList<Employee> bd = new DevAge.ComponentModel.BoundList<Employee>(list);

            bd.ListChanged += delegate(object sender, System.ComponentModel.ListChangedEventArgs e)
            {
                System.Diagnostics.Debug.WriteLine("ListChanged " + list.Count);
            };

            System.ComponentModel.PropertyDescriptorCollection props = bd.GetItemProperties();

            int r;

            r = bd.BeginAddNew();

            bd.SetEditValue(props[0], "Pluto");
            bd.SetEditValue(props[1], "Via Tripoli");

            bd.EndEdit(true);


            r = bd.BeginAddNew();

            bd.SetEditValue(props[0], "Pluto");
            bd.SetEditValue(props[1], "Corso Orbassano");

            bd.EndEdit(false);


            bd.BeginEdit(0);

            bd.SetEditValue(props[0], "Davide");

            bd.EndEdit(true);
        }

        class Employee
        {
            private string mName;

            public string Name
            {
                get { return mName; }
                set { mName = value; }
            }

            private string mAddress;

            public string Address
            {
                get { return mAddress; }
                set { mAddress = value; }
            }
        }
    }
}
