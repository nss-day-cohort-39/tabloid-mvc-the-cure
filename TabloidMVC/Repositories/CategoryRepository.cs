using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class CategoryRepository : BaseRepository
    {
        public CategoryRepository(IConfiguration config) : base(config) { }
        public List<Category> GetAllCategories()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT c.Id, 
                              c.Name
                        FROM Category c
                        ";
                    var reader = cmd.ExecuteReader();

                    var categories = new List<Category>();

                    while (reader.Read())
                    {
                        categories.Add(NewCategoryFromReader(reader));
                    }
                    List<Category> filteredCategories = categories.OrderBy(c => c.Name).ToList();

                    reader.Close();

                    return filteredCategories;
                }
            }
        }

        private Category NewCategoryFromReader(SqlDataReader reader)
        {
            return new Category()
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
            };
        }

        public Category GetCategoryById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT c.Id, 
                              c.Name
                         FROM Category c
                         WHERE c.id = @id 
                       ";

                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    Category category = null;

                    if (reader.Read())
                    {
                        category = NewCategoryFromReader(reader);
                    }

                    reader.Close();

                    return category;
                }
            }
        }

        public void CreateCategory(Category category)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Category (
                           Name
                        )
                        OUTPUT INSERTED.Id
                        VALUES (
                            @Name 
                        )";
                    cmd.Parameters.AddWithValue("@Name", category.Name);

                    category.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void UpdateCategory(Category category)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Category 
                            SET [Name] = @Name
                            WHERE Id = @id"
                            ;
                    cmd.Parameters.AddWithValue("@Name", category.Name);
                    cmd.Parameters.AddWithValue("@id", category.Id);


                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCategory(int categoryId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                         DELETE FROM Category WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", categoryId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
