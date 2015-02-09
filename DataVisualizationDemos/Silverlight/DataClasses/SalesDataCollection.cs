using System.Collections.ObjectModel;

namespace DataVisualizationDemos
{
    public class SalesDataCollection : Collection<SalesData>
    {
        public SalesDataCollection()
        {
            Add(new SalesData { Animal = "Dogs", WestStoreQuantity = 5, EastStoreQuantity = 7 });
            Add(new SalesData { Animal = "Cats", WestStoreQuantity = 5, EastStoreQuantity = 6 });
            Add(new SalesData { Animal = "Birds", WestStoreQuantity = 3, EastStoreQuantity = 8 });
            Add(new SalesData { Animal = "Fish", WestStoreQuantity = 6, EastStoreQuantity = 9 });
        }
    }
}
