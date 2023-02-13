using NudeContentsLimitInsurance.Interfaces;
using NudeContentsLimitInsurance.Models;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace NudeContentsLimitInsurance.DataAccess
{
    public class LocalDbConnector : IDataConnection
    {
        public QueryResult SaveInsuredItems(IEnumerable<InsuredItem> itemList)
        {
            QueryResult result;
            foreach (var item in itemList)
            {
                if (item.Id == null || item.Id == Guid.Empty)
                {
                    item.Id = Guid.NewGuid();
                }
            }

            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            var connectionString = config["LocalDbConnectionString"];

            var insuredItem = itemList.FirstOrDefault();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlString = $"EXEC[dbo].[CreateItem] @Id = '{insuredItem.Id}', @Name = '{insuredItem.Name}', @Value = '{insuredItem.Value}', @CategoryId = '{insuredItem.Category.Id}';";

                using (var command = new SqlCommand(sqlString, connection))
                {
                    command.CommandTimeout = 240;
                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    do
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        if (values[0] == null)
                        {
                            result = new QueryResult(false, "Error! An unexpected problem has occured. Please contact a server admin.");
                        }
                        else if (int.Parse(values[0].ToString()) < 1)
                        {
                            result = new QueryResult(false, $"Error! {values[0]} rows were affected. Inner exception: {values[1]}");
                        }
                        else
                        {
                            result = new QueryResult(true, "Success! Item saved.");
                        }
                    } while (reader.Read());
                }
            }

            return result;
        }

        public List<InsuredItem> LoadInsuredItems()
        {
            var insuredItems = new List<InsuredItem>();

            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            var connectionString = config["LocalDbConnectionString"];

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlString = "EXEC dbo.GetItems";

                using (var command = new SqlCommand(sqlString, connection))
                {
                    command.CommandTimeout = 240;
                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    do
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        if (values[0] != null)
                        {
                            insuredItems.Add(new InsuredItem
                            {
                                Id = Guid.Parse(values[0].ToString() ?? string.Empty),
                                Name = values[1].ToString(),
                                Value = values[2].ToString(),
                                Category = new Category
                                {
                                    Id = Guid.Parse(values[3].ToString() ?? string.Empty),
                                    Name = values[4].ToString(),
                                }
                            });
                        }
                    } while (reader.Read());
                }
            }

            return insuredItems;
        }

        public QueryResult DeleteItems(IEnumerable<InsuredItem> itemList)
        {
            QueryResult result;

            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            var connectionString = config["LocalDbConnectionString"];

            var insuredItem = itemList.FirstOrDefault();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlString = $"EXEC[dbo].[DeleteItem] @Id = '{insuredItem.Id}';";

                using (var command = new SqlCommand(sqlString, connection))
                {
                    command.CommandTimeout = 240;
                    SqlDataReader reader = command.ExecuteReader();

                    var dbItems = this.LoadInsuredItems();

                    if (dbItems.FirstOrDefault(x => x.Id == itemList.FirstOrDefault()?.Id) != null)
                    {
                        result = new QueryResult(false, "Error! An unexpected problem has occured. Please contact a server admin.");
                    }
                    else
                    {
                        result = new QueryResult(true, "Success! Item Deleted.");
                    }

                }
            }

            return result;
        }

        public List<Category> LoadItemCategories()
        {
            var itemCategories = new List<Category>();

            var config = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            var connectionString = config["LocalDbConnectionString"];

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sqlString = "EXEC dbo.GetCategories";

                using (var command = new SqlCommand(sqlString, connection))
                {
                    command.CommandTimeout = 240;
                    SqlDataReader reader = command.ExecuteReader();

                    reader.Read();

                    do
                    {
                        object[] values = new object[reader.FieldCount];
                        reader.GetValues(values);

                        if (values[0] != null)
                        {
                            itemCategories.Add(new Category
                            {
                                Id = Guid.Parse(values[0].ToString() ?? string.Empty),
                                Name = values[1].ToString(),
                            });
                        }
                    } while (reader.Read());
                }
            }

            return itemCategories;
        }
    }
}
