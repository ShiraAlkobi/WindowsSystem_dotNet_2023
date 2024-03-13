using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public readonly int PixelsPerDay = 21;


        
        public IEnumerable<TaskGantt> GanttTasks
        {
            get { return (IEnumerable<TaskGantt>)GetValue(GanttTasksProperty); }
            set { SetValue(GanttTasksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GanttTasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GanttTasksProperty =
            DependencyProperty.Register("GanttTasks", typeof(IEnumerable<TaskGantt>), typeof(GanttWindow), new PropertyMetadata(null));

        public ObservableCollection<string> WeekRanges
        {
            get { return (ObservableCollection<string>)GetValue(WeekRangesProperty); }
            set { SetValue(WeekRangesProperty, value); }
        }

        public static readonly DependencyProperty WeekRangesProperty =
            DependencyProperty.Register(nameof(WeekRanges), typeof(ObservableCollection<string>), typeof(GanttWindow), new PropertyMetadata(new ObservableCollection<string>()));



        public int WeekWidth
        {
            get { return (int)GetValue(WeekWidthProperty); }
            set { SetValue(WeekWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WeekWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WeekWidthProperty =
            DependencyProperty.Register("WeekWidth", typeof(int), typeof(GanttWindow), new PropertyMetadata(0));



        public int TotalWidth
        {
            get { return (int)GetValue(TotalWidthProperty); }
            set { SetValue(TotalWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TotalWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TotalWidthProperty =
            DependencyProperty.Register("TotalWidth", typeof(int), typeof(GanttWindow), new PropertyMetadata(0));



        public GanttWindow()
        {
            InitializeComponent();
            UpdateWeekRanges();
                DateTime? projectStartDate=s_bl.getStartDate();
                DateTime? projectEndDate = s_bl.getEndDate();

            GanttTasks = from item in s_bl.Task.ReadAll()
                         let task = s_bl.Task.Read(item.Id)
                         select new TaskGantt()
                         {
                             Id = task.Id,
                             Name = task.Alias,
                             Duration = task.RequiredEffortTime!.Value.Days * PixelsPerDay,
                             TimeFromStart = (task.ScheduledDate - projectStartDate).Value.Days * PixelsPerDay,
                             TimeToEnd = ((projectEndDate - task.ScheduledDate).Value.Days + task.RequiredEffortTime!.Value.Days) * PixelsPerDay,
                             Status = task.Status,
                             Dependencies = (from item in task.Dependencies
                                            select new BO.TaskInEngineer() { Alias=item.Alias,Id=item.Id}).ToList()
                                            
                        };
            this.DataContext = this;
        }
        private void UpdateWeekRanges()
        {
            WeekRanges.Clear();
            int count = 0;
            DateTime? currentStartDate = s_bl.getStartDate();
            DateTime? ProjectStartDate = s_bl.getStartDate();
            DateTime? currentEndDate;
            DateTime? ProjectEndDate = s_bl.getEndDate();
            while (currentStartDate <= ProjectEndDate)
            {
                count++;
                currentEndDate = currentStartDate?.AddDays(6);
                if (currentEndDate > ProjectEndDate)
                {
                    currentEndDate = ProjectEndDate;
                }

                string weekRange = $"{currentStartDate:MM/dd/yyyy} - {currentEndDate:MM/dd/yyyy}";
                WeekRanges.Add(weekRange);

                currentStartDate = currentEndDate?.AddDays(1);
            }
            TotalWidth = count * 147 + 50;
            WeekWidth = count * 147;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}  
    

