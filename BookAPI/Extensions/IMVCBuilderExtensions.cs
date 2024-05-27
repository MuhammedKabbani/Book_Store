using BookAPI.Utilities.Formatters;

namespace BookAPI.Extensions
{
    public static class IMVCBuilderExtensions
    {
        public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder builder)
        {
            return builder.AddMvcOptions(config =>
            {
                config.OutputFormatters.Add(new CsvOutputFormatter());
            });
        }
    }
}
