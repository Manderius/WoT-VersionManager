using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace VersionManagerUI.Data
{
    [CollectionDataContract(Name = "ManagedVersions", ItemName = "ManagedGameVersion", Namespace = "VersionManagerUI")]
    public class ManagedVersionCollection: ObservableCollection<ManagedGameVersion>
    {
    }
}
