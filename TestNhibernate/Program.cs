using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNhibernate
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var DepartmentObject = new Department
                    { Name = "IT", PhoneNumber = "962788700227" }; 
                    ///  www
                    session.Save(DepartmentObject);
                    transaction.Commit();
                    Console.WriteLine("Department Created: " + DepartmentObject.Name);
                }
            }
        }
    }
    public class Employee
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string Position { get; set; }
        //public virtual List<Department> Departments { get; set; } // Represent the relation between Employee and Department
    }

    public class Department
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string PhoneNumber { get; set; }
    }

    class EmployeeMap : ClassMap<Employee>
    {
        //Constructor
        public EmployeeMap()
        {
            Id(x => x.Id);

            Map(x => x.FirstName);

            Map(x => x.Position);

            //References(x => x.Departments).Column("DepartmentId");

            Table("Employee");
        }
    }

    class DepartmentMap : ClassMap<Department>
    {
        //Constructor
        public DepartmentMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);

            Map(x => x.PhoneNumber);

            Table("Department");
        }
    }

    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)

                    InitializeSessionFactory();
                return _sessionFactory;
            }
        }

        private static void InitializeSessionFactory()
        {
            _sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008
                  .ConnectionString(
                  @"Server=(local);initial catalog=NHibernateTests;
		integrated security=true;") // Modify your ConnectionString
                              .ShowSql()
                )
                .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssemblyOf<Program>())
                .ExposeConfiguration(cfg => new SchemaExport(cfg)
                                                .Create(true, true))
                .BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
