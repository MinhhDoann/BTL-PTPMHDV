using Microsoft.Data.SqlClient;
using System.Data;
using QuanLyKH_TC_API.Model;

namespace QuanLyKH_TC_API.ADL
{
    public class KhachHangDAL
    {
        private readonly string _connectionString;

        public KhachHangDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<KhachHang> GetAll()
        {
            var list = new List<KhachHang>();
            const string sql = @"SELECT KhachHangID, TenKH, DiaChi, SDT, Email FROM KhachHang";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new KhachHang
                {
                    KhachHangID = Convert.ToInt32(reader["KhachHangID"]),
                    TenKH = reader["TenKH"].ToString() ?? "",
                    DiaChi = reader["DiaChi"] as string,
                    SDT = reader["SDT"] as string,
                    Email = reader["Email"] as string
                });
            }
            return list;
        }

        public KhachHang? GetById(int id)
        {
            const string sql = @"SELECT KhachHangID, TenKH, DiaChi, SDT, Email
                                 FROM KhachHang WHERE KhachHangID = @id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return null;

            return new KhachHang
            {
                KhachHangID = Convert.ToInt32(reader["KhachHangID"]),
                TenKH = reader["TenKH"].ToString() ?? "",
                DiaChi = reader["DiaChi"] as string,
                SDT = reader["SDT"] as string,
                Email = reader["Email"] as string
            };
        }

        public bool Create(KhachHang model)
        {
            const string sql = @"INSERT INTO KhachHang(TenKH, DiaChi, SDT, Email)
                                 VALUES (@TenKH, @DiaChi, @SDT, @Email)";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@TenKH", SqlDbType.NVarChar, 100).Value = model.TenKH;
                cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 200).Value = (object?)model.DiaChi ?? DBNull.Value;
                cmd.Parameters.Add("@SDT", SqlDbType.NVarChar, 20).Value = (object?)model.SDT ?? DBNull.Value;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = (object?)model.Email ?? DBNull.Value;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public bool Update(KhachHang model)
        {
            const string sql = @"UPDATE KhachHang
                                 SET TenKH=@TenKH, DiaChi=@DiaChi, SDT=@SDT, Email=@Email
                                 WHERE KhachHangID=@KhachHangID";
            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.Add("@KhachHangID", SqlDbType.Int).Value = model.KhachHangID;
                cmd.Parameters.Add("@TenKH", SqlDbType.NVarChar, 100).Value = model.TenKH;
                cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 200).Value = (object?)model.DiaChi ?? DBNull.Value;
                cmd.Parameters.Add("@SDT", SqlDbType.NVarChar, 20).Value = (object?)model.SDT ?? DBNull.Value;
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = (object?)model.Email ?? DBNull.Value;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch { return false; }
        }

        public bool Delete(int id)
        {
            const string sql = @"DELETE FROM KhachHang WHERE KhachHangID=@id";
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
