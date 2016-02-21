using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
//using Donor.Core;
using Donor;

namespace Donor.Screens
{
    [Activity(Label = "NZ Blood Donor", MainLauncher = true, Icon = "@drawable/icon")]
    public class HomeScreen : Activity
    {
        //Adapters.TaskListAdapter taskList;
        Adapters.AppointmentListAdaptor appointmentList;
        IList<Appointment> appointments;
        //IList<Task> tasks;
        Button addTaskButton;
        Button addAppointment;
        ListView taskListView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.HomeScreen);

            taskListView = FindViewById<ListView>(Resource.Id.TaskList);
            //addTaskButton = FindViewById<Button>(Resource.Id.AddButton);
            addAppointment = FindViewById<Button>(Resource.Id.MakeAppointment);

            //button handler
            if (addTaskButton != null)
            {
                addTaskButton.Click += (sender, e) =>
                {
                    //StartActivity(typeof(TaskDetailsScreen));
                };
            }

            if (addAppointment != null)
            {
                addAppointment.Click += (sender, e) =>
                {
                    StartActivity(typeof(AppointmentScreen));
                };
            }

            //click handler
            if (taskListView != null)
            {
                taskListView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) =>
                {
                    //var taskDetails = new Intent(this, typeof(TaskDetailsScreen));
                    //taskDetails.PutExtra("AppointmentID", appointments[e.Position].ID);
                    //StartActivity(taskDetails);

                    var taskDetails = new Intent(this, typeof(AppointmentScreen));
                    taskDetails.PutExtra("AppointmentID", appointments[e.Position].ID);
                    StartActivity(taskDetails);
                };
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            //tasks = TaskManager.GetTasks();
            appointments = AppointmentManager.GetAppointments();

            // create our adapter
            //taskList = new Adapters.TaskListAdapter(this, tasks);
            appointmentList = new Adapters.AppointmentListAdaptor(this, appointments);

            //Hook up our adapter to our ListView
            taskListView.Adapter = appointmentList;
        }
    }
}