using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;




namespace RankEbay
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int counter = 0;
            string line;//x
            string headerRow = "";
            List<string> theLines = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader(@"d:\recent.csv");
            while ((line = file.ReadLine()) != null)
            {
                counter++;
                if (counter < 6) continue;
                if (counter == 6)
                {
                    counter++;
                    headerRow = line;
                    continue;
                    
                }
                theLines.Add(line);//x
  
            }

            file.Close();

            //Top Line in file is header file
            DataTable dt = new DataTable();
            List<string> hRow = new List<string>();
            
            foreach (string s in ProcessCsv(headerRow))
            {
                dt.Columns.Add(s);
            }
            dt.Columns.Add("x");

            dt.Columns[3].DataType = typeof(Int32);
            dt.Columns[4].DataType = typeof(Int32);
            dt.Columns[5].DataType = typeof(Int32);
            dt.Columns[6].DataType = typeof(Int32);
            dt.Columns[7].DataType = typeof(Int32);
            dt.Columns[8].DataType = typeof(Int32);
            dt.Columns[9].DataType = typeof(Int32);
            dt.Columns[10].DataType = typeof(Int32);
            dt.Columns[11].DataType = typeof(float);
            dt.Columns[12].DataType = typeof(Int32);
            dt.Columns[13].DataType = typeof(float);
            dt.Columns[14].DataType = typeof(Int32);

            //Now need to add the data to the table

            foreach (string s in theLines)
            {
                List<string> theData = ProcessCsv(s);
                DataRow dr = dt.NewRow();
                int i = 0;
                foreach (string ss in theData)
                {
                    string tmp = ss.Replace(",", "");
                    if (tmp == "-") tmp = "0";
                    tmp = tmp.Replace("%","");
                    if (i>2 & i<11) { dr[i] = int.Parse(tmp); i++; continue; }
                    if (i==12) { dr[i] = int.Parse(tmp); i++; continue; }
                    if (i == 11 || i ==13) { dr[i] = float.Parse(tmp); i++; continue; }
                    if (i == 14) { dr[i] = 0; i++; continue; }




                    dr[i] = tmp;
                    i++;

                }
                dt.Rows.Add(dr);

            }


            dataGridView1.DataSource = dt;





            DataView dv = dt.DefaultView;
            dv.Sort = dt.Columns[4].ColumnName;

            int rank = 1;
            foreach (DataRowView dr in dt.DefaultView)
            {
                dr[14] = rank;
                rank++;
            }

            dataGridView1.DataSource = dv;
            
            //dataGridView1.DataBind();







        }



        public static IEnumerable<string[]> ParseBasicCsv(string input)
        {
            using (var reader = new StringReader(input))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        yield return line.Split(',');
                    }
                }
            }
        }

        public static List<string> ProcessCsv(string csvInput)
        {
            using (var csvReader = new StringReader(csvInput))
            using (var parser = new NotVisualBasic.FileIO.CsvTextFieldParser(csvReader))
            {
                
                parser.HasFieldsEnclosedInQuotes = true;
               

                
                    return parser.ReadFields().ToList();
                    
                

            }
        }


    }
}
