using AdventOfCodeScaffolding.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace AdventOfCodeScaffolding
{
    public class AdventOfCodeApp : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new MainWindow().Show();
        }
    }
}
