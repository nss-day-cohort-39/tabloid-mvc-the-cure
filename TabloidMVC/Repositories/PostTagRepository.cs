using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class PostTagRepository : BaseRepository
    {
        public PostTagRepository(IConfiguration config) : base(config) { }

        public List<PostTag> GetPostTagsByPostId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT
                            pt.Id,
                            pt.PostId,
                            pt.TagId
                        FROM PostTag pt
                        JOIN Post p ON pt.PostId = p.Id
                        WHERE p.Id = @postId";

                    cmd.Parameters.AddWithValue("@postId", id);
                    var reader = cmd.ExecuteReader();
                    var postTags = new List<PostTag>();

                    while (reader.Read())
                    {
                        postTags.Add(NewPostTagFromReader(reader));
                    }

                    reader.Close();
                    return postTags;
                }
            }
        }
        private PostTag NewPostTagFromReader(SqlDataReader reader)
        {
            return new PostTag()
            {
                PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                TagId = reader.GetInt32(reader.GetOrdinal("TagId")),
            };
        }
    }
}
