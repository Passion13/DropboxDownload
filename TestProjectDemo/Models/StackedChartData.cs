using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestProjectDemo.Models
{
    public class StackedChartData
    {
        public static List<StackedChartData> GetData()
        {
            var data = new List<StackedChartData>();

            data.Add(new StackedChartData("A", 62, 56, 33));
            data.Add(new StackedChartData("B", 70, 30, 42));
            data.Add(new StackedChartData("C", 68, 62, 54));
            data.Add(new StackedChartData("D", 58, 65, 23));
            data.Add(new StackedChartData("E", 52, 40, 54));
            data.Add(new StackedChartData("F", 60, 36, 47));
            data.Add(new StackedChartData("G", 48, 70, 61));

            return data;
        }

        public static List<StackedChartData> GetGroupData()
        {
            var data = new List<StackedChartData>();

            data.Add(new StackedChartData("2000", 62, 56, 33, 43));
            data.Add(new StackedChartData("2001", 70, 30, 42, 52));
            data.Add(new StackedChartData("2002", 68, 62, 54, 64));
            data.Add(new StackedChartData("2003", 58, 65, 23, 33));

            return data;
        }

        public StackedChartData(string label, double value1, double value2, double value3)
        {
            this.Label = label;
            this.Value1 = value1;
            this.Value2 = value2;
            this.Value3 = value3;
        }

        public StackedChartData(string label, double value1, double value2, double value3, double value4)
        {
            this.Label = label;
            this.Value1 = value1;
            this.Value2 = value2;
            this.Value3 = value3;
            this.Value4 = value4;
        }

        public string Label { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
        public double Value3 { get; set; }
        public double Value4 { get; set; }
    }
}