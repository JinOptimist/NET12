using System;
using System.Collections.Generic;
using System.Text;

namespace Net12
{
    class User
    {
        /// <summary>
        /// Birthday will be set today
        /// </summary>
        public User()
        {
            _birthdsay = DateTime.Now;
        }

        /// <summary>
        /// Birthday will be set 1 Jan
        /// </summary>
        /// <param name="age">Age of user</param>
        public User(int age)
        {
            _birthdsay = new DateTime(DateTime.Now.Year - age, 1, 1);
        }

        public User(DateTime birthday)
        {
            _birthdsay = birthday;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Pet { get; set; }

        private DateTime _birthdsay;

        public int MyAge()
        {
            return DateTime.Now.Year - _birthdsay.Year;
        }
    }
}
