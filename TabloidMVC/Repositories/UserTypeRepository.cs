using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class UserTypeRepository : BaseRepository
    {
        public UserTypeRepository(IConfiguration config) : base(config) { }

        public List<UserType> GetAllUserTypes()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT id, Name
                         FROM UserType";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<UserType> userTypes = new List<UserType>();
                    while (reader.Read())
                    {
                        UserType userType = new UserType
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name= reader.GetString(reader.GetOrdinal("Name"))
                          
                        };
                        userTypes.Add(userType);
                    }


                    reader.Close();

                    return userTypes;
                }
            }
        }
    }
}
