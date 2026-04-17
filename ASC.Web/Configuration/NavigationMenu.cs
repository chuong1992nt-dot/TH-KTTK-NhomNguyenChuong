namespace ASC.Web.Configuration
{
    public class MenuItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string AllowedRoles { get; set; }
    }

    public class NavigationMenu
    {
        public List<MenuItem> MenuItems { get; set; }
    }
}