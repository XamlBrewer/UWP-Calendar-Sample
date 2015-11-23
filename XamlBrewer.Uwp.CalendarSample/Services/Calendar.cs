namespace Mvvm.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Appointments;
    using Windows.Foundation;
    using Windows.Storage;
    using Windows.UI.Popups;
    using Windows.UI.Xaml;

    public static partial class Calendar
    {
        public async static Task Open(DateTimeOffset dto, TimeSpan ts)
        {
            await AppointmentManager.ShowTimeFrameAsync(dto, ts);
        }

        public async static Task<string> Add(Appointment appt)
        {
            var selection = new Rect(new Point(Window.Current.Bounds.Width / 2, Window.Current.Bounds.Height / 2), new Size());
            return await Add(appt, selection);
        }

        public async static Task<string> Add(Appointment appt, Rect selection)
        {
            var id = await AppointmentManager.ShowAddAppointmentAsync(appt, selection, Placement.Default);
            AddAppointmentId(id);

            return id;
        }

        public async static Task Display(string appointmentId)
        {
            await AppointmentManager.ShowAppointmentDetailsAsync(appointmentId);
        }

        public async static Task<bool> Delete(string appointmentId, bool ignoreExceptions = false)
        {
            var selection = new Rect(new Point(Window.Current.Bounds.Width / 2, Window.Current.Bounds.Height / 2), new Size());
            try
            {
                var success = await AppointmentManager.ShowRemoveAppointmentAsync(appointmentId, selection);
                if (success)
                {
                    RemoveAppointmentId(appointmentId);
                }

                return success;
            }
            catch (Exception)
            {
                if (!ignoreExceptions)
                {
                    throw;
                }
                else
                {
                    RemoveAppointmentId(appointmentId);
                    return true;
                }
            }
        }

        public static void AddAppointmentId(string appointmentId)
        {
            if (String.IsNullOrEmpty(appointmentId))
            {
                return;
            }

            string ids = ApplicationData.Current.RoamingSettings.Values["AppointmentIds"] as string;
            if (String.IsNullOrEmpty(ids))
            {
                ids = appointmentId;
            }
            else
            {
                ids += ";" + appointmentId;
            }

            ApplicationData.Current.RoamingSettings.Values["AppointmentIds"] = ids;
        }

        public static void RemoveAppointmentId(string appointmentId)
        {
            var remainingAppts = AppointmentIds;
            remainingAppts.Remove(appointmentId);
            var newIds = string.Join(";", remainingAppts);
            ApplicationData.Current.RoamingSettings.Values["AppointmentIds"] = newIds;
        }

        public static List<string> AppointmentIds
        {
            get
            {
                string ids = ApplicationData.Current.RoamingSettings.Values["AppointmentIds"] as string;
                if (String.IsNullOrEmpty(ids))
                {
                    return new List<string>();
                }

                return new List<string>(ids.Split(';'));
            }
        }

        public async static Task CleanUp()
        {
            var appts = AppointmentIds.ToArray();
            foreach (var appt in appts)
            {
                await Delete(appt, true);
            }

            ApplicationData.Current.RoamingSettings.Values.Remove("AppointmentIds");
        }
    }
}

