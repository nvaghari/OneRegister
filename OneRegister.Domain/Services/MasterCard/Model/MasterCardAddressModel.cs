namespace OneRegister.Domain.Services.MasterCard.Model
{
    public class MasterCardAddressModel
    {
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string Addr3 { get; set; }
        public string Suburb { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string StateCode { get; set; }
        public string CountryCode { get; set; }
        public string GpsLatitude { get; set; }
        public string GpsLongitude { get; set; }
        public string AddrType { get; set; }
        public int AddrId { get; set; }
    }
}
