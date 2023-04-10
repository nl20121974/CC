using CC.Data;

namespace CC.Helpers
{
    public class ConnectedUser
    {
        private Member? member;

        public Member? Member
        {
            get => member;
            set
            {
                member = value;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public event Action? OnChange;
}
}
