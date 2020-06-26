using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class SubRepository: BaseRepository
    {
        public SubRepository(IConfiguration config) : base(config) { }
        public void AddSub(Sub sub)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Subscription (SubscriberUserProfileId, ProviderUserProfileId, BeginDateTime, EndDateTime)
                    OUTPUT INSERTED.id
                    VALUES (@subUserId, @provUserId, @beginDate, @endDate);
                ";

                    cmd.Parameters.AddWithValue("@subUserId", sub.SubscriberUserProfileId);
                    cmd.Parameters.AddWithValue("@provUserId", sub.ProviderUserProfileId);
                    cmd.Parameters.AddWithValue("@beginDate", sub.BeginDateTime);
                    cmd.Parameters.AddWithValue("@endDate", DBNull.Value);

                    int id = (int)cmd.ExecuteScalar();

                    sub.Id = id;
                }
            }
        }
    }
}
