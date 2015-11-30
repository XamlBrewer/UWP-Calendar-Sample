namespace XamlBrewer.Uwp.CalendarSample.ViewModels
{
    using Mvvm;
    using Mvvm.Services;
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Windows.ApplicationModel.Appointments;

    class MainPageViewModel : ViewModelBase
    {
        private string selectedAppointmentId;

        public MainPageViewModel()
        {
            this.OpenCommand = new DelegateCommand(this.Open_Executed);
            this.AddCommand = new DelegateCommand(this.Add_Executed);
            this.GoToCommand = new DelegateCommand(this.GoTo_Executed);
            this.DeleteCommand = new DelegateCommand(this.Delete_Executed);
            this.CleanCommand = new DelegateCommand(this.Clean_Executed);
        }

        public List<String> AppointmentIds { get { return Calendar.AppointmentIds; } }

        public string SelectedAppointmentId
        {
            get { return selectedAppointmentId; }
            set
            {
                this.SetProperty(ref selectedAppointmentId, value);
                this.OnPropertyChanged(nameof(HasSelection));
            }
        }

        public bool HasSelection
        {
            get { return !string.IsNullOrEmpty(selectedAppointmentId); }
        }

        public ICommand OpenCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand GoToCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand CleanCommand { get; private set; }

        private async void Open_Executed()
        {
            var tomorrow = DateTime.Now.AddDays(1);
            var duration = TimeSpan.FromHours(24);
            await Calendar.Open(tomorrow, duration);
            Toast.ShowInfo("Your calendar app should be open now.");
        }

        private async void Add_Executed()
        {
            var appointment = new Appointment();
            appointment.StartTime = DateTime.Now.AddDays(1);
            appointment.Duration = TimeSpan.FromHours(1);
            appointment.Subject = "Five Cents Session";
            appointment.Location = "Lucy's psychiatric booth";
            // Warning: you don't get an ID back with these:
            // appointment.Invitees.Add(new AppointmentInvitee());
            // appointment.Invitees[0].DisplayName = "Lucy van Pelt";
            // appointment.Invitees[0].Address = "lucy@peanuts.com";
            // appointment.Organizer = new AppointmentOrganizer();
            // appointment.Organizer.DisplayName = "Lucy van Pelt";
            // appointment.Organizer.Address = "lucy@peanuts.com";
            appointment.Sensitivity = AppointmentSensitivity.Private;
            appointment.BusyStatus = AppointmentBusyStatus.OutOfOffice;
            appointment.Details = "I need to discuss my fear of the Kite-Eating Tree with someone I can trust.";
            appointment.Reminder = TimeSpan.FromMinutes(15);

            try
            {
                var appointmentId = await Calendar.Add(appointment);

                if (appointmentId != String.Empty)
                {
                    this.OnPropertyChanged(nameof(AppointmentIds));
                    Toast.ShowInfo("Thanks for saving the appointment.");
                }
                else
                {
                    Toast.ShowError("You decided not to save the appointment.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, nameof(Add_Executed));
                Toast.ShowError("I'm not sure if this appointment was added.");
            }
        }

        private async void GoTo_Executed()
        {
            await Calendar.Display(SelectedAppointmentId);
        }

        private async void Delete_Executed()
        {
            try
            {
                await Calendar.Delete(SelectedAppointmentId);
                this.OnPropertyChanged(nameof(AppointmentIds));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, nameof(Delete_Executed));
                Toast.ShowError("I'm not sure if the appointment was deleted.");
            }
        }

        private async void Clean_Executed()
        {
            await Calendar.CleanUp();
            this.OnPropertyChanged(nameof(AppointmentIds));
        }
    }
}
