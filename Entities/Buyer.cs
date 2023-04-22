using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace BienenstockCorpAPI
{

	public class Buyer : User
	{

        private int Id { get; set; }
        private string name { get; set; }
        private string lastName { get; set; }
        private string email { get; set; }
        private string pass { get; set; }

        public Buyer() { }

        public Buyer(Id, name, lastName, email, pass)
        {
            this.Id = Id;
            this.name = name;
            this.lastName = lastName;
            this.email = email;
            this.pass = pass;
        }

        public static buyProduct() { }
        public static payProduct() { }
      

    }
}
