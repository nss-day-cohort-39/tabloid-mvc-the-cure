using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class TagRepository: BaseRepository
    {
        public TagRepository(IConfiguration config) : base(config) { }

        public List<Tag> GetAllTags()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT t.Id, 
                              t.Name
                        FROM Tag t
                        ";
                    var reader = cmd.ExecuteReader();

                    var tags = new List<Tag>();

                    while (reader.Read())
                    {
                        tags.Add(NewTagFromReader(reader));
                    }
                    List<Tag> sortedTags = tags.OrderBy(t => t.Name).ToList();

                    reader.Close();

                    return sortedTags;
                }
            }
        }
        public List<Tag> GetTagsByPostId(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT
                            t.Id,
                            t.Name
                        FROM Tag t
                        JOIN PostTag pt ON t.Id = pt.TagId
                        JOIN POst p ON pt.PostId = p.Id
                        WHERE p.Id = @postId";

                    cmd.Parameters.AddWithValue("@postId", id);
                    var reader = cmd.ExecuteReader();
                    var tags = new List<Tag>();

                    while (reader.Read())
                    {
                        tags.Add(NewTagFromReader(reader));
                    }

                    reader.Close();
                    return tags;
                }
            }
        }
        public Tag GetTagById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT t.Id,
                               t.Name
                        FROM Tag t
                        WHERE t.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    Tag tag = null;

                    if (reader.Read())
                    {
                        tag = NewTagFromReader(reader);
                    }

                    reader.Close();

                    return tag;
                }
            }
        }
        public void AddTag(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Tag ([Name])
                    OUTPUT INSERTED.id
                    VALUES (@name);
                ";

                    cmd.Parameters.AddWithValue("@name", tag.Name);

                    int id = (int)cmd.ExecuteScalar();

                    tag.Id = id;
                }
            }
        }
        public void EditTag(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Tag
                            SET
                                [Name] = @name
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    cmd.Parameters.AddWithValue("@id", tag.Id);


                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteTag(int tagId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Tag
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", tagId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        private Tag NewTagFromReader(SqlDataReader reader)
        {
            return new Tag()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
            };
        }
    }
}
