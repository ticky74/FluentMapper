using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace Hx.Extensions.FluentMapper.Testing
{
    [TestClass]
    public class MutatorTests
    {
        private Person[] CreatePersonInput()
        {
            return new Person[] {
              new Person{
                Id = 1,
                FirstName = "Scott",
                LastName = "hanselman"
                },
                new Person{
                Id = 2,
                    FirstName = "Scott",
                    LastName = "Guthrie"
                },
                new Person{
                Id = 3,
                    FirstName = "Linus",
                    LastName = "Torvalds"
                }
            };
        }

        private Programmer[] CreateProgrammerInput()
        {
            return new Programmer[] {
              new Programmer{
                Id = 1,
                FirstName = "Scott",
                LastName = "hanselman",
                FavoriteSkill = "ASP.net"
                },
                new Programmer{
                Id = 2,
                    FirstName = "Scott",
                    LastName = "Guthrie",
                    FavoriteSkill = "Architecture"
                },
                new Programmer{
                Id = 3,
                    FirstName = "Linus",
                    LastName = "Torvalds",
                    FavoriteSkill = "Shell Programming"
                }
            };
        }

        [TestMethod]
        public void ReturnTheProperlyTypedQueryableTest()
        {
            var input = CreatePersonInput();
            var result = input.Mutate().Into<Programmer>().ToArray();
            Assert.IsNotNull(result);
            Assert.AreEqual<int>(input.Length, result.Length);
            foreach(var item in result)
            {
                Assert.IsInstanceOfType(item, typeof(Programmer));
            }
        }

        [TestMethod]
        public void CastUpHierarchyIfAvailableTest()
        {
            var input = CreateProgrammerInput();
            var result = input.Mutate().Into<Person>().ToArray();
            Assert.IsNotNull(result);
            Assert.AreEqual<int>(input.Length, result.Length);
            foreach (var item in result)
            {
                Assert.IsInstanceOfType(item, typeof(Person));
                var programmer = input.Single(x => x.Id == item.Id);
                Assert.ReferenceEquals(item, programmer);
                Assert.AreEqual<string>(programmer.FirstName, item.FirstName); // Moot
                Assert.AreEqual<string>(programmer.LastName, item.LastName); // Moot
            }
        }

        [TestMethod]
        public void MutateAllMatchingTypePropertiesTest()
        {
            var input = CreatePersonInput();
            var result = input.Mutate().Into<Programmer>().ToArray();
            foreach (var item in result)
            {
                Assert.IsInstanceOfType(item, typeof(Programmer));
                var programmer = input.Single(x => x.Id == item.Id);
                Assert.AreEqual<string>(programmer.FirstName, item.FirstName); // Moot
                Assert.AreEqual<string>(programmer.LastName, item.LastName); // Moot
                Assert.IsTrue(string.IsNullOrEmpty(item.FavoriteSkill));
            }

            var output = input.Mutate().Into<Programmer>().ToArray();
        }

        [TestMethod]
        public void MutateSourceIntoUnrelatedThingWithDefaultMappingTest()
        {
            var input = CreatePersonInput();
            var result = input.Mutate().Into<UnrelatedThing>().ToArray();
            foreach (var item in result)
            {
                Assert.IsInstanceOfType(item, typeof(UnrelatedThing));
                var person = input.Single(x => x.Id == item.Id);
                Assert.AreEqual<string>(person.FirstName, item.FirstName);
                Assert.AreEqual<int>(24, item.Age);
            } 
        }
    }
}
