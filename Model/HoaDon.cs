namespace QuanLyKH_TC_API.Model
{
    public class HoaDon
    {
        public int HoaDonID { get; set; }
        public int HopDongID { get; set; }
        public decimal? SoTien { get; set; }
        public DateTime? NgayLap { get; set; }
        public int PhanTramDaThanhToan { get; set; }
    }
}
