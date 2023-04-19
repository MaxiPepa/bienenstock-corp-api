using System;

namespace BienenstockCorpAPI
{


    public class User
    {

        private int Id { get; set; } 
        private string name { get; set; }
        private string lastName { get; set; }
        private string email { get; set; }
        private string pass { get; set; }

        public User() { }

        public User(Id,name,lastName,email,pass) {
            this.Id = Id;
            this.name = name;
            this.lastName = lastName;
            this.email = email;
            this.pass = pass;   
        }


    }
}