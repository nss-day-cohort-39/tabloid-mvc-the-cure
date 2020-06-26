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
        public PostTag GetPostTagByPostandTag(int tagId, int postId)
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
                        WHERE pt.PostId = @postId AND pt.TagId = @tagId";

                    cmd.Parameters.AddWithValue("@postId", postId);
                    cmd.Parameters.AddWithValue("@tagId", tagId);

                    var reader = cmd.ExecuteReader();
                    PostTag postTag = null;

                    if (reader.Read())
                    {
                        postTag = NewPostTagFromReader(reader);
                    }

                    reader.Close();

                    return postTag;
                }
            }
        }
        public void AddPostTag(PostTag postTag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO PostTag (PostId, TagId)
                    OUTPUT INSERTED.Id
                    VALUES (@postId, @tagId);
                ";

                    cmd.Parameters.AddWithValue("@postId", postTag.PostId);
                    cmd.Parameters.AddWithValue("@tagId", postTag.TagId);

                    int id = (int)cmd.ExecuteScalar();

                    postTag.Id = id;
                }
            }
        }
        public void DeletePostTag(int postTagId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM PostTag
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", postTagId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        private PostTag NewPostTagFromReader(SqlDataReader reader)
        {
            return new PostTag()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                TagId = reader.GetInt32(reader.GetOrdinal("TagId")),
            };
        }
    }
}
