using NudeContentsLimitInsurance.Models;

namespace NudeContentsLimitInsurance.Interfaces
{
    public interface IDataConnection
    {
        public QueryResult SaveInsuredItems(IEnumerable<InsuredItem> itemList);

        public List<InsuredItem> LoadInsuredItems();

        public QueryResult DeleteItems(IEnumerable<InsuredItem> itemList);

        public List<Category> LoadItemCategories();
    }
}
