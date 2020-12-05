using AdventOfCodeScaffolding.Util;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AdventOfCodeScaffolding.UI
{
    public partial class OutputPane : UserControl
    {
        public string Title
        {
            get => (string)GetValue(titleDp);
            set => SetValue(titleDp, value);
        }

        public string Output
        {
            get => (string)GetValue(outputDp);
            set => SetValue(outputDp, value);
        }

        public Metrics Metrics
        {
            get => (Metrics)GetValue(metricsDp);
            set => SetValue(metricsDp, value);
        }

        public TestResults TestResults
        {
            get => (TestResults)GetValue(testResultsDp);
            set => SetValue(testResultsDp, value);
        }

        public OutputPane()
        {
            DataContext = this;
            InitializeComponent();
        }

        public void Update(object result, Metrics metrics)
        {
            Output = result.ToString();
            Metrics = metrics;
        }

        private static readonly DependencyProperty titleDp =
            DependencyProperty.Register("Title",
            typeof(string), typeof(OutputPane),
            new PropertyMetadata(""));

        private static readonly DependencyProperty outputDp =
            DependencyProperty.Register("Output",
            typeof(string), typeof(OutputPane),
            new PropertyMetadata(""));

        private static readonly DependencyProperty metricsDp =
            DependencyProperty.Register("Metrics",
            typeof(Metrics), typeof(OutputPane),
            new PropertyMetadata(Metrics.Empty));

        private static readonly DependencyProperty testResultsDp =
            DependencyProperty.Register("TestResults",
            typeof(TestResults), typeof(OutputPane),
            new PropertyMetadata(TestResults.None));
    }
}
