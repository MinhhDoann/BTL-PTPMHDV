using QuanLyContainer_API.Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace QuanLyContainer_API.ADL
{
    public class LoaiHangDAL
    {
        private readonly string _connectionString;

        public LoaiHangDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<LoaiHang> GetAll()
        {
            var list = new List<LoaiHang>();
            string sql = "SELECT * FROM LoaiHang";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new LoaiHang
                        {
                            LoaiHangID = reader["LoaiHangID"].ToString(),
                            TenLoai = reader["TenLoai"].ToString(),
                            MoTa = reader["MoTa"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public LoaiHang GetById(string id)
        {
            string sql = "SELECT * FROM LoaiHang WHERE LoaiHangID = @id";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new LoaiHang
                        {
                            LoaiHangID = reader["LoaiHangID"].ToString(),
                            TenLoai = reader["TenLoai"].ToString(),
                            MoTa = reader["MoTa"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        public bool Create(LoaiHang model)
        {
            string sql = @"INSERT INTO LoaiHang (TenLoai, MoTa) 
                   VALUES (@TenLoai, @MoTa)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@TenLoai", model.TenLoai ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@MoTa", model.MoTa ?? (object)DBNull.Value);
                // KHÔNG truyền LoaiHangID nữa

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool Update(LoaiHang model)
        {
            string sql = @"UPDATE LoaiHang
                   SET TenLoai = @TenLoai,
                       MoTa = @MoTa
                   WHERE LoaiHangID = @LoaiHangID";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@LoaiHangID", SqlDbType.NVarChar).Value = model.LoaiHangID;
                cmd.Parameters.Add("@TenLoai", SqlDbType.NVarChar).Value = model.TenLoai;
                cmd.Parameters.Add("@MoTa", SqlDbType.NVarChar).Value = model.MoTa;

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool Delete(string id)
        {
            string sql = "DELETE FROM LoaiHang WHERE LoaiHangID = @id";
            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.Add("@id", SqlDbType.NVarChar).Value = id;
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
