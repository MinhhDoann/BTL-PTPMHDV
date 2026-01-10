namespace QuanLyKH_TC_API.Model
{
    public class ChiPhi
    {
        public int ChiPhiID { get; set; }
        public int? HopDongID { get; set; }
        public int? ContainerID { get; set; }
        public string? LoaiChiPhi { get; set; }
        public decimal? SoTien { get; set; }
        public string? ThuKhachHang { get; set; }
    }
}
