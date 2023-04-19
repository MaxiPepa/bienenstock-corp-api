using System;


namespace BienenstockCorpAPI
{
    public class Product
    {
        
       private int Id_product { get; set; }
       private string name { get; set; }
       private DateTime dateIn { get; set; }
       private float price { get; set; }

       public Product() { }

       public Product(Id_product,name,dateIn,price) {
            this.Id_product = Id_product;
            this.name = name;
            this.dateIn = dateIn;
            this.price = price;
       }



    }

}

