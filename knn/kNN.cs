using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace knn
{
    public class KNN
    {
        List<double[]> trainData;
        List<double[]> testData;
        int numtich, numtest;

        public KNN(List<double[]> trainData, List<double[]> testData, int numtich, int numtest)
        {
            this.trainData = trainData;
            this.testData = testData;
            this.numtich = numtich;
            this.numtest = numtest;
        }

        public void Start(ListBox listBox)
        {
            double[,] train = ConvertToDoubleArray(this.trainData, numtich);
            double[,] test = ConvertToDoubleArray(this.testData, numtest);
            //List<int> errorTrain = Training(train, 15, 1);
            List<int> errorTest = Testing(test, train, 15, 1);
            double accuracy = Math.Round((((double)numtest - (double)errorTest.Count()) / (double)numtest),3,MidpointRounding.AwayFromZero);
            listBox.Items.Add(errorTest.Count);
            listBox.Items.Add(accuracy.ToString());
        }
        double[,] ConvertToDoubleArray(List<double[]> tmp, int count = 0)
        {
            double[,] result = new double[22, count];
            int tp = 0;
            foreach (double[] tmpItem in tmp)
            {
                for (int j = 0; j < 22; j++)
                    result[j, tp] = tmpItem[j];
                tp++;
            }
            return result;
        }
        List<int> Training(double[,] trainData, int numClasses, int k, int numrow = -1)
        {
            List<int> errorteach = new List<int>();
            if (numrow == -1)
            {

            }
            else
            {

            }
            return errorteach;
        }
        List<int> Testing(double[,] testData, double[,] trainData, int numClasses, int k, int numrow = -1)
        {
            List<int> errortest = new List<int>();
            double[] tmp = new double[21];
            int l = 0;
            if (numrow == -1)
            {
                for(int i=0; i<1080; i++)
                {
                    for(int j=0; j<21; j++)
                    {
                        tmp[j] = testData[j,i];
                    }
                    int t = Classify(tmp, trainData, 15, 1);
                    if (i == 980)
                        l++;
                    if(t!=testData[21,i])
                        errortest.Add(i);
                }
            }
            else
            {

            }
            return errortest;
        }
        int Classify(double[] unknown, double[,] trainData, int numClasses, int k)
        {   
            IndexAndDistance[] info = new IndexAndDistance[numtich];
            double[] data = new double[21];
            for (int i = 0; i < numtich; i++)
            {
                IndexAndDistance curr = new IndexAndDistance();
                for (int j = 0; j < 21; j++)
                    data[j] = trainData[j, i];
                curr.idx = i;
                curr.distance = Distance(unknown, data);
                info[i] = curr;
            }
            Array.Sort(info);  // sort by distance
            int result = Vote(info, trainData, numClasses, k);
            return result;
        }

        int Vote(IndexAndDistance[] info, double[,] trainData, int numClasses, int k)
        {
            int[] votes = new int[numClasses];  // One cell per class
            for (int i = 0; i < k; ++i) // Just first k
            {
                int idx = info[i].idx;            // Which train item
                int c = (int)trainData[21, idx];   // Class in last cell
                ++votes[c];
            }
            int mostVotes = -1;
            int classWithMostVotes = -1;
            for (int j = 0; j < numClasses; ++j)
            {
                if (votes[j] > mostVotes)
                {
                    mostVotes = votes[j];
                    classWithMostVotes = j;
                }
            }
            return classWithMostVotes;
        }

        double Distance(double[] unknown, double[] data)
        {
            double sum = 0.0;
            for (int i = 0; i < unknown.Length; ++i)
                sum += (unknown[i] - data[i]) * (unknown[i] - data[i]);
            return Math.Sqrt(sum);
        }
    }
    public class IndexAndDistance : IComparable<IndexAndDistance>
    {
        public int idx;
        public double distance;

        public int CompareTo(IndexAndDistance other)
        {
            if (this.distance < other.distance) return -1;
            else if (this.distance > other.distance) return +1;
            else return 0;
        }
    }
}
