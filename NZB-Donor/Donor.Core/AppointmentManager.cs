using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Donor
{
    public static class AppointmentManager
    {
        static AppointmentManager()
        {
        }

        public static Appointment GetAppointment(int id)
        {
            return AppointmentRepositoryADO.GetAppointment(id);
        }

        public static IList<Appointment> GetAppointments()
        {
            return new List<Appointment>(AppointmentRepositoryADO.GetAppointments());
        }

        public static int SaveAppointment(Appointment item)
        {
            return AppointmentRepositoryADO.SaveAppointment(item);
        }

        public static int DeleteAppointment(int id)
        {
            return AppointmentRepositoryADO.DeleteAppointment(id);
        }
    }
}