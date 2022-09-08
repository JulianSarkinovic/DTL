namespace DikkeTennisLijst.Core.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class TodoAttribute : Attribute
    {
        public TodoAttribute(string work)
        {
        }
    }
}