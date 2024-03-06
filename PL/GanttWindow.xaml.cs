using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for GanttWindow.xaml
    /// </summary>
    public partial class GanttWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public IEnumerable<TaskGantt> GanttTasks
        {
            get { return (IEnumerable<TaskGantt>)GetValue(GanttTasksProperty); }
            set { SetValue(GanttTasksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GanttTasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GanttTasksProperty =
            DependencyProperty.Register("GanttTasks", typeof(IEnumerable<TaskGantt>), typeof(GanttWindow), new PropertyMetadata(null));


        public GanttWindow()
        {
            InitializeComponent();

                DateTime? projectStartDate=s_bl.getStartDate();
            DateTime? projectEndDate = s_bl.getEndDate();

            GanttTasks = from item in s_bl.Task.ReadAll()
                        let task =s_bl.Task.Read(item.Id)                        
                        select new TaskGantt() { Id = task.Id ,Name=task.Alias, Duration = task.RequiredEffortTime!.Value.Days,
                        TimeFromStart=(task.ScheduledDate- projectStartDate).Value.Days,TimeToEnd=(projectEndDate-task.ScheduledDate).Value.Days+ task.RequiredEffortTime!.Value.Days};

        }
}
}
