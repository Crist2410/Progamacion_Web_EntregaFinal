namespace Fit_Center.Models
{
    public class UserSingleton
    {

        private static UserSingleton instance;
        private User user;

        private UserSingleton(){  }

        public static UserSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UserSingleton();
                }
                return instance;
            }
        }

        public User User
        {
            get { return user; }
            set { user = value; }
        }

    }
}
