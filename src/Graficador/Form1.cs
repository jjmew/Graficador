using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Graficador
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Pen p = new System.Drawing.Pen(System.Drawing.Color.Tomato);
            this.MyGraphics = this.MyGraphics== null? graphPanel.CreateGraphics() : this.MyGraphics;
           
            MathGrapher.DrawGraph a = new MathGrapher.DrawGraph(this.graphPanel.CreateGraphics(), graphPanel.Width, graphPanel.Height);
            a.DrawAxis();
            a.DrawEquation(this.txtInfixEquation.Text);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.MyGraphics = this.MyGraphics == null ? this.CreateGraphics() : this.MyGraphics;
            this.MyGraphics.Clear(Color.White);
            List<FormulaSolver.Token> tokens;
            string s; 
            FormulaSolver.TryParseInfixToPostFix(txtInfixEquation.Text, out tokens);
            //this.txtPostFix.Text = FormulaSolver.EvaluatePostFix(tokens).ToString();


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
