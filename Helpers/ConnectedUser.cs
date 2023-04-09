using CC.Data;
using Microsoft.AspNetCore.Identity;

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

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
}
}
