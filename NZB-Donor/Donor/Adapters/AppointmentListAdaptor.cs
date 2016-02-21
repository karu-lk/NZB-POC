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

namespace Donor.Adapters
{
    public class AppointmentListAdaptor : BaseAdapter<Appointment>
    {
        Activity context = null;
        IList<Appointment> appointments = new List<Appointment>();

        public AppointmentListAdaptor(Activity context, IList<Appointment> appointments) : base ()
		{
            this.context = context;
            this.appointments = appointments;
        }

        public override Appointment this[int position]
        {
            get { return appointments[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get { return appointments.Count; }
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            // Get our object for position
            var item = appointments[position];

            //Try to reuse convertView if it's not  null, otherwise inflate it from our item layout
            // gives us some performance gains by not always inflating a new view
            // will sound familiar to MonoTouch developers with UITableViewCell.DequeueReusableCell()
            var view = (convertView ??
                    context.LayoutInflater.Inflate(
                    Resource.Layout.TaskListItem,
                    parent,
                    false)) as LinearLayout;

            // Find references to each subview in the list item's view
            var txtName = view.FindViewById<TextView>(Resource.Id.NameText);
            var txtDescription = view.FindViewById<TextView>(Resource.Id.NotesText);

            //Assign item's values to the various subviews
            txtName.SetText(item.Name, TextView.BufferType.Normal);
            txtDescription.SetText(item.Notes, TextView.BufferType.Normal);

            //Finally return the view
            return view;
        }
    }
}