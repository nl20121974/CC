using System.Collections.ObjectModel;

namespace CC.Helpers
{
    public class ConnectedUserList
    {

        private readonly ObservableCollection<ConnectedUser> list = new();

        public ConnectedUserList()
        {
            list.CollectionChanged += (s, e) => NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public event Action? OnChange;
    }
}
