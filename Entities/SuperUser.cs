using System;

namespace BienenstockCorpAPI
{

	public class SuperUser : User
    {
        private int Id { get; set; }
        private string name { get; set; }
        private string lastName { get; set; }
        private string email { get; set; }
        private string pass { get; set; }

        public SuperUser() { }

        public SuperUser(Id, name, lastName, email, pass)
        {
            this.Id = Id;
            this.name = name;
            this.lastName = lastName;
            this.email = email;
            this.pass = pass;
        }

        public static addEmpleyed() { }
        public static deleteEmployed() { }
        public static modifyEmployed() { }



    }

}