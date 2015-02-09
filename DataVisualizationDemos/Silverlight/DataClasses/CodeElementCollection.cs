using System.Collections.ObjectModel;

namespace DataVisualizationDemos
{
    public class CodeElementCollection : ObservableCollection<CodeElement>
    {
        public CodeElementCollection()
        {
            Add(new CodeElement { Name = "Code", Lines = 400 });
            Add(new CodeElement { Name = "Comments", Lines = 200 });
            Add(new CodeElement { Name = "Whitespace", Lines = 100 });
        }
    }
}
