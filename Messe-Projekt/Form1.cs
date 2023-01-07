using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Messe_Projekt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.BackColor = Color.Gray;
            label1.Text = "Bereit";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "0" && textBox5.Text != "0" && textBox6.Text != "")
            {
                Connector database = new Connector();
                database.connect("w01c9ff0.kasserver.com", "d039572e", "d039572e", "guWMJ75B6q6KpjQ", "3306");
                database.Insert(textBox1.Text, textBox2.Text, textBox3.Text, double.Parse(textBox4.Text), double.Parse(textBox5.Text), textBox6.Text, checkBox1.Checked, checkBox2.Checked, checkBox3.Checked);
                bool connectionstring = database.disconnect();
                if (connectionstring == true)
                {
                    label1.BackColor = Color.LimeGreen;
                    label1.Text = "Daten wurden gespeichert";
                }
                else
                {
                    label1.BackColor = Color.Red;
                    label1.Text = "Connection Error";
                }
            }
            else
            {
                label1.BackColor = Color.Yellow;
                label1.Text = "Eingaben ungültig!";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Connector database = new Connector();
            database.connect("w01c9ff0.kasserver.com", "d039572e", "d039572e", "guWMJ75B6q6KpjQ", "3306");
            string parameter;
            if (textBox2.Text == "")
            {
                parameter = "";
            }
            else
            {
                parameter = "WHERE `nachname` = '" + textBox2.Text + "'";
            }
            var results = database.Select(parameter);
            int numOfCols = database.GetColumns().Count;
            tableLayoutPanel1.ColumnCount = numOfCols;
            tableLayoutPanel1.Controls.Clear();
            string[] columnnames = { "ID", "Vorname", "Nachname", "Straße", "Hausnr.", "PLZ", "Wohnort", "Interesse 1", "Interesse 2", "Interesse 3"};
            for (int i = 0; i < results.Count(); i++)
            {
                Label l1 = new Label();
                l1.Text = columnnames[i].ToString();
                tableLayoutPanel1.Controls.Add(l1, i, 0);
            }                
            for (int k = 1; k < results[0].Count+1; k++)
            {
                for (int i = 0; i < results.Count(); i++)
                {
                    Label l1 = new Label();
                    l1.Text = results[i][k-1].ToString();
                    tableLayoutPanel1.Controls.Add(l1, i, k);  // add label in column0
                }
            }

            bool connectionstring = database.disconnect();
            if (connectionstring == true)
            {
                label1.BackColor = Color.LimeGreen;
                label1.Text = "Daten wurden gefunden";
            }
            else
            {
                label1.BackColor = Color.Red;
                label1.Text = "Connection Error";
            }
        }
    }
}
