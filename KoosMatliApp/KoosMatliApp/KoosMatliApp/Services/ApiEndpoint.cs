using Microsoft.Maui.Devices;

namespace KoosMatliApp.Services;

public static class ApiEndpoint
{
    public const string DeployedBaseUrl = "";

    public static string BaseUrl => !string.IsNullOrWhiteSpace(DeployedBaseUrl) //(Microsoft Learn, 2026b).
        ? DeployedBaseUrl.TrimEnd('/') + "/"
        : DeviceInfo.Platform == DevicePlatform.Android //(Microsoft Learn, 2026a).
            ? "http://10.0.2.2:5000/" : "http://localhost:5000/"; //(Android Developers, 2026).
}
/*
 Reference List

    Microsoft Corporation, "Device Information in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/device-information

    Microsoft Corporation, "Connect to Local Web Services from Android Emulators in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/local-web-services

    Google LLC, "Configure Emulator Networking," Android Developers Documentation, 2026. [Online]. Available: https://developer.android.com/studio/run/emulator-networking
 */