using Microsoft.Maui.Devices;

namespace KoosMatliApp.Services;

public static class ApiEndpoint
{
    // Replace this one value with the deployed Render HTTPS URL before release.
    // Leave blank for local Windows / Android emulator development.
    public const string DeployedBaseUrl = "";

    public static string BaseUrl => !string.IsNullOrWhiteSpace(DeployedBaseUrl)
        ? DeployedBaseUrl.TrimEnd('/') + "/"
        : DeviceInfo.Platform == DevicePlatform.Android
            ? "http://10.0.2.2:5000/" : "http://localhost:5000/";
}
