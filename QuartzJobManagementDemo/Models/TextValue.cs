namespace QuartzJobManagementDemo.Models
{
    public class TextValue
    {
        public TextValue(string text, string value)
        {
            Text = text;
            Value = value;
        }

        public string Text { get; set; }
        public string Value { get; set; }
    }
}
