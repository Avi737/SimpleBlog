using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;
using SimpleBlog.Models;


namespace SimpleBlog
{
    public static class Database
    {
        private const string SessionKey = "SimpleBlog.Database.SessionKey";
        private static ISessionFactory _sessionFactory;

        public static ISession Session
        {
            get { return (ISession)HttpContext.Current.Items[SessionKey]; }
        }
        public static void Configure()
        {
            // The configuartion will host our mapping and the connectionString and it will result a session factory.
            var config = new Configuration();

            // configure the connection string
            config.Configure(); // it will go to the web config and locate a special section and use it for the connection string.


            // add our mapping
            var mapper = new ModelMapper();
            mapper.AddMapping<UserMap>(); // tell the mapper about out mapping.
            mapper.AddMapping<RoleMap>();
            mapper.AddMapping<TagMap>();
            mapper.AddMapping<PostMap>();


            config.AddMapping(mapper.CompileMappingForAllExplicitlyAddedEntities());//telling our configuration about our mapper.

            // create session factory
            // going to be used by the open/close session.
            _sessionFactory = config.BuildSessionFactory();
        }

        public static void OpenSession()
        {
            // the result of openning a session will be assign to the SessionKey.
            // the session represent indevidual request to our database.
            HttpContext.Current.Items[SessionKey] = _sessionFactory.OpenSession();
        }

        public static void CloseSession()
        {
            // ISession is the interface that nHibernate using to represnt a session.
            // Basically try to give me an object at the sessionkey from Items that is a type of ISession. 
            var session = HttpContext.Current.Items[SessionKey] as ISession;
            if (session != null)
                session.Close();

            HttpContext.Current.Items.Remove(SessionKey); // remove that current session from the Items dictionary.
        }
    }
}