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

        public void Add(Category category)
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

        public void Edit(Category category)
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
                        OUTPUT INSERTED.ID
                        VALUES (
                            @Name
                        )";
                    cmd.Parameters.AddWithValue("@Name", category.Name);
                    

                    category.Id = (int)cmd.ExecuteScalar();
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
                         UPDATE CATEGORY SET WasDeleted = 1 WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", categoryId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
