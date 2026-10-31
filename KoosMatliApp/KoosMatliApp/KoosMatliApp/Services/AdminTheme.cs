namespace KoosMatliApp.Services;

public static class AdminTheme
{
    public static void Apply(ContentPage page)
    {
        page.Resources.MergedDictionaries.Add(new AdminThemeResources.AdminColors());
        page.Resources.MergedDictionaries.Add(new AdminThemeResources.AdminControlStyles());
        page.Resources.MergedDictionaries.Add(new AdminThemeResources.AdminBrandStyles());
    }
}
