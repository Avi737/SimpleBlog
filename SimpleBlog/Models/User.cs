using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace SimpleBlog.Models
{
    public class User
    {

        // Basically here we're mapping in c# the table from mysql called users.
        // Every row in the table will get corresponding object User that will have the fields from the database.


        // The fakehash is for the time attack on the website, let's say someone using a registered user in the system
        // we can see by pulling up the Network the amount of time the database response with with the correct password.
        // we want to throw in fakehash incase the user is exist.

        private const int WorkFactor = 13;

        public static void FakeHash()
        {
            BCrypt.Net.BCrypt.HashPassword("", WorkFactor);
        }

        public virtual int Id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Email { get; set; }
        public virtual string PasswordHash { get; set; }

        //Roles going to contain every single role that user is part off.
        public virtual IList<Role> Roles { get; set; }

        public User()
        {
            Roles = new List<Role>();
        }

        public virtual void SetPassword(string password)
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public virtual bool CheckPassword(string password)
        {
            //this function will check if the typed in password is matched to the one we entered in the register form.
            return BCrypt.Net.BCrypt.Verify(password, PasswordHash);
        }
    }

    // How the nHibernate turns the schema of the table into our User Class (mapping)

        public class UserMap : ClassMapping<User>
        {
            //Constructor
            public UserMap()
            {
                // Here is where the mappinig happens
                //We need to tell nHibernate from which table the entity comes from.

                Table("users");

                //What property is the primary key and what is been generated through the database.
                // and if it's auto incremented.
                Id(x => x.Id, x => x.Generator(Generators.Identity));

                // Next we need to tell nHibernate about our fields
                Property(x => x.Username, x => x.NotNullable(true));
                Property(x => x.Email, x => x.NotNullable(true));

                Property(x => x.PasswordHash, x =>
                  {
                      x.Column("password_hash");
                      x.NotNullable(true);
                  });


            // Map the tag to the post in many to many fashion
            // Bag is a collection, bags are the ability to relate an entity one or more time to another entity.
            // the bag needs to know the name of the pivot table, the column that represent the user_id,role_id columns.
            // we need to tell it, it's a many to many associsation.
            Bag(x => x.Roles, x =>
            {
                x.Table("role_users");
                x.Key(k => k.Column("user_id"));
                
            }, x => x.ManyToMany(k => k.Column("role_id")));



        }
    }

}