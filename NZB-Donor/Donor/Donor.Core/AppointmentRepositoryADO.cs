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
using System.IO;

namespace Donor
{
    public class AppointmentRepositoryADO
    {
        AppointmentDatabase db = null;
        protected static string dbLocation;
        protected static AppointmentRepositoryADO me;

        static AppointmentRepositoryADO()
        {
            me = new AppointmentRepositoryADO();
        }

        protected AppointmentRepositoryADO()
        {
            // set the db location
            dbLocation = DatabaseFilePath;

            // instantiate the database	
            //db = new NZBDatabase(dbLocation);
            db = new AppointmentDatabase(dbLocation);
        }

        public static string DatabaseFilePath
        {
            get
            {
                var sqliteFilename = "NZBDatabase.db3";
                #if NETFX_CORE
				var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);
                #else

                #if SILVERLIGHT
				// Windows Phone expects a local path, not absolute
				var path = sqliteFilename;
                #else

                #if __ANDROID__
                // Just use whatever directory SpecialFolder.Personal returns
                string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); ;
                #else
				// we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
				// (they don't want non-user-generated data in Documents)
				string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
				string libraryPath = Path.Combine (documentsPath, "..", "Library"); // Library folder
                #endif
                var path = Path.Combine(libraryPath, sqliteFilename);
                #endif

                #endif
                return path;
            }
        }

        public static Appointment GetAppointment(int id)
        {
            return me.db.GetItem(id);
        }

        public static IEnumerable<Appointment> GetAppointments()
        {
            return me.db.GetItems();
        }

        public static int SaveAppointment(Appointment item)
        {
            return me.db.SaveItem(item);
        }

        public static int DeleteAppointment(int id)
        {
            return me.db.DeleteItem(id);
        }
    }
}