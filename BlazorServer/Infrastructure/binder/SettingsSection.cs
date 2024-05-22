namespace BlazorServer.Infrastructure.binder;

public static class SettingsSection
{
    public static T Configure<T>(IServiceCollection services, IConfiguration configuration, T defaultValue = null) where T : class
    {
        var sectionValue = configuration.GetSection(typeof(T).Name).Get(options =>
        {
            options.BindPropertyUsingAttributeNames = true;
        }, defaultValue);

        if (sectionValue != null)
        {
            services.AddSingleton(sectionValue);
        }

        return sectionValue;
    }

    public static T Configure<T>(string sectionName, IServiceCollection services, IConfiguration configuration, T defaultValue = null) where T : class
    {
        if (string.IsNullOrEmpty(sectionName))
        {
            sectionName = typeof(T).Name;
        }

        var sectionValue = configuration.GetSection(sectionName).Get(options =>
        {
            options.BindPropertyUsingAttributeNames = true;
        }, defaultValue);

        if (sectionValue != null)
        {
            services.AddSingleton(sectionValue);
        }

        return sectionValue;
    }

    public static T Get<T>(IConfiguration configuration, T defaultValue = null) where T : class
    {
        return configuration.GetSection(typeof(T).Name).Get(options =>
        {
            options.BindPropertyUsingAttributeNames = true;
        }, defaultValue);
    }

    public static T Get<T>(string sectionName, IConfiguration configuration, T defaultValue = null) where T : class
    {
        if (string.IsNullOrEmpty(sectionName))
        {
            sectionName = typeof(T).Name;
        }

        return configuration.GetSection(typeof(T).Name).Get(options =>
        {
            options.BindPropertyUsingAttributeNames = true;
        }, defaultValue);
    }
}
