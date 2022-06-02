namespace OneRegister.Domain.Model.Shared
{
    public class DropDownListItem
    {
        public DropDownListItem()
        {
            Selected = false;
            Disabled = false;
        }
        public string Text { get; set; }
        public string Value { get; set; }
        public bool Selected { get; set; }
        public bool Disabled { get; set; }
    }
}