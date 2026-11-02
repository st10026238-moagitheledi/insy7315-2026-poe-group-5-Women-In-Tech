namespace KoosMatliApp.Services;

public static class AdminTheme
{
    public static void Apply(ContentPage page)
    {
        page.Resources.MergedDictionaries.Add(new AdminThemeResources.AdminColors()); //(Microsoft Learn, 2026a).

        page.Resources.MergedDictionaries.Add(new AdminThemeResources.AdminControlStyles());
        page.Resources.MergedDictionaries.Add(new AdminThemeResources.AdminBrandStyles()); //(Microsoft Learn, 2026b).
    }
}
/*Reference List
 
 Microsoft Corporation, "Resource Dictionaries in .NET MAUI," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/resource-dictionaries

 Microsoft Corporation, "Styling .NET MAUI Apps Using XAML," Microsoft Learn, 2026. [Online]. Available: https://learn.microsoft.com/en-us/dotnet/maui/user-interface/styles/xaml
 */