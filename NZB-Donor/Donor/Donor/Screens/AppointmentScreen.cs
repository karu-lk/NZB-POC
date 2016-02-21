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
//using Donor.Core;

namespace Donor.Screens
{
    [Activity(Label = "Make an Appointment", Icon = "@drawable/icon")]
    public class AppointmentScreen : Activity
    {
        Appointment appointment = new Appointment();
        Button cancelDeleteButton;
        EditText notesTextEdit;
        EditText nameTextEdit;
        Button saveButton;

        private TextView dateDisplay;
        private Button pickDate;
        private DateTime date;

        const int DATE_DIALOG_ID = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            int appointmentID = Intent.GetIntExtra("AppointmentID", 0);
            if (appointmentID > 0)
            {
                appointment = AppointmentManager.GetAppointment(appointmentID);
            }

            SetContentView(Resource.Layout.Appointment);
            nameTextEdit = FindViewById<EditText>(Resource.Id.NameText);
            notesTextEdit = FindViewById<EditText>(Resource.Id.NotesText);
            saveButton = FindViewById<Button>(Resource.Id.SaveButton);

            cancelDeleteButton = FindViewById<Button>(Resource.Id.CancelDeleteButton);

            cancelDeleteButton.Text = (appointment.ID == 0 ? "Cancel" : "Delete");

            nameTextEdit.Text = appointment.Name;
            notesTextEdit.Text = appointment.Notes;

            cancelDeleteButton.Click += (sender, e) => { CancelDelete(); };
            saveButton.Click += (sender, e) => { Save(); };
            
            #region Android Date Picker

            dateDisplay = FindViewById<TextView>(Resource.Id.dateDisplay);
            pickDate = FindViewById<Button>(Resource.Id.pickDate);

            pickDate.Click += delegate { ShowDialog(DATE_DIALOG_ID); };

            date = DateTime.Today;

            UpdateDisplay();

            #endregion
        }

        #region Android Date Picker

        private void UpdateDisplay()
        {
            dateDisplay.Text = date.ToString("d");
        }

        void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            this.date = e.Date;
            UpdateDisplay();
        }

        protected override Dialog OnCreateDialog(int id)
        {
            switch (id)
            {
                case DATE_DIALOG_ID:
                    return new DatePickerDialog(this, OnDateSet, date.Year, date.Month - 1, date.Day);
            }
            return null;
        }

        #endregion

        void Save()
        {
            appointment.Name = nameTextEdit.Text;
            appointment.Notes = notesTextEdit.Text;
            appointment.AppDate = dateDisplay.Text;
            AppointmentManager.SaveAppointment(appointment);
            Finish();
        }

        void CancelDelete()
        {
            if (appointment.ID != 0)
            {
                AppointmentManager.DeleteAppointment(appointment.ID);
            }
            Finish();
        }
    }
}