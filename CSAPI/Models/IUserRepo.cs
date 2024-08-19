namespace CSAPI.Models
{
    public interface IUserRepo
    {
        List<User> GetAll();
        User FindById(int id);
        void AddUser(User user);
        void UpdateUser(int id, User user);
        void DeleteUser(int id);
    }

    public class UserRepo : IUserRepo
    {
        CareerSolutionsDB _context;
        public UserRepo(CareerSolutionsDB context)
        {
            _context = context;
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            //throw new NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            User user = _context.Users.Find(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            //throw new NotImplementedException();
        }

        public User FindById(int id)
        {
            return _context.Users.Find(id);
            //throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            List<User> userList = _context.Users.ToList();
            return userList;
            //throw new NotImplementedException();
        }

        public void UpdateUser(int id, User user)
        {
            User updatedUser = _context.Users.Find(id);

            updatedUser.Username = user.Username;
            updatedUser.Password = user.Password;
            updatedUser.Email = user.Email;
            updatedUser.BranchOfficeID = user.BranchOfficeID;

            _context.SaveChanges();
            //throw new NotImplementedException();
        }
    }

}
