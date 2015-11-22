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
            set {
                this.SetProperty(ref selectedAppointmentId, value);
                this.OnPropertyChanged("HasSelection");
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
            appointment.Subject = "Man man man";

            try
            {
                var appointmentId = await Calendar.Add(appointment);

                if (appointmentId != String.Empty)
                {
                    this.OnPropertyChanged("AppointmentIds");
                    Toast.ShowInfo("Thanks for saving the appointment.");
                }
                else
                {
                    Toast.ShowError("You decided not to save the appointment.");
                }
            }
            catch (Exception ex)
            {
                // I get here way too often...
                Toast.ShowError(ex.Message);
            }
        }

        private async void GoTo_Executed()
        {
            await Calendar.Display(SelectedAppointmentId);
        }

        private async void Delete_Executed()
        {
            await Calendar.Delete(SelectedAppointmentId);
            this.OnPropertyChanged("AppointmentIds");
        }

        private async void Clean_Executed()
        {
            await Calendar.CleanUp();
            this.OnPropertyChanged("AppointmentIds");
        }
    }
}
