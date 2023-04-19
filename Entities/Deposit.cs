using System;

namespace BienenstockCorpAPI
{


    public class Deposit : User
    {

        private int Id { get; set; }
        private string name { get; set; }
        private string lastName { get; set; }
        private string email { get; set; }
        private string pass { get; set; }

        public Deposit() { }

        public Deposit(Id, name, lastName, email, pass)
        {
            this.Id = Id;
            this.name = name;
            this.lastName = lastName;
            this.email = email;
            this.pass = pass;
        }

        public static addProduct() { }
        public static deleteProduct() { }
        public static modifyProduct() { }


    }

}
