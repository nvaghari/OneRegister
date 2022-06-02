namespace OneRegister.Domain.Model.Shared
{
    public class ImportExcelRowValidationModel
    {
        public ImportExcelRowValidationModel()
        {
            IsAcceptable = true;
        }
        public bool IsAcceptable { get; set; }
        public string Description { get; set; }
    }
}
