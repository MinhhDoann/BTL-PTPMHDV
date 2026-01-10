namespace QuanLyKH_TC_API.Model
{
    public class HopDong
    {
        public int HopDongID { get; set; }
        public int KhachHangID { get; set; }
        public DateTime NgayKy { get; set; }
        public string? LoaiDichVu { get; set; }
        public decimal? GiaTri { get; set; }
        public string? TrangThai { get; set; }
    }
}
