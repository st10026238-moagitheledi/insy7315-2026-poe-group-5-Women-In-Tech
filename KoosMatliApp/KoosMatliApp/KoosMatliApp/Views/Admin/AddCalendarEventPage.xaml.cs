using System;
using KoosMatliApp.Models;
using KoosMatliApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KoosMatliApp.Views.Admin;

public partial class AddCalendarEventPage : ContentPage
{
    private readonly IAdminApiService _api;

    public AddCalendarEventPage()
    {
        InitializeComponent();

        _api = App.Services!.GetRequiredService<IAdminApiService>();

        StartDatePicker.Date = DateTime.Today;
        EndDatePicker.Date = DateTime.Today;

        StartTimePicker.Time = new TimeSpan(8, 0, 0);
        EndTimePicker.Time = new TimeSpan(9, 0, 0);
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        if (string.IsNullOrWhiteSpace(TitleEntry.Text))
        {
            ErrorLabel.Text = L.T("Please enter an event title.");
            ErrorLabel.IsVisible = true;
            return;
        }

        // Convert nullable picker values into definite DateTime values.
        DateTime startDate = StartDatePicker.Date ?? DateTime.Today;
        DateTime endDate = EndDatePicker.Date ?? DateTime.Today;

        TimeSpan startTime = StartTimePicker.Time ?? new TimeSpan(8, 0, 0);
        TimeSpan endTime = EndTimePicker.Time ?? new TimeSpan(9, 0, 0);

        DateTime start = startDate.Date.Add(startTime);
        DateTime end = endDate.Date.Add(endTime);

        if (end <= start)
        {
            ErrorLabel.Text = L.T("The end date and time must be after the start.");
            ErrorLabel.IsVisible = true;
            return;
        }

        SaveButton.IsEnabled = false;

        try
        {
            var newEvent = new CalendarEvent
            {
                Title = TitleEntry.Text.Trim(),
                Description = DescriptionEditor.Text?.Trim() ?? string.Empty,
                Start = DateTime.SpecifyKind(start, DateTimeKind.Local).ToUniversalTime(),
                End = DateTime.SpecifyKind(end, DateTimeKind.Local).ToUniversalTime()
            };

            var saved = await _api.AddCalendarEventAsync(newEvent);

            if (saved is not null)
            {
                await DisplayAlertAsync(
                    L.T("Success"),
                    L.T("Calendar event saved successfully."),
                    L.T("OK"));

                await Shell.Current.GoToAsync("..");
            }
            else
            {
                ErrorLabel.Text = L.T("The API did not return the saved event.");
                ErrorLabel.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.Message;
            ErrorLabel.IsVisible = true;
        }
        finally
        {
            SaveButton.IsEnabled = true;
        }
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}