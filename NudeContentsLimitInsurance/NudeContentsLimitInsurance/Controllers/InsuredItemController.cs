using Microsoft.AspNetCore.Mvc;
using NudeContentsLimitInsurance.Interfaces;
using NudeContentsLimitInsurance.Models;

namespace NudeContentsLimitInsurance.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InsuredItemController : ControllerBase
    {
        private readonly ILogger<InsuredItemController> _logger;
        private IDataConnection connection { get; set; }

        public InsuredItemController(ILogger<InsuredItemController> logger, IDataConnection connection)
        {
            this._logger = logger;
            this.connection = connection;
        }

        [Route("/LoadItems")]
        [HttpGet]
        public IEnumerable<InsuredItem> LoadItems()
        {
            List<InsuredItem> insuredItemList = this.connection.LoadInsuredItems();

            if (!insuredItemList.Any())
            {
                insuredItemList = this.ProvideSampleData();
            }

            return insuredItemList;
        }

        [Route("/LoadCategories")]
        [HttpGet]
        public IEnumerable<Category> LoadCategories()
        {
            List<Category> categoryList = this.connection.LoadItemCategories();

            return categoryList;
        }

        private List<InsuredItem> ProvideSampleData()
        {
            return new List<InsuredItem>()
            {
                new InsuredItem()
                {
                    Id = Guid.NewGuid(),
                    Name = "TV",
                    Value = "2000",
                    Category = new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Electronics",
                    }
                },
                new InsuredItem()
                {
                    Id = Guid.NewGuid(),
                    Name = "Playstation",
                    Value = "400",
                    Category = new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Electronics",
                    }
                },
                new InsuredItem()
                {
                    Id = Guid.NewGuid(),
                    Name = "Shirts",
                    Value = "1100",
                    Category = new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Clothing",
                    }
                },
                new InsuredItem()
                {
                    Id = Guid.NewGuid(),
                    Name = "Knife Set",
                    Value = "500",
                    Category = new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = "Kitchen",
                    }
                },
            };
        }

        [Route("/SaveItems")]
        [HttpPost]
        public QueryResult SaveItems(IEnumerable<InsuredItem> itemList)
        {
            var result = this.connection.SaveInsuredItems(itemList);
            return result;
        }

        [Route("/DeleteItems")]
        [HttpPost]
        public QueryResult DeleteItems(IEnumerable<InsuredItem> itemList)
        {
            var result = this.connection.DeleteItems(itemList);
            return result;
        }
    }
}