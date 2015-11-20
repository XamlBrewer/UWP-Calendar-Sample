namespace XamlBrewer.Uwp.CalendarSample.ViewModels
{
    using Mvvm;
    using Mvvm.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Windows.ApplicationModel.Appointments;
    using Windows.Foundation;
    using Windows.UI.Popups;

    class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel()
        {
            this.OpenCommand = new DelegateCommand(this.Open_Executed);
            this.AddCommand = new DelegateCommand(this.Add_Executed);
        }

        public ICommand OpenCommand { get; private set; }
        public ICommand AddCommand { get; private set; }

        private async void Open_Executed()
        {
            var tomorrow = DateTime.Now.AddDays(1);
            var duration = TimeSpan.FromHours(24);
            await AppointmentManager.ShowTimeFrameAsync(tomorrow, duration);
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
                Rect rect = new Rect(0, 0, 0, 0);

                var appointmentId = await AppointmentManager.ShowAddAppointmentAsync(appointment, rect, Placement.Default);

                if (appointmentId != String.Empty)
                {
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
    }
}
