using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace knn
{
    public partial class Form1 : Form
    {
        #region Переменные, получаемые из файлов
        int statisticValue, classesValue, objectValue, numclass, numtich, numtest;
        List<double[]> classes,teaching, testing;
        #endregion
        public Form1()
        {
            InitializeComponent();
            statisticValue = 0;
            numclass = 0;
            numtich=0;
            numtest= 0;
            classesValue= 0;
            objectValue= 0;
            classes = new List<double[]>();
            teaching = new List<double[]>();
            testing = new List<double[]>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog ofd = new FolderBrowserDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    label1.Text = ofd.SelectedPath;
                    ReadCsvFiles(ofd.SelectedPath);
                }
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            KNN knn = new KNN(teaching, testing, numtich, numtest);
            knn.Start(listBox1,Convert.ToInt32(numericUpDown1.Value));
        }
        /// <summary>
        /// Чтение CSV файла
        /// </summary>
        /// <param name="path">Путь</param>
        void ReadCsvFiles(string path)
        {

            if (Directory.Exists(path))
            {
                string[] allfiles = Directory.GetFiles(path).OrderBy(x => x.Length).ThenBy(x => x).ToArray();
                char[] parm = new char[2] { '\r', '\n' };
                this.classesValue = allfiles.Length;
                string read = File.ReadAllText(allfiles[0]);
                string[] ster = read.Split(parm);
                List<string> karp = new List<string>();
                
                for (int i = 0; i < ster.Length; i++)
                {
                    if (ster[i].Length != 0)
                        karp.Add(ster[i]);
                }

                objectValue = karp.Count;
                string[] cons = karp[0].Split(';');
                statisticValue = cons.Length;

                for (int kl = 1; kl < allfiles.Length; kl++)
                {
                    string readText = File.ReadAllText(allfiles[kl]);
                    string[] str = readText.Split(parm);
                    List<string> krp = new List<string>();

                    for (int i = 0; i < str.Length; i++)
                    {
                        if (str[i].Length != 0)
                        {
                            krp.Add(str[i]);
                        }
                    }
                    foreach (string txt in krp)
                    {
                        string[] con = txt.Split(';');
                        double[] editclass=new double[statisticValue+1];
                        for (int i = 0; i < statisticValue+1; i++)
                        {
                            editclass[i] = Convert.ToDouble(con[i]);
                        }
                        switch(kl)
                        {
                            case 1:
                                testing.Add(editclass);
                                numtest++;
                                break;

                                case 2:
                                teaching.Add(editclass);
                                numtich++;
                                break;
                        }
                    }

                }
            }
        }
    }
}
