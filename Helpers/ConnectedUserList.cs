using System.Collections.ObjectModel;

namespace CC.Helpers
{
    public class ConnectedUserList : ObservableCollection<ConnectedUser>
    {
        public ConnectedUserList()
        {
            CollectionChanged += (s, e) => OnChange?.Invoke();
        }

        public event Action? OnChange;
    }
}
