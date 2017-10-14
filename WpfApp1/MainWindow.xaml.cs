using Autofac;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static IContainer Container { get; set; }

        private readonly ManualResetEvent _resetEvent = new ManualResetEvent(false);

        Data data;

        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();

            var builder = new ContainerBuilder();
            builder.RegisterType<Parser>().As<IParser<Data>>();
            Container = builder.Build();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += timer_Tick;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string str = urlTextbox.Text;
            ThreadPool.QueueUserWorkItem(Parse, str);

            

            _resetEvent.WaitOne();
        }

        private void Parse(object o)
        {
            statusLabel.Dispatcher.BeginInvoke((Action)(() => statusLabel.Content = "Parsing..."));
            timer.Start();

            ClearUI();

            string str = o.ToString();

            using (var scope = Container.BeginLifetimeScope())
            {
                var parser = scope.Resolve<IParser<Data>>();

                data = parser.Parse(str);

                FillUI();
            }

            _resetEvent.Set();

            statusLabel.Dispatcher.BeginInvoke((Action)(() => statusLabel.Content = "Succseed!"));
            timer.Start();
        }

        void FillUI()
        {
            titleTb.Dispatcher.BeginInvoke((Action)(() => titleTb.Text = data.Title));
            descTb.Dispatcher.BeginInvoke((Action)(() => descTb.Text = data.Description));
            codeTb.Dispatcher.BeginInvoke((Action)(() => codeTb.Text = data.ServerResponse));
            timeTb.Dispatcher.BeginInvoke((Action)(() => timeTb.Text = data.ResponseTime.ToString()));

            foreach (var header in data.H1Headers)
            {
                headerLb.Dispatcher.BeginInvoke((Action)(() => headerLb.Items.Add(header)));
            }

            foreach (var img in data.Images)
            {
                imgLb.Dispatcher.BeginInvoke((Action)(() => imgLb.Items.Add(img)));
            }

            foreach (var link in data.Links)
            {
                linkLb.Dispatcher.BeginInvoke((Action)(() => linkLb.Items.Add(link)));
            }
        }

        void ClearUI()
        {
            titleTb.Dispatcher.BeginInvoke((Action)(() => titleTb.Text = ""));
            descTb.Dispatcher.BeginInvoke((Action)(() => descTb.Text = ""));
            codeTb.Dispatcher.BeginInvoke((Action)(() => codeTb.Text = ""));
            timeTb.Dispatcher.BeginInvoke((Action)(() => timeTb.Text = ""));

            headerLb.Dispatcher.BeginInvoke((Action)(() => headerLb.Items.Clear()));
            imgLb.Dispatcher.BeginInvoke((Action)(() => imgLb.Items.Clear()));
            linkLb.Dispatcher.BeginInvoke((Action)(() => linkLb.Items.Clear()));
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (statusLabel.Opacity <= 0)
            {
                timer.Stop();
                statusLabel.Opacity = 1;
                statusLabel.Content = "";
            }

            statusLabel.Opacity -= 0.007;
        }
    }
}
