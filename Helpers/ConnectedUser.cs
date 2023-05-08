using CC.Data;

namespace CC.Helpers
{
    public class ConnectedUser
    {
        private UserProfile? userProfile;

        public UserProfile? UserProfile
        {
            get => userProfile;
            set
            {
                userProfile = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public event Action? OnChange;
}
}
