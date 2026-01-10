namespace QuanLyKH_TC_API.Model
{
    public class ThanhToan
    {
        public int ThanhToanID { get; set; }
        public int HoaDonID { get; set; }
        public decimal? SoTien { get; set; }
        public string? PhuongThuc { get; set; }
        public DateTime? ThoiGian { get; set; }
    }
}
