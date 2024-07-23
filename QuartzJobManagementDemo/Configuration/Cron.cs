namespace QuartzJobManagementDemo.Configuration
{
    public static class Cron
    {
        //public const string Never = "* * * 31 2 ?";
        public const string Never = "0 0 0 1 1 ? 2025";

        public const string EveryMinute = "0 0/1 * * * ?";

        public const string Every5Minute = "0 0/5 * * * ?";
    }
}
