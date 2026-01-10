using Microsoft.Data.SqlClient;
using System.Data;
using QuanLyKH_TC_API.Model;

namespace QuanLyKH_TC_API.ADL
{
    public class HoaDonDAL
    {
        private readonly string _connectionString;

        public HoaDonDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<HoaDon> GetAll()
        {
            var list = new List<HoaDon>();
            const string sql = @"SELECT HoaDonID, HopDongID, SoTien, NgayLap, PhanTramDaThanhToan FROM HoaDon";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new HoaDon
                {
                    HoaDonID = Convert.ToInt32(reader["HoaDonID"]),
                    HopDongID = Convert.ToInt32(reader["HopDongID"]),
                    SoTien = reader["SoTien"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["SoTien"]),
                    NgayLap = reader["NgayLap"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayLap"]),
                    PhanTramDaThanhToan = reader["PhanTramDaThanhToan"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PhanTramDaThanhToan"])
                });
            }
            return list;
        }

        public HoaDon? GetById(int id)
        {
            const string sql = @"SELECT HoaDonID, HopDongID, SoTien, NgayLap, PhanTramDaThanhToan
                                 FROM HoaDon WHERE HoaDonID = @id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new HoaDon
            {
                HoaDonID = Convert.ToInt32(reader["HoaDonID"]),
                HopDongID = Convert.ToInt32(reader["HopDongID"]),
                SoTien = reader["SoTien"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(reader["SoTien"]),
                NgayLap = reader["NgayLap"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["NgayLap"]),
                PhanTramDaThanhToan = reader["PhanTramDaThanhToan"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PhanTramDaThanhToan"])
            };
        }

        public bool Create(HoaDon model)
        {
            const string sql = @"INSERT INTO HoaDon(HopDongID, SoTien, NgayLap, PhanTramDaThanhToan)
                                 VALUES (@HopDongID, @SoTien, @NgayLap, @PhanTramDaThanhToan)";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@HopDongID", SqlDbType.Int).Value = model.HopDongID;

                var pSoTien = cmd.Parameters.Add("@SoTien", SqlDbType.Decimal);
                pSoTien.Precision = 15;
                pSoTien.Scale = 2;
                pSoTien.Value = (object?)model.SoTien ?? DBNull.Value;

                cmd.Parameters.Add("@NgayLap", SqlDbType.Date).Value = (object?)model.NgayLap ?? DBNull.Value;
                cmd.Parameters.Add("@PhanTramDaThanhToan", SqlDbType.Int).Value = model.PhanTramDaThanhToan;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public bool Update(HoaDon model)
        {
            const string sql = @"UPDATE HoaDon
                                 SET HopDongID=@HopDongID, SoTien=@SoTien, NgayLap=@NgayLap, PhanTramDaThanhToan=@PhanTramDaThanhToan
                                 WHERE HoaDonID=@HoaDonID";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@HoaDonID", SqlDbType.Int).Value = model.HoaDonID;
                cmd.Parameters.Add("@HopDongID", SqlDbType.Int).Value = model.HopDongID;

                var pSoTien = cmd.Parameters.Add("@SoTien", SqlDbType.Decimal);
                pSoTien.Precision = 15;
                pSoTien.Scale = 2;
                pSoTien.Value = (object?)model.SoTien ?? DBNull.Value;

                cmd.Parameters.Add("@NgayLap", SqlDbType.Date).Value = (object?)model.NgayLap ?? DBNull.Value;
                cmd.Parameters.Add("@PhanTramDaThanhToan", SqlDbType.Int).Value = model.PhanTramDaThanhToan;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public bool Delete(int id)
        {
            const string sql = @"DELETE FROM HoaDon WHERE HoaDonID=@id";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }
    }
}
